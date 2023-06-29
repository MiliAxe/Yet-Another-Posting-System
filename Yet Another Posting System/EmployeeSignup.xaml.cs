using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class EmployeeSignup : Window
    {
        public EmployeeSignup()
        {
            InitializeComponent();
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if (WPFUtils.AreTextBoxesEmpty(MainStackPanel) == true)
            {
                throw new Exception("Please fill in all the information");
            }

            WPFUtils.CheckEmail(emailBox);
            WPFUtils.CheckEmployeeID(idBox);
            WPFUtils.CheckName(nameBox);
            WPFUtils.CheckPhone(phoneBox);
            WPFUtils.CheckPassword(PasswordBox);

            if (PasswordBox.Password != RepeatPasswordBox.Password)
            {
                throw new Exception("Please enter the same password");
            }

            App.usersDb.AddUser(usernameBox.Text, PasswordBox.Password, emailBox.Text, idBox.Text, phoneBox.Text, nameBox.Text, "Employee");

            MainTextbox.Text = "User created successfully";
            MainTextbox.FontSize = 10;
            MainTextbox.Width = 120;
            SignupButton.IsEnabled = false;
            await Task.Delay(2000);
            Close();
        }
    }
}
