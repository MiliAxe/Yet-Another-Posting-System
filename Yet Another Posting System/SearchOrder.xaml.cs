using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for SearchOrder.xaml
    /// </summary>
    public partial class SearchOrder : Window
    {
        public SearchOrder()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM Orders";
            SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter(query, App.usersDb.dtConnection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            dtGrid.ItemsSource = dataSet.Tables[0].DefaultView;
        }
    }
}
