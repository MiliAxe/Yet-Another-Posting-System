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

        private bool AreTextBoxesEmpty(StackPanel stackPanel)
        {
            bool isEmpty = false;
            foreach (var item in stackPanel.Children)
            {
                if (item is TextBox text && string.IsNullOrEmpty(text.Text))
                {
                    isEmpty = true;
                    text.BorderBrush = new SolidColorBrush(Colors.Red);
                }

                if (item is PasswordBox password && string.IsNullOrEmpty(password.Password))
                {
                    isEmpty = true;
                    password.BorderBrush = new SolidColorBrush(Colors.Red);
                }

                if (item is StackPanel stackPanelRecursion)
                {
                    isEmpty = AreTextBoxesEmpty(stackPanelRecursion);
                }
            }

            return isEmpty;
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreTextBoxesEmpty(MainStackPanel) == true)
            {
                throw new Exception("Please fill in all the information");
            }

            if (PasswordBox.Password != RepeatPasswordBox.Password)
            {
                throw new Exception("Please enter the same password");
            }

            users.AddUser(usernameBox.Text, PasswordBox.Password, idBox.Text, phoneBox.Text, nameBox.Text, "Employee");

            MainTextbox.Text = "User created successfully";
            MainTextbox.FontSize = 10;
            MainTextbox.Width = 120;
            SignupButton.IsEnabled = false;
            await Task.Delay(2000);
            Close();
        }
    }
}
