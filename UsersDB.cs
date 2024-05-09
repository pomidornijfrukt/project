using System;
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
        private string dataSource = "Data Source=usermanager&userlog.db;";

        public UsersDB(string username, string password)
        {
            this.username = username;
            this.salt = GenerateSalt();
            this.password = HashPassword(password, this.salt);
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
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO users (username, password, salt) VALUES (@username, @password, @salt)";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", hashedPassword);
                    command.Parameters.AddWithValue("@salt", salt);
                    command.ExecuteNonQuery();
                }
            }

            UsersDB newUser = new UsersDB(username, hashedPassword);
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
                            Console.WriteLine($"Username: {reader.GetString(0)}, Password: {reader.GetString(1)}, Salt: {reader.GetString(2)}");
                        }
                    }
                }
            }
        }

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

        private string GenerateSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}