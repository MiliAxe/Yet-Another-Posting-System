using System.Windows;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for WorkerMain.xaml
    /// </summary>
    public partial class EmployeeMain : Window
    {
        public string username;

        public EmployeeMain(string username)
        {
            this.username = username;
            InitializeComponent();
        }

        private void CreateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerSignup customerSignup = new CustomerSignup();
            customerSignup.ShowDialog();
        }

        private void PlaceOrderButton_Click(object sender, RoutedEventArgs e)
        {
            PlaceOrder placeOrder = new PlaceOrder();
            placeOrder.ShowDialog();
        }

        private void SearchOrderButton_Click(object sender, RoutedEventArgs e)
        {
            SearchOrder searchOrder = new SearchOrder();
            searchOrder.ShowDialog();
        }

        private void OrderInfoButton_Click(object sender, RoutedEventArgs e)
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo.ShowDialog();
        }
    }
}
