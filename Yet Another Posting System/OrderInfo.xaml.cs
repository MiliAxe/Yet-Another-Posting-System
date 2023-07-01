using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for OrderInfo.xaml
    /// </summary>
    public partial class OrderInfo : Window
    {
        public OrderInfo()
        {
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
                        receiveLabel.Content = reader["ReceiveAddress"].ToString();
                        sendLabel.Content = reader["SendAddress"].ToString();
                        customerIDLabel.Content = reader["CustomerID"].ToString();
                        contentLabel.Content = contentList[int.Parse(reader["ContentIndex"]?.ToString() ?? "0")];
                        typeLabel.Content = typeList[int.Parse(reader["TypeIndex"]?.ToString() ?? "0")];
                        phoneLabel.Content = reader["Phone"].ToString();
                        costLabel.Content = reader["Cost"].ToString();
                        weightLabel.Content = reader["Weight"].ToString();
                        expensiveLabel.Content = reader["Expensive"].ToString();
                        statusComboBox.IsEnabled = true;
                        statusIndex = int.Parse(reader["Status"]?.ToString() ?? "0");

                    }
                }
            }
            App.usersDb.dtConnection.Close();

            statusComboBox.SelectedIndex = statusIndex;


        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            FillInfo(int.Parse(orderIDBox.Text));
        }

        private void statusComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            App.usersDb.UpdateOrderStatus(statusComboBox.SelectedIndex, orderIDBox.Text);

            if (statusComboBox.SelectedIndex == 3)
            {
                statusComboBox.IsEnabled = false;
            }
        }
    }

}
