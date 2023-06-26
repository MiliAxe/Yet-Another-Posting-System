using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string? userLogin = App.usersDb.AuthenticateUser(usernameBox.Text, PasswordBox.Password);
            if (userLogin != null)
            {
                        PasswordBox.BorderBrush = new SolidColorBrush(Colors.Green);
                switch (userLogin)
                {
                    case "Employee":
                        EmployeeMain employeeMain = new EmployeeMain(usernameBox.Text);
                        employeeMain.ShowDialog();
                        break;

                    case "Customer":
                        CustomerMain customerMain = new CustomerMain(usernameBox.Text);
                        customerMain.ShowDialog();
                        break;

                    default:
                        break;
                }
            }
            else
            {
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }


        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeSignup signupWindow = new EmployeeSignup();
            signupWindow.ShowDialog();
        }
    }
}
