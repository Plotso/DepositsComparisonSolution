namespace DepositsComparisonDomainLogicAPI.Services.DataCollecting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AngleSharp;
    using AngleSharp.Dom;
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

                var interestPaymentInfoSection = offerData[4];
                var interestPaymentInfo = interestPaymentInfoSection.Children.First(c => c.ClassName == "inner-cnt").TextContent.Trim();;

                deposits.Add(new DepositInfo
                {
                    Name = depositName,
                    Bank = new BankInfo { Name = bankName},  //ToDo: Add logic to create bank if not existing
                    MinAmount = decimal.Parse(minAmount),
                    InterestDetails = interestDetails,
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