using System.Collections.ObjectModel;
using System.Windows;
using RestSharp;

namespace Desktop_framework_stefan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var bank = new Banks();
            var curr = new Currencies();
            var term = new Terms();
            var interest = new Interests();
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
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
                txtMinValue.Text = "ne moje prazen string be deeba";
            }
            else if (IsDigitsOnly(txtMinValue.Text).Equals(false))
            {
                txtMinValue.Focus();
                txtMinValue.Text = "Ama kvi sa tiq bukvi tuka be";
            }
            else if (int.Parse(txtMinValue.Text) < 1)
            {
                txtMinValue.Focus();
                txtMinValue.Text = "kak otricatelno be ludak";
            }
            else
            {
                isMinOk = true;
            }
            
            if (string.IsNullOrWhiteSpace(txtMaxValue.Text))
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "ne moje prazen string be deeba";
            }
            else if (IsDigitsOnly(txtMaxValue.Text).Equals(false))
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "Ama kvi sa tiq bukvi tuka be";
            }
            else if (int.Parse(txtMaxValue.Text) > 100000)
            {
                txtMaxValue.Focus();
                txtMaxValue.Text = "kva e taq stoinost ei";
            }
            else
            {
                isMaxOk = true;
            }

            if (isMaxOk && isMinOk)
            {
                if (int.Parse(txtMaxValue.Text) < int.Parse(txtMinValue.Text))
                {
                    MessageBox.Show("Ama ti lud li si s tiq granici?");
                }
            }

        //    var client = new RestClient("https://localhost:5001/Administration/CreateDeposit");

        //    var request = new CreateDepositRequest();
        }
    }

    class Banks : ObservableCollection<string>
    {
        public Banks()
        {
            Add("DSK");
            Add("REIFFEISEN");
            Add("OBB");
            Add("UNICREDIT");
            Add("NBU");
        }
    }

    class Currencies : ObservableCollection<string>
    {
        public Currencies()
        {
            Add("BGN");
            Add("USD");
            Add("EUR");
            Add("HUI");
            Add("KUR");
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
            Add("1%");
            Add("2%");
            Add("3%");
            Add("6%");
            Add("9%");
            Add("12%");
            Add("18%");
            Add("24%");
            Add("30%");
            Add("36%");
        }
    }
}