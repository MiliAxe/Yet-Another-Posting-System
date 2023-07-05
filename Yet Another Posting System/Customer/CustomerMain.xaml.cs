using System.Windows;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for CustomerMain.xaml
    /// </summary>
    public partial class CustomerMain : Window
    {
        string username;

        public CustomerMain(string username)
        {
            this.username = username;
            InitializeComponent();
        }

        private void BalanceButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerBalance customerBalance = new CustomerBalance(username);
            customerBalance.ShowDialog();
        }
    }
}
