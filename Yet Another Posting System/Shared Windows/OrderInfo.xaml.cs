using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using Yet_Another_Posting_System.Utils;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for OrderInfo.xaml
    /// </summary>
    public partial class OrderInfo : Window
    {
        string username;
        public OrderInfo(string username = "")
        {
            if (username.Length > 0)
            {
                this.username = username;
            } 
            InitializeComponent();
        }

        public void FillInfo(int orderID)
        {
            WPFUtils.ClearAll(mainGrid);
            statusComboBox.IsEnabled = false;
            List<string> contentList = new List<string>() { "Object", "Documents", "Breakable" };
            List<string> typeList = new List<string>() { "Normal", "Prime" };
            string query = $"SELECT * FROM Orders WHERE OrderID = {orderID}";
            int statusIndex = 1;

            App.usersDb.dtConnection.Open();

            using (SQLiteCommand command = new SQLiteCommand(query, App.usersDb.dtConnection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        statusIndex = int.Parse(reader["Status"]?.ToString() ?? "0");
                        if (this.username != null)
                        {

                            if (reader["CustomerID"].ToString() != username)
                            {
                                App.usersDb.dtConnection.Close();
                                throw new System.Exception("This order is not yours. You can't see its info.");
                            }
                            if (statusIndex == 3)
                            {
                                feedbackTextBox.IsReadOnly = false;
                            }
                        } else
                        {
                            customerIDLabel.Content = reader["CustomerID"].ToString();
                            statusComboBox.IsEnabled = true;
                        }
                        customerIDLabel.Content = reader["CustomerID"].ToString();
                        receiveLabel.Content = reader["ReceiveAddress"].ToString();
                        sendLabel.Content = reader["SendAddress"].ToString();
                        contentLabel.Content = contentList[int.Parse(reader["ContentIndex"]?.ToString() ?? "0")];
                        typeLabel.Content = typeList[int.Parse(reader["TypeIndex"]?.ToString() ?? "0")];
                        phoneLabel.Content = reader["Phone"].ToString();
                        costLabel.Content = reader["Cost"].ToString();
                        weightLabel.Content = reader["Weight"].ToString();
                        expensiveLabel.Content = reader["Expensive"].ToString();
                        feedbackTextBox.Text = reader["Feedback"].ToString();

                    }
                }
            }
            App.usersDb.dtConnection.Close();

            statusComboBox.SelectionChanged -= statusComboBox_SelectionChanged;
            statusComboBox.SelectedIndex = statusIndex;
            statusComboBox.SelectionChanged += statusComboBox_SelectionChanged;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            FillInfo(int.Parse(orderIDBox.Text));
        }

        private void statusComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string? customerID = customerIDLabel.Content.ToString();
            if (customerID == null)
            {
                throw new System.Exception("The customer ID is empty");
            }


            if (string.IsNullOrEmpty(customerIDLabel.Content.ToString()) == false)
            {

                App.usersDb.UpdateOrderStatus(statusComboBox.SelectedIndex, orderIDBox.Text);

                if (statusComboBox.SelectedIndex == 3)
                {
                    statusComboBox.IsEnabled = false;
                    MailUtils mail = new MailUtils("Credentials.txt");
                    mail.SendMail(App.usersDb.GetUserEmail(customerID), "Post delivery", $"Your order with the id of {orderIDBox.Text} has been delivered");
                }
            }
        }

        private void feedbackTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            feedbackButton.IsEnabled = feedbackTextBox.Text.Length > 0;
        }

        private void feedbackButton_Click(object sender, RoutedEventArgs e)
        {

            App.usersDb.SetFeedback(feedbackTextBox.Text, orderIDBox.Text);
        }
    }

}
