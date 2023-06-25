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
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class SignupMain : Window
    {
        UsersDatabase users;

        public SignupMain(UsersDatabase users)
        {
            this.users = users;
            InitializeComponent();
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            users.AddUser(usernameBox.Text, PasswordBox.Password, "Employee");
            MainTextbox.Text = "User created successfully";
            MainTextbox.FontSize = 10;
            MainTextbox.Width = 120;
            SignupButton.IsEnabled = false;
            await Task.Delay(2000);
            Close();
        }
    }
}
