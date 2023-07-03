using System.Windows;
using Yet_Another_Posting_System.Utils;

namespace Yet_Another_Posting_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UsersDatabase usersDb = new UsersDatabase("Users.db");

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An error occured: " + e.Exception.Message, "Error");
            e.Handled = true;
        }
    }
}
