namespace DepositsCreator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using DepositsComparison.Data.Public;
    using DepositsComparisonDomainLogic.Contracts;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using RestSharp;
    using DataFormat = RestSharp.DataFormat;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var bank = new Banks();
            var curr = new Currencies();
            var term = new Terms();
            var interest = new Interests();
            var interestType = new InterestTypes();
            
            InitializeComponent();
        }

        private void SeedButton_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess = true;
            bool isMaxOk = false;
            bool isMinOk = false;

            bool IsDigitsOnly(string str)
            {
                int minus_count = 0;
                foreach (char c in str)
                {
                    if (c == '-') minus_count++;
                    if ((c < '0' || c > '9') && c != '-')
                        return false;
                }

                if (minus_count > 1) return false;
                return true;
            }

            if (string.IsNullOrWhiteSpace(txtMinValue.Text))
            {
                txtMinValue.Focus();
                txtMinValue.Text = "Няма въведена стойност!";
                isSuccess = false;
            }
            else if (IsDigitsOnly(txtMinValue.Text).Equals(false))
            {
                txtMinValue.Focus();
                txtMinValue.Text = "Стойността трябва да е число!";
                isSuccess = false;
            }
            else if (int.Parse(txtMinValue.Text) < 1)
            {
                txtMinValue.Focus();
                txtMinValue.Text = "Стойността трябва да е положителна!";
                isSuccess = false;
            }
            else
            {
                isMinOk = true;
            }

            if (string.IsNullOrWhiteSpace(txtMaxValue.Text))
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Няма въведена стойност!";
                isSuccess = false;
            }
            else if (IsDigitsOnly(txtMaxValue.Text).Equals(false))
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Стойността трябва да е число!";
                isSuccess = false;
            }
            
            else if (int.Parse(txtMaxValue.Text) < 1)
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Стойността трябва да е положителна!";
                isSuccess = false;
            }

            else if (int.Parse(txtMaxValue.Text) > 100000)
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Стойността не може да надвишава 100 000!";
                isSuccess = false;
            }
            else
            {
                isMaxOk = true;
            }

            if (isMaxOk && isMinOk)
            {
                if (int.Parse(txtMaxValue.Text) < int.Parse(txtMinValue.Text))
                {
                    MessageBox.Show("Максималната въведена стойност е по-ниска от минималната!");
                    isSuccess = false;
                }
            }

            if (isSuccess)
            {
                var client = new RestClient(RouteConstants.ApiUrl);

                var requestModel = new CreateDepositRequest
                {
                    Deposit = new DepositInfo
                    {
                        Bank = new BankInfo
                        {
                            Name = BankComboBox.Text
                        },
                        Currency = (Currency) Enum.Parse(typeof(Currency), CurencyComboBox.Text),
                        InterestDetails = string.Empty,
                        InterestOptions = new List<InterestInfo>
                        {
                            new InterestInfo
                            {
                                Months = int.Parse(TermsComboBox.Text),
                                Percentage = decimal.Parse(percentageComboBox.Text),
                                Type = (InterestType) Enum.Parse(typeof(InterestType), InterestTypeComboBox.Text),
                            }
                        },
                        InterestPaymentInfo = String.Empty,
                        MaxAmount = decimal.Parse(txtMaxValue.Text),
                        MinAmount = decimal.Parse(txtMinValue.Text),
                        Name = DeposiTxtName.Text
                    }
                };

                var request = new RestRequest(RouteConstants.CreateDepositEndpoint, Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(requestModel);

                var response = client.Execute<CreateDepositResponse>(request);

                if (!response.Data.IsSuccess)
                {
                    MessageBox.Show($"[ERROR][API] {response.Data.ErrorMessage}");
                    Console.WriteLine($"[ERROR][API] {response.Data.ErrorMessage}");
                }
                else
                {
                    MessageBox.Show($"Successfully created deposit \"{DeposiTxtName.Text}\"");
                
                    DeposiTxtName.Text = string.Empty;
                    txtMinValue.Text = string.Empty;
                    txtMaxValue.Text = string.Empty;
                    BankComboBox.Text = string.Empty;
                }
            }
        }
    }

    class Banks : ObservableCollection<string>
    {
        public Banks()
        {
            var client = new RestClient(RouteConstants.ApiUrl);

            var request = new RestRequest(RouteConstants.GetAllBanksEndpoint, Method.GET);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute<GetAllBanksResponse>(request);
            if (response.Data.Banks.Any())
            {
                foreach (var bank in response.Data.Banks.GroupBy(b => b.Name))
                {
                    Add(bank.Key);
                }
            }
            else
            {
                Add("Инвестбанк");
                Add("Обединена Българска Банка");
                Add("Д Банк");
                Add("УниКредит Булбанк");
                Add("Алианц Банк България");
                Add("ТИ БИ АЙ Банк");
                Add("Централна Кооперативна Банка");
            }
        }
    }

    class Currencies : ObservableCollection<string>
    {
        public Currencies()
        {
            var currencies = Enum.GetNames(typeof(Currency));
            foreach (var currency in currencies)
            {
                Add(currency);
            }
        }
    }

    class Terms : ObservableCollection<string>
    {
        public Terms()
        {
            Add("1");
            Add("2");
            Add("3");
            Add("6");
            Add("9");
            Add("12");
            Add("18");
            Add("24");
            Add("30");
            Add("36");
        }
    }

    class Interests : ObservableCollection<string>
    {
        public Interests()
        {
            Add("1");
            Add("2");
            Add("3");
            Add("6");
            Add("9");
            Add("12");
            Add("18");
            Add("24");
            Add("30");
            Add("36");
        }
    }

    class InterestTypes : ObservableCollection<string>
    {
        public InterestTypes()
        {
            var interestTypes = Enum.GetNames(typeof(InterestType));
            foreach (var type in interestTypes)
            {
                Add(type);
            }
        }
    }
}