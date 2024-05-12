using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
namespace Project
{
    public class DataDB
    {
        protected UsersDB? usersDB;
        protected string dataSource = "Data Source=/workspaces/project/data.db";

        public void CreateDatabaseTable(string tableName, Dictionary<string, string> columns)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    StringBuilder columnDefinitions = new StringBuilder();

                    foreach (var pair in columns)
                    {
                        columnDefinitions.Append($"{pair.Key} {pair.Value}, ");
                    }

                    // Remove the last comma and space
                    columnDefinitions.Length -= 2;

                    command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} ({columnDefinitions})";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddData(string tableName, Dictionary<string, object> data)
        {
            // Creating table if it doesn't exist
            var columns = new Dictionary<string, string>();
            foreach (var pair in data)
            {
                columns[pair.Key] = "TEXT NOT NULL";
            }
            CreateDatabaseTable(tableName, columns);

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    StringBuilder columnsBuilder = new StringBuilder();
                    StringBuilder valuesBuilder = new StringBuilder();

                    foreach (var pair in data)
                    {
                        columnsBuilder.Append(pair.Key + ",");
                        valuesBuilder.Append("@" + pair.Key + ",");
                        command.Parameters.AddWithValue("@" + pair.Key, pair.Value);
                    }

                    // Remove the trailing commas
                    columnsBuilder.Length--;
                    valuesBuilder.Length--;

                    command.CommandText = $"INSERT INTO {tableName} ({columnsBuilder}) VALUES ({valuesBuilder})";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ShowData(string tableName, List<string> columnNames)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"SELECT * FROM {tableName}";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StringBuilder output = new StringBuilder();
                            for (int i = 0; i < columnNames.Count; i++)
                            {
                                output.Append($"{columnNames[i]}: {reader.GetValue(i)}, ");
                            }
                            // Remove the last comma and space
                            output.Length -= 2;
                            Console.WriteLine(output);
                        }
                    }
                }
            }
        }
        public void UpdateData(string tableName, int id, string username, string typeOfData, DateTime startDate, DateTime endDate)
        {
            // Creating table if it doesn't exist
            var columns = new Dictionary<string, string>
            {
                { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                { "Username", "TEXT NOT NULL" },
                { "TypeOfData", "TEXT NOT NULL" },
                { "StartDate", "TEXT NOT NULL" },
                { "EndDate", "TEXT NOT NULL" }
            };
            CreateDatabaseTable(tableName, columns);
            var activeUser = UsersDB.GetActiveUser();
            if (activeUser == null)
                {
                    Console.WriteLine("Problem with code logic, relogin please.");
                    Logic.MainLogic();
                }
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"UPDATE {tableName} SET username = @Username, typeOfData = @TypeOfData, startDate = @StartDate, endDate = @EndDate WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Username", activeUser.GetUsername());
                    command.Parameters.AddWithValue("@TypeOfData", typeOfData);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}