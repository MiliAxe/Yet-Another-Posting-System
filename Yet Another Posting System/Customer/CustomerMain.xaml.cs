using System.Windows;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for CustomerMain.xaml
    /// </summary>
    public partial class CustomerMain : Window
    {
        string username;

        public CustomerMain(string username)
        {
            this.username = username;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
