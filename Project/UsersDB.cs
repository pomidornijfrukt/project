using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Project
{
    public class UsersDB
    {
        private static UsersDB? ActiveUser;
        private string username;
        private string password;
        private string salt;
        private string dataSource = "Data Source=/workspaces/project/usersDB.db;";
        private DataDB dataDB;
        
        // Constructor to initialize UsersDB with a DataDB instance
        public UsersDB(DataDB dataDB)
        {
            this.dataDB = dataDB;
        }

        // Constructor to initialize UsersDB with username, password, and DataDB instance
        public UsersDB(string username, string password, DataDB dataDB)
        {
            this.username = username;
            this.salt = GenerateSalt();
            this.password = HashPassword(password, this.salt);
            this.dataDB = dataDB;
        }

        // Sets the active user
        public static void SetActiveUser(UsersDB user)
        {
            ActiveUser = user;
        }

        // Retrieves the active user
        public static UsersDB? GetActiveUser()
        {
            return ActiveUser;
        }

        // Gets the username of the current user
        public string GetUsername() => username;

        // Adds a new user to the database
        public void AddUser(string username, string password)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);

            // Creating table if it doesn't exist
            var columns = new Dictionary<string, string>
            {
                { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                { "username", "TEXT" },
                { "password", "TEXT" },
                { "salt", "TEXT" },
                { "creationTime", "TEXT" }
            };
            dataDB.CreateDatabaseTable("users", columns, dataSource);

            // Adding user data
            var data = new Dictionary<string, object>
            {
                { "username", username },
                { "password", hashedPassword },
                { "salt", salt },
                { "creationTime", DataDB.GetTimeInGTMPlus3(DateTime.Now).ToString() }
            };
            dataDB.AddData("users", data, dataSource);

            UsersDB newUser = new(username, hashedPassword, this.dataDB);
            SetActiveUser(newUser);
        }

        // Displays all users from the database
        public void ShowUsers()
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM users";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}, Salt: {reader.GetString(2)}");
                        }
                    }
                }
            }
        }

        // Hashes a password with a given salt using SHA256
        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Generates a random salt value
        private string GenerateSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // Validates a user's username and password
        public bool ValidateUser(string username, string password)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT password, salt FROM users WHERE username = @username";
                    command.Parameters.AddWithValue("@username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader.GetString(0);
                            string storedSalt = reader.GetString(1);
                            string hashedInputPassword = HashPassword(password, storedSalt);

                            if (storedPassword == hashedInputPassword)
                            {
                                SetActiveUser(new UsersDB(username, storedPassword, dataDB));
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        // Checks if a username already exists in the database
        public bool ValidateUserUsername(string username)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT username FROM users WHERE username = @username";
                    command.Parameters.AddWithValue("@username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}