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
using Yet_Another_Posting_System.Utils;

namespace Yet_Another_Posting_System.Customer
{
    /// <summary>
    /// Interaction logic for CustomerCredentialChange.xaml
    /// </summary>
    public partial class CustomerCredentialChange : Window
    {
        string username { get; set; }
        public CustomerCredentialChange(string username)
        {
            this.username = username;
            InitializeComponent();
            currentUsernameLabel.Content = username;
        }

        private void chargeButton_Click(object sender, RoutedEventArgs e)
        {
            WPFUtils.CheckPassword(newPasswordBox);

            if (App.usersDb.UserExists(newUsernameBox.Text) == true)
            {
                throw new Exception("Such user already exists");
            }

            App.usersDb.ChangeUserPassword(username, newUsernameBox.Text, newPasswordBox.Password);
            MessageBox.Show("Credentials changed successfully");
        }
    }
}
