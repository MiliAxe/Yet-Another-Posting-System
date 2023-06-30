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
            createTableQuery = "CREATE TABLE IF NOT EXISTS Balances (Username TEXT, Balance INT)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, this.dtConnection))
            {
                createTableCommand.ExecuteNonQuery();
            }
            createTableQuery = "CREATE TABLE IF NOT EXISTS Orders (CreationDate DATETIME, OrderID INT, CustomerID TEXT, SendAddress TEXT, ReceiveAddress TEXT, ContentIndex INT, TypeIndex INT, Expensive INT, Weight DOUBLE, Phone TEXT, Cost DOUBLE)";
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

        public void CreateUserBalance(string username)
        {
            if (TypeUserExists(username, "Customer") == false)
            {
                throw new Exception("Such customer doesn't exist");
            }

            this.dtConnection.Open();

            string insertQuery = "INSERT INTO Balances (Username, Balance) VALUES (@Username, @Balance);";
            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, this.dtConnection))
            {
                insertCommand.Parameters.AddWithValue("@Username", username);
                insertCommand.Parameters.AddWithValue("@Balance", 0);
                insertCommand.ExecuteNonQuery();
            }

            dtConnection.Close();
        }

        public void ChangeUserBalance(string username, double balance)
        {
            if (TypeUserExists(username, "Customer") == false)
            {
                throw new Exception("Such customer doesn't exist");
            }

            string updateQuery = $"UPDATE Balances SET Balance = {balance} WHERE Username = {username};";
            using (SQLiteCommand insertCommand = new SQLiteCommand(updateQuery, this.dtConnection))
            {
                insertCommand.ExecuteNonQuery();
            }

            this.dtConnection.Open();
        }

        public double UserBalance(string username)
        {
            double result;

            string selectQuery = $"SELECT Balance FROM Balances WHERE Username = '{username}';";
            this.dtConnection.Open();
            using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, this.dtConnection))
            {
                object selectResult = selectCommand.ExecuteScalar();
                result = Convert.ToInt16(selectResult);
            }

            this.dtConnection.Close();
            return result;
        }

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
            this.dtConnection.Open();

            string countQuery = "SELECT COUNT(*) FROM Orders";
            using (SQLiteCommand countCommand = new SQLiteCommand(countQuery, this.dtConnection))
            {
                object countResult = countCommand.ExecuteScalar();
                result = Convert.ToInt16(countResult);
            }

            this.dtConnection.Close();

            return result + 1;
        }

        /*        public void CreateOrder(string customerID, string sendAddress, string receiveAddress, int contentIndex, int typeIndex, int isExpensive, double weight, string phone, double cost)
                {
                    int nextOrderID = NextOrderID();
                    string insertQuery = $"INSERT INTO Orders (CreationDate, OrderID, CustomerID, SendAddress, ReceiveAddress, ContentIndex, TypeIndex, Expensive, Weight, Phone, Cost) VALUES ({DateTime.Now}, {nextOrderID}, '{customerID}', '{sendAddress}', '{receiveAddress}', {contentIndex}, {typeIndex}, {isExpensive}, {weight}, '{phone}', {cost});";
                    dtConnection.Open();

                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, this.dtConnection))
                    {
                        insertCommand.ExecuteNonQuery();
                    }
                    dtConnection.Close();
                }*/
        public void CreateOrder(string customerID, string sendAddress, string receiveAddress, int contentIndex, int typeIndex, int isExpensive, double weight, string phone, double cost)
        {
            int nextOrderID = NextOrderID();

            string insertQuery = "INSERT INTO Orders (CreationDate, OrderID, CustomerID, SendAddress, ReceiveAddress, ContentIndex, TypeIndex, Expensive, Weight, Phone, Cost) " +
                "VALUES (@CreationDate, @OrderID, @CustomerID, @SendAddress, @ReceiveAddress, @ContentIndex, @TypeIndex, @IsExpensive, @Weight, @Phone, @Cost)";

            dtConnection.Open();

            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, this.dtConnection))
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

                insertCommand.ExecuteNonQuery();
            }

            dtConnection.Close();
        }

        public void QueryToCSV(string fileName, string query)
        {
            this.dtConnection.Open();

            using (SQLiteCommand sqlCommand = new SQLiteCommand(this.dtConnection))
            {
                sqlCommand.CommandText = $".output {fileName}";
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = query;
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = $".output stdout";
                sqlCommand.ExecuteNonQuery();
            }

            this.dtConnection.Close();
        }

    }

}
