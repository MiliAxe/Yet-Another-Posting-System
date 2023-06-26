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

            Tuple<string, string, string> userPass = App.usersDb.GenerateUser(emailBox.Text, idBox.Text, phoneBox.Text, nameBox.Text, "Customer");


            MailUtils mail = new MailUtils("miliaxe0@gmail.com", "Milad Zarei", "nxmuydnsxmdblzpz");

            mail.SendMail(userPass.Item3, "Posting system user pass", $"User: {userPass.Item1}\nPass: {userPass.Item2}");
        }
    }
}
