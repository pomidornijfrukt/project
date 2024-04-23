using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

class UsersDB
{
    protected string username;
    protected string password;

    public UsersDB(string username, string password){
        this.username = username;
        this.password = password;
    }

    static void AddUsers(){
        using (var connection = new SQLiteConnection("Data Source=usermanager&userlog.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO users (username, password) VALUES ($'{username}', '{password}')";
                command.ExecuteNonQuery();
            }
        }
    }

    public void ShowUsers(){
        using (var connection = new SQLiteConnection("Data Source=usermanager&userlog.db"))
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
        using (var connection = new SQLiteConnection("Data Source=usermanager&userlog.db"))
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

                // Remove the last comma and space
                sb.Length -= 2;

                sb.Append(")");

                command.CommandText = sb.ToString();
                command.ExecuteNonQuery();
            }
        }
    }

    // string connectionString = "Data Source=usermanager&userlog.db";
    // using (var connection = new SQLiteConnection(connectionString))
    // {
    //     connection.Open();
    // using var command = new SQLiteCommand(connection);
    // command.CommandText = "CREATE TABLE IF NOT EXISTS users (username TEXT, password TEXT)";
    // command.CommandText = "INSERT INTO users (username, password) VALUES ('admin', 'admin')";
    // command.ExecuteNonQuery();
// }
    
}








// string connectionString = "Data Source=usermanager&userlog.db";
// using (var connection = new SQLiteConnection(connectionString))
// {
//     connection.Open();
//     using (var command = new SQLiteCommand(connection))
//     {
//         command.CommandText = "CREATE TABLE IF NOT EXISTS users (username TEXT, password TEXT)";
//         command.CommandText = "INSERT INTO users (username, password) VALUES ('admin', 'admin')";
//         command.ExecuteNonQuery();
//     }

// }