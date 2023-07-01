using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

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

        private void signupButton_Click(object sender, RoutedEventArgs e)
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
        }
    }
}
