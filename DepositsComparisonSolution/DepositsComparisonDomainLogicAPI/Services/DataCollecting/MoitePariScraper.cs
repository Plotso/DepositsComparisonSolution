namespace DepositsComparisonDomainLogicAPI.Services.DataCollecting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AngleSharp;
    using AngleSharp.Dom;
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparison.Data.Public;
    using DepositsComparisonDomainLogic.Contracts.Models;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using Models.DataCollecting;

    public class MoitePariScraper : AngleSharpScraper, IWebScraper
    {
        private const string BankProductsURL = @"https://www.moitepari.bg";
        private const string DepositsURL = @"https://www.moitepari.bg/depoziti/katalog.aspx";
        private const string NonBreakingSpaceUTF8 = "\u00A0";

        public async Task<GetBankProductsResult> GetAllBankProductsAsync()
        {
            var result = new GetBankProductsResult();
            result.SourceURL = BankProductsURL;

            var document = await Context.OpenAsync(BankProductsURL);
            var productBoxes = document.All.Where(n => n.ClassList.Contains("item-box"));

            foreach (var productBox in productBoxes)
            {
                var topHolder = productBox.ChildNodes.First(n => n.NodeType == NodeType.Element);
                var headingHolder = topHolder.ChildNodes.First(n => n.NodeType == NodeType.Element);
                var productHeading = headingHolder.ChildNodes.First(n => n.NodeType == NodeType.Element).TextContent.Trim();
                if (!string.IsNullOrEmpty(productHeading))
                {
                    var buttonWithLink = topHolder.ChildNodes.Where(n => n.NodeType == NodeType.Element).Skip(1).First(); 
                    //var href = buttonWithLink.
                    var href = buttonWithLink.ToHtml();
                    var productType = GetProductType(href);
                    result.BankProducts.Add(new BankProductInfo
                    {
                        Name = productHeading,
                        Type = productType
                    });
                }
            }

            return result;
        }

        public async Task<GetDepositsResult> GetDepositsAsync()
        {
            var deposits = new List<DepositInfo>();
            
            var document = await Context.OpenAsync(DepositsURL);
            var depositOffersWrapper = document.All.Where(e => e.ClassList.Contains("offers-table-wrapper"));
            var depositOffers =
                depositOffersWrapper.First().Children.Where(c => c.ClassList.Contains("DataWithHeader"));

            foreach (var offer in depositOffers)
            {
                var offerData = offer.Children.ToArray();
                
                var bankNameSection = offerData[0];
                var bankNameDiv = bankNameSection.Children.First(c => c.ClassList.Contains("col_bank"));
                var bankName = GetBankNameFromColBank(bankNameDiv);

                var depositNameSection = offerData[1];
                var depositNameDiv = depositNameSection.Children.First(c => c.ClassList.Contains("col_product_name"));
                var depositName = depositNameDiv
                    .Children.First(n => n.NodeType == NodeType.Element)
                    .Children.First(n => n.NodeType == NodeType.Element)
                    .InnerHtml;
                
                var minAmountSection = offerData[2];
                var minAmount = minAmountSection.Children
                    .First(c => c.ClassName == "inner-cnt").TextContent
                    .Replace(NonBreakingSpaceUTF8, "")
                    .Replace(',', '.')
                    .Trim();
                if (string.IsNullOrEmpty(minAmount))
                {
                    minAmount = "0";
                }
                
                var interestDetailsSection = offerData[3];
                var interestDetailsSpan = interestDetailsSection.QuerySelector("span");
                var titleAttributeValue = interestDetailsSpan.Attributes.First(a => a.Name == "title").Value;
                var interestDetails = titleAttributeValue
                    .Replace("<br />", Environment.NewLine)
                    .Replace("<br/>", Environment.NewLine);

                var interests = GetInterests(interestDetails);

                var interestPaymentInfoSection = offerData[4];
                var interestPaymentInfo = interestPaymentInfoSection.Children.First(c => c.ClassName == "inner-cnt").TextContent.Trim();;

                deposits.Add(new DepositInfo
                {
                    Name = depositName,
                    Bank = new BankInfo { Name = bankName},
                    MinAmount = decimal.Parse(minAmount),
                    InterestDetails = interestDetails,
                    InterestOptions = interests,
                    InterestPaymentInfo = interestPaymentInfo,
                    Currency = GetCurrencyFromProductName(depositName)
                });
            }

            return new GetDepositsResult
            {
                SourceURL = DepositsURL,
                Deposits = deposits
            };
        }

        private string GetBankNameFromColBank(IElement bankNameDiv)
        {
            var imgTag = bankNameDiv.Children.First(n => n.NodeType == NodeType.Element).QuerySelector("img");
            if (imgTag != null && imgTag.Attributes.Any(a => a.Name == "alt"))
            {
                return imgTag.Attributes.First(a => a.Name == "alt").Value;
            }
            
            var bankLinkElement = bankNameDiv.Children.First(n => n.NodeType == NodeType.Element);
            return bankLinkElement.InnerHtml.Trim();
        }

        private IList<InterestInfo> GetInterests(string interestDetails)
        {
            var result = new List<InterestInfo>();
            var interests = interestDetails.Replace(NonBreakingSpaceUTF8, "");

            if (interests.Contains("едномесечен"))
            {
                var options = interests.Split("\r");
                var months = 1;
                foreach (var interestOption in options)
                {
                    if (interestOption == "\n")
                    {
                        continue;
                    }
                    var multiplier = GetMonthsMultiplier(interestOption);
                    var percentage = GetPercentage(interestOption);
                    result.Add(new InterestInfo
                    {
                        Months = months * multiplier,
                        Percentage = percentage
                    });
                }
            }
            else if (interests.Contains("тримесечен"))
            {
                var options = interests.Split("\r");
                var months = 3;
                foreach (var interestOption in options)
                {
                    if (interestOption == "\n")
                    {
                        continue;
                    }
                    var multiplier = GetMonthsMultiplier(interestOption);
                    var percentage = GetPercentage(interestOption);
                    result.Add(new InterestInfo
                    {
                        Months = months * multiplier,
                        Percentage = percentage
                    });
                }
            }
            else if (interests.Contains("месец") || interests.Contains("месеца"))
            {
                var options = interests.Split("\r");
                foreach (var interestOption in options)
                {
                    if (interestOption.Contains("%") && 
                        (interestOption.Contains("месец") || interestOption.Contains("месеца")))
                    {
                        var months = GetMonthsFromNormalString(interestOption);
                        var percentage = GetPercentage(interestOption);
                        result.Add(new InterestInfo
                        {
                            Months = months,
                            Percentage = percentage
                        });
                    }
                }
            }

            return result;
        }

        private int GetMonthsFromNormalString(string interestOption)
        {
            var index = interestOption.Contains("месеца")? interestOption.IndexOf("месеца") : interestOption.IndexOf("месец");
            var substringContainingMonthNumber = interestOption.Substring(0, index);
            var indexOfFirstNumber = -1;

            for (int i = 0; i < substringContainingMonthNumber.Length; i++)
            {
                if (char.IsNumber(substringContainingMonthNumber[i]))
                {
                    indexOfFirstNumber = i;
                    break;
                }
            }

            var number = $"{substringContainingMonthNumber[indexOfFirstNumber].ToString()}";
            if (indexOfFirstNumber != -1 && indexOfFirstNumber != (substringContainingMonthNumber.Length - 1))
            {
                var isNextCharADigit = char.IsNumber(substringContainingMonthNumber[indexOfFirstNumber + 1]);
                if (isNextCharADigit)
                {
                    number += $"{substringContainingMonthNumber[indexOfFirstNumber + 1].ToString()}";
                }
            }
            return int.Parse(number);
        }

        private decimal GetPercentage(string interestOption)
        {
            var regexPattern = @"\d\.\d.+%|\d\,\d.+%";
            var result = Regex.Match(interestOption, regexPattern);
            var firstDigitIndex = result.Value.StartsWith('0') ? result.Value.IndexOf('0') : result.Value.IndexOf('1');
            try
            {
                var number = result.Value.Substring(firstDigitIndex, 4);
                if (number.Contains(','))
                {
                    number = number.Replace(',', '.');
                }
                return decimal.Parse(number);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private int GetMonthsMultiplier(string interestOption)
        {
            if (interestOption.Contains("1-ви") || interestOption.Contains("първи"))
            {
                return 1;
            }
            else if (interestOption.Contains("2-ри") || interestOption.Contains("втори"))
            {
                return 2;
            }
            else if (interestOption.Contains("3-ти") || interestOption.Contains("трети"))
            {
                return 3;
            }
            else if (interestOption.Contains("4-ти") || interestOption.Contains("четвърти"))
            {
                return 4;
            }
            else if (interestOption.Contains("5-ти") || interestOption.Contains("пети"))
            {
                return 5;
            }
            else if (interestOption.Contains("6-ти") || interestOption.Contains("шести"))
            {
                return 6;
            }
            else if (interestOption.Contains("7-ми") || interestOption.Contains("седми"))
            {
                return 7;
            }
            else if (interestOption.Contains("8-ми") || interestOption.Contains("осми"))
            {
                return 8;
            }
            else if (interestOption.Contains("9-ти") || interestOption.Contains("девети"))
            {
                return 9;
            }
            else if (interestOption.Contains("10-ти") || interestOption.Contains("десети"))
            {
                return 10;
            }
            else if (interestOption.Contains("11-ти") || interestOption.Contains("единадесети"))
            {
                return 11;
            }
            
            return 12;
        }

        private Currency GetCurrencyFromProductName(string productName)
        {
            if (productName.Contains("BGN"))
            {
                return Currency.BGN;
            }
            if (productName.Contains("EUR"))
            {
                return Currency.EUR;
            }
            if (productName.Contains("USD"))
            {
                return Currency.USD;
            }
            if (productName.Contains("GBP"))
            {
                return Currency.GBP;
            }
            if (productName.Contains("CHF"))
            {
                return Currency.CHF;
            }
            
            return Currency.Unknown;
        }

        private BankProductType GetProductType(string anchorHtml)
        {
            var lowered = anchorHtml.ToLower();
            
            if (lowered.Contains("kredit"))
            {
                return BankProductType.Credit;
            }

            if (lowered.Contains("depozit"))
            {
                return BankProductType.Deposit;
            }

            if (lowered.Contains("fondove"))
            {
                if (lowered.Contains("vzaimni"))
                {
                    return BankProductType.Investment;
                }

                return BankProductType.PensionFund;
            }

            if (lowered.Contains("kalkulator"))
            {
                return BankProductType.Calculator;
            }
            return BankProductType.Unknown;
        }
    }
}