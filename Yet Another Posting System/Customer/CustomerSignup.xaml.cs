using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Yet_Another_Posting_System.Utils;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for CustomerSignup.xaml
    /// </summary>
    public partial class CustomerSignup : Window
    {
        public CustomerSignup()
        {
            InitializeComponent();
        }

        private async void signupButton_Click(object sender, RoutedEventArgs e)
        {
            if (WPFUtils.AreTextBoxesEmpty(MainStackPanel) == true)
            {
                throw new Exception("Please fill in all the information");
            }

            WPFUtils.CheckName(nameBox);
            WPFUtils.CheckCustomerID(idBox);
            WPFUtils.CheckEmail(emailBox);
            WPFUtils.CheckPhone(phoneBox);

            Tuple<string, string, string> userPass = App.usersDb.GenerateUser(emailBox.Text, idBox.Text, phoneBox.Text, nameBox.Text, "Customer");

            MailUtils mail = new MailUtils("Credentials.txt");

            mainText.Text = "Sending Email";
            mail.SendMail(userPass.Item3, "Posting system user pass", $"User: {userPass.Item1}\nPass: {userPass.Item2}");
            mainText.Text = "Email sent";

            mainText.Text = "User created successfully";
            mainText.FontSize = 10;
            mainText.Width = 120;
            signupButton.IsEnabled = false;
            await Task.Delay(2000);
            Close();
        }
    }
}
