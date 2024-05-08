using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
namespace Project
{
    class UsersDB
    {
        protected string username;
        protected string password;
        protected string dataSource = "Data Source=usermanager&userlog.db";
        private static UsersDB? ActiveUser { get; set; }

        public UsersDB(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public static void SetActiveUser(UsersDB user)
        {
            ActiveUser = user;
        }

        public static UsersDB? GetActiveUser()
        {
            return ActiveUser;
        }

        public string GetUsername()
        {
            return username;
        }

        public void AddUser(string username, string password)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
        
        UsersDB newUser = new UsersDB(username, password);
        SetActiveUser(newUser);
        }


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
                            Console.WriteLine($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}");
                        }
                    }
                }
            }
        }

        public void CreateTable(string tableName, Dictionary<string, string> columns)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"CREATE TABLE IF NOT EXISTS {tableName} (");

                    foreach (var column in columns)
                    {
                        sb.Append($"{column.Key} {column.Value}, ");
                    }

                    // Removes the last comma and space
                    sb.Length -= 2;

                    sb.Append(")");

                    command.CommandText = sb.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}