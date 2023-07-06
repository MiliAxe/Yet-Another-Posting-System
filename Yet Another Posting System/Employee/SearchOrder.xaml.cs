using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.IO;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for SearchOrder.xaml
    /// </summary>
    public partial class SearchOrder : Window
    {
        public SearchOrder(bool customerIdIsReadOnly=false, string customerIdText="")
        {
            InitializeComponent();
            if (customerIdIsReadOnly)
            {
                customerIDBox.Text = customerIdText;
                customerIDBox.IsReadOnly = true;
            }
        }

        public static void ExportQueryResultsToCsv(string query, string outputPath)
        {
            App.usersDb.dtConnection.Open();

            using (SQLiteCommand command = new SQLiteCommand(query, App.usersDb.dtConnection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Create the output CSV file
                        using (StreamWriter writer = new StreamWriter(outputPath))
                        {
                            // Write column names as the first line
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                writer.Write($"\"{reader.GetName(i)}\"");

                                if (i < reader.FieldCount - 1)
                                    writer.Write(",");
                            }
                            writer.WriteLine();

                            // Write data rows
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    writer.Write($"\"{reader[i]}\"");

                                    if (i < reader.FieldCount - 1)
                                        writer.Write(",");
                                }
                                writer.WriteLine();
                            }
                        }

                    }
                }
            }
            App.usersDb.dtConnection.Close();
        }


        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM Orders WHERE 1=1";

            if (!string.IsNullOrEmpty(customerIDBox.Text))
            {
                query += $" AND CustomerID = '{customerIDBox.Text}'";
            }
            if (!string.IsNullOrEmpty(weightBox.Text))
            {
                query += $" AND Weight = {weightBox.Text}";
            }
            if (!string.IsNullOrEmpty(costBox.Text))
            {
                query += $" AND Cost = {costBox.Text}";
            }
            if (postTypeBox.SelectedItem != null)
            {
                query += $" AND TypeIndex = {postTypeBox.SelectedIndex}";
            }
            if (contentBox.SelectedItem != null)
            {
                query += $" AND ContentIndex = {contentBox.SelectedIndex}";
            }

            query += $" ORDER BY CreationDate DESC";


            SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(query, App.usersDb.dtConnection);

            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            dtGrid.ItemsSource = dataSet.Tables[0].DefaultView;

            ExportQueryResultsToCsv(query, "SearchResult.csv");
        }
    }
 }
