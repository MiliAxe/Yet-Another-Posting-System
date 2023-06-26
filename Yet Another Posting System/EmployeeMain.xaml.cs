using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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
            customerSignup.Show();
        }
    }
}
