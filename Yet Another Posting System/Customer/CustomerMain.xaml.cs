using System.Windows;
using Yet_Another_Posting_System.Customer;

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
        
        private void SearchOrdersButton_Click(object sender, RoutedEventArgs e)
        {

            SearchOrder searchOrder = new SearchOrder(username);
            searchOrder.ShowDialog();
        }

        private void ChangeCredsButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerCredentialChange customerCredentialChange = new CustomerCredentialChange(username);
            customerCredentialChange.ShowDialog();
        }
    }
}
