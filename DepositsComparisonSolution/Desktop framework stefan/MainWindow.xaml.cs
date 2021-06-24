using System.Collections.ObjectModel;

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
        
    }
    
    class Banks : ObservableCollection<string>
    {
        public Banks ()
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
        public Currencies ()
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
        public Terms ()
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
        public Interests ()
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