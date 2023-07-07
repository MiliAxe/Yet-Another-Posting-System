using System;
using System.Data;
using System.Data.SQLite;

namespace Yet_Another_Posting_System.Utils
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
            dtConnection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Username TEXT, Password TEXT, Email TEXT, ID TEXT, Phone TEXT, Name TEXT, Type TEXT)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, dtConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            createTableQuery = "CREATE TABLE IF NOT EXISTS Balances (Username TEXT, Balance INT)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, dtConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            createTableQuery = "CREATE TABLE IF NOT EXISTS Orders (CreationDate DATETIME, OrderID INT, CustomerID TEXT, SendAddress TEXT, ReceiveAddress TEXT, ContentIndex INT, TypeIndex INT, Expensive INT, Weight DOUBLE, Phone TEXT, Cost DOUBLE, Status INT, Feedback TEXT)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, dtConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }

            dtConnection.Close();
        }

        public string? AuthenticateUser(string username, string password)
        {
            dtConnection.Open();

            string selectQuery = "SELECT * FROM Users";
            try
            {
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
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {
                    dtConnection.Close();
                }
            }
            return null;
        }

        public bool UserExists(string username)
        {

            dtConnection.Open();

            string selectQuery = "SELECT * FROM Users";

            try
            {
                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dtConnection))
                {
                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Username"].ToString() == username)
                            {
                                dtConnection.Close();
                                return true;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {
                    dtConnection.Close();
                }
            }
            return false;
        }

        public void ChangeUserPassword(string oldUsername, string newUsername, string newPassword)
        {
            dtConnection.Open();

            string updateQuery = $"UPDATE Users SET Username = '{newUsername}', Password = '{newPassword}' WHERE Username = '{oldUsername}';";
            try
            {
                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, dtConnection))
                {
                    updateCommand.ExecuteNonQuery();
                }

            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }


        }

        public bool TypeUserExists(string username, string type)
        {

            dtConnection.Open();

            string selectQuery = "SELECT * FROM Users";

            try
            {
                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dtConnection))
                {
                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Username"].ToString() == username && reader["Type"].ToString() == type)
                            {
                                dtConnection.Close();
                                return true;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }
            return false;
        }

        public void AddUser(string username, string password, string email, string? id, string phone, string name, string type)
        {
            if (TypeUserExists(username, type) == true)
            {
                throw new Exception("This user already exists");
            }

            dtConnection.Open();

            string insertQuery = "INSERT INTO Users (Username, Password, Email, ID, Phone, Name, Type) VALUES (@Username, @Password, @Email, @ID, @Phone, @Name, @Type);";
            try
            {
                using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, dtConnection))
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
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }
        }

        public void CreateUserBalance(string username)
        {
            if (TypeUserExists(username, "Customer") == false)
            {
                throw new Exception("Such customer doesn't exist");
            }

            dtConnection.Open();

            string insertQuery = "INSERT INTO Balances (Username, Balance) VALUES (@Username, @Balance);";
            try
            {
                using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, dtConnection))
                {
                    insertCommand.Parameters.AddWithValue("@Username", username);
                    insertCommand.Parameters.AddWithValue("@Balance", 0);
                    insertCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }

        }

        public void ChangeUserBalance(string username, double balance)
        {
            if (TypeUserExists(username, "Customer") == false)
            {
                throw new Exception("Such customer doesn't exist");
            }

            dtConnection.Open();
            string updateQuery = $"UPDATE Balances SET Balance = {balance} WHERE Username = '{username}';";
            try
            {
                using (SQLiteCommand insertCommand = new SQLiteCommand(updateQuery, dtConnection))
                {
                    insertCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }

        }

        public double UserBalance(string username)
        {
            double result;

            string selectQuery = $"SELECT Balance FROM Balances WHERE Username = '{username}';";
            dtConnection.Open();
            try
            {
                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dtConnection))
                {
                    object selectResult = selectCommand.ExecuteScalar();
                    result = Convert.ToDouble(selectResult);
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }

            return result;
        }

        public string GetUserEmail(string userID)
        {
            string? result;

            string selectQuery = $"SELECT Email FROM Users WHERE Username = '{userID}';";
            dtConnection.Open();
            try
            {
                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, dtConnection))
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        result = reader["Email"].ToString();
                        if (result == null)
                        {
                            throw new Exception("There was a problem finding the email of the user");
                        }
                    }
                    else
                    {
                        throw new Exception("No such user exists");
                    }
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }

            return result;
        }
        
/*
        public string GetCustomerIdFromUsername(string username)
        {
            string? result = "";

            string selectQuery = $"SELECT ID FROM Users WHERE Username = '{username}';";
            this.dtConnection.Open();
            try
            {
                using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, this.dtConnection))
                {
                    SQLiteDataReader reader = selectCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        result = reader.GetValue(0).ToString();
                        if (result == null)
                        {
                            throw new Exception("There was a problem in SQL Logic");
                        }
                    }
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }

            return result;
        }*/

        public Tuple<string, string, string> GenerateUser(string email, string id, string phone, string name, string type)
        {
            var random = new Random();

            string randomUsername = "user" + random.Next(0, 9999).ToString();

            while (TypeUserExists(randomUsername, type) == true)
            {
                randomUsername = "user" + random.Next(0, 9999).ToString();
            }

            string randomPassword = random.Next(10000000, 99999999).ToString();

            AddUser(randomUsername, randomPassword, email, id, phone, name, type);
            CreateUserBalance(randomUsername);

            return Tuple.Create(randomUsername, randomPassword, email);
        }

        public int NextOrderID()
        {
            int result;
            dtConnection.Open();

            string countQuery = "SELECT COUNT(*) FROM Orders";
            try
            {
                using (SQLiteCommand countCommand = new SQLiteCommand(countQuery, dtConnection))
                {
                    object countResult = countCommand.ExecuteScalar();
                    result = Convert.ToInt16(countResult);
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }

            return result + 1;
        }

        public void CreateOrder(string customerID, string sendAddress, string receiveAddress, int contentIndex, int typeIndex, int isExpensive, double weight, string phone, double cost)
        {
            int nextOrderID = NextOrderID();

            string insertQuery = "INSERT INTO Orders (CreationDate, OrderID, CustomerID, SendAddress, ReceiveAddress, ContentIndex, TypeIndex, Expensive, Weight, Phone, Cost, Status) " +
                "VALUES (@CreationDate, @OrderID, @CustomerID, @SendAddress, @ReceiveAddress, @ContentIndex, @TypeIndex, @IsExpensive, @Weight, @Phone, @Cost, @Status)";

            dtConnection.Open();

            try
            {
                using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, dtConnection))
                {
                    insertCommand.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@OrderID", nextOrderID);
                    insertCommand.Parameters.AddWithValue("@CustomerID", customerID);
                    insertCommand.Parameters.AddWithValue("@SendAddress", sendAddress);
                    insertCommand.Parameters.AddWithValue("@ReceiveAddress", receiveAddress);
                    insertCommand.Parameters.AddWithValue("@ContentIndex", contentIndex);
                    insertCommand.Parameters.AddWithValue("@TypeIndex", typeIndex);
                    insertCommand.Parameters.AddWithValue("@IsExpensive", isExpensive);
                    insertCommand.Parameters.AddWithValue("@Weight", weight);
                    insertCommand.Parameters.AddWithValue("@Phone", phone);
                    insertCommand.Parameters.AddWithValue("@Cost", cost);
                    insertCommand.Parameters.AddWithValue("@Status", 0);
                    insertCommand.Parameters.AddWithValue("@Feedback", "");

                    insertCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }
        }

        public void UpdateOrderStatus(int statusIndex, string orderID)
        {

            string query = $"UPDATE Orders SET Status = {statusIndex} WHERE OrderID = {orderID}";

            dtConnection.Open();
            try
            {
                using (SQLiteCommand updateCommand = new SQLiteCommand(query, App.usersDb.dtConnection))
                {
                    updateCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }
        }
        public void SetFeedback(string feedback, string orderID)
        {

            string query = $"UPDATE Orders SET Feedback = '{feedback}' WHERE OrderID = {orderID}";

            dtConnection.Open();
            try
            {
                using (SQLiteCommand updateCommand = new SQLiteCommand(query, App.usersDb.dtConnection))
                {
                    updateCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                if (dtConnection.State == ConnectionState.Open)
                {

                    dtConnection.Close();
                }
            }
        }
    }

}
