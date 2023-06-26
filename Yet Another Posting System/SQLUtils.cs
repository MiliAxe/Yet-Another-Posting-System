using System;
using System.Data.SQLite;

namespace Yet_Another_Posting_System
{
    public class SQLUtils
    {
        public SQLiteConnection dtConnection;

        public SQLUtils(string dtName)
        {
            string connectionString = $"Data Source=./{dtName};Version=3;";
            dtConnection = new SQLiteConnection(connectionString);
        }
    }

    public class UsersDatabase : SQLUtils
    {
        public UsersDatabase(string dbName) : base(dbName)
        {
            this.dtConnection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Username TEXT, Password TEXT, Email TEXT, ID TEXT, Phone TEXT, Name TEXT, Type TEXT)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, this.dtConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }

            this.dtConnection.Close();
        }

        public string? AuthenticateUser(string username, string password)
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
                            string? result = reader["Type"].ToString();
                            dtConnection.Close();
                            return result;
                        }
                    }
                }
            }
            dtConnection.Close();
            return null;
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
                            this.dtConnection.Close();
                            return true;
                        }
                    }
                }
            }
            this.dtConnection.Close();
            return false;
        }

        public bool TypeUserExists(string username, string type)
        {

            this.dtConnection.Open();

            string selectQuery = "SELECT * FROM Users";
            using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dtConnection))
            {
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Username"].ToString() == username && reader["Type"].ToString() == type)
                        {
                            this.dtConnection.Close();
                            return true;
                        }
                    }
                }
            }
            this.dtConnection.Close();
            return false;
        }

        public void AddUser(string username, string password, string email, string? id, string phone, string name, string type)
        {
            if (TypeUserExists(username, type) == true)
            {
                throw new System.Exception("This user already exists");
            }

            this.dtConnection.Open();

            string insertQuery = "INSERT INTO Users (Username, Password, Email, ID, Phone, Name, Type) VALUES (@Username, @Password, @Email, @ID, @Phone, @Name, @Type);";
            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, this.dtConnection))
            {
                insertCommand.Parameters.AddWithValue("@Username", username);
                insertCommand.Parameters.AddWithValue("@Password", password);
                insertCommand.Parameters.AddWithValue("@ID", id);
                insertCommand.Parameters.AddWithValue("@Phone", phone);
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Type", type);
                insertCommand.Parameters.AddWithValue("@Email", email);
                insertCommand.ExecuteNonQuery();
            }

            dtConnection.Close();
        }

        public string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string result = "";

            for (int i = 0; i < length; i++)
            {
                result += chars[random.Next(chars.Length)];
            }

            return result;
        }

        public Tuple<string, string, string> GenerateUser(string email, string id, string phone, string name, string type)
        {
            string randomUsername = GenerateRandomString(10);

            while (TypeUserExists(randomUsername, type) == true)
            {
                randomUsername = GenerateRandomString(10);
            }

            string randomPassword = GenerateRandomString(10);

            AddUser(randomUsername, randomPassword, email, id, phone, name, type);

            return Tuple.Create(randomUsername, randomPassword, email);
        }
    }
}
