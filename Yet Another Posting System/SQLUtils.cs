using System.Data.SQLite;

namespace Yet_Another_Posting_System
{
    internal class SQLUtils
    {
        public SQLiteConnection dtConnection;

        public SQLUtils(string dtName)
        {
            string connectionString = $"Data Source=./{dtName};Version=3;";
            dtConnection = new SQLiteConnection(connectionString);
        }
    }

    internal class UsersDatabase : SQLUtils
    {
        public UsersDatabase(string dbName) : base(dbName)
        {
            this.dtConnection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Username TEXT, Password TEXT)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, this.dtConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }

            this.dtConnection.Close();
        }

        public bool AuthenticateUser(string username, string password)
        {
            this.dtConnection.Open();

            string selectQuery = "SELECT * FROM Users";
            using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dtConnection))
            {
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Username"].ToString() == username && reader["Password"].ToString() == password)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool UserExists(string username)
        {

            this.dtConnection.Open();

            string selectQuery = "SELECT * FROM Users";
            using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dtConnection))
            {
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Username"].ToString() == username)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void AddUser(string username, string password)
        {
            if (UserExists(username) == true)
            {
                throw new System.Exception("This user already exists");
            }
        }
    }
}
