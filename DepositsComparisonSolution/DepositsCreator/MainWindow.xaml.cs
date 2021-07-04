using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DepositsComparison.Data.Public;
using DepositsComparisonDomainLogic.Contracts;
using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
using RestSharp;
using DataFormat = RestSharp.DataFormat;

namespace DepositsCreator
{
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
            InitializeComponent();
        }

        private void SeedButton_Click(object sender, RoutedEventArgs e)
        {
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
            }
            else if (IsDigitsOnly(txtMinValue.Text).Equals(false))
            {
                txtMinValue.Focus();
                txtMinValue.Text = "Стойността трябва да е число!";
            }
            else if (int.Parse(txtMinValue.Text) < 1)
            {
                txtMinValue.Focus();
                txtMinValue.Text = "Стойността трябва да е положителна!";
            }
            else
            {
                isMinOk = true;
            }

            if (string.IsNullOrWhiteSpace(txtMaxValue.Text))
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Няма въведена стойност!";
            }
            else if (IsDigitsOnly(txtMaxValue.Text).Equals(false))
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Стойността трябва да е число!";
            }

            //TODO add check if the number is negative

            else if (int.Parse(txtMaxValue.Text) > 100000)
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Стойността не може да надвишава 100 000!";
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
                }
            }

            var client = new RestClient("https://localhost:5001");

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
                            Type = InterestType.Fixed
                        }
                    },
                    InterestPaymentInfo = String.Empty,
                    MaxAmount = decimal.Parse(txtMaxValue.Text),
                    MinAmount = decimal.Parse(txtMinValue.Text),
                    Name = DeposiTxtName.Text
                }
            };

            var request = new RestRequest("/Administration/CreateDeposit", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(requestModel);

            var response = client.Execute<CreateDepositResponse>(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"[ERROR] {response.Data.ErrorMessage}");
            }
        }
    }

    class Banks : ObservableCollection<string>
    {
        public Banks()
        {
            var client = new RestClient("https://localhost:5001");

            var request = new RestRequest("/Banks/GetAllBanks", Method.GET);
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
}