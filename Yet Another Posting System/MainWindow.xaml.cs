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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UsersDatabase users;

        public MainWindow()
        {
            users = new UsersDatabase("Users.db");
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (users.AuthenticateUser(usernameBox.Text, PasswordBox.Password) != null)
            {
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Green);
                MessageBox.Show(users.AuthenticateUser(usernameBox.Text, PasswordBox.Password));
            }
            else
            {
                PasswordBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }
        

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            SignupMain signupWindow = new SignupMain(users);
            signupWindow.Show();
        }
    }
}
