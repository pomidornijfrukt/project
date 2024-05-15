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

        public void CreateDatabaseTable(string tableName, Dictionary<string, string> columns, string dataSource)
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

                    // Remove the last commas
                    columnsBuilder.Length--;
                    valuesBuilder.Length--;

                    // Creating table if it doesn't exist
                    var columns = new Dictionary<string, string>
                    {
                        { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                        { "Username", "TEXT NOT NULL" },
                        { "StartDate", "TEXT NOT NULL" },
                        { "EndDate", "TEXT NOT NULL" }
                    };
                    CreateDatabaseTable(tableName, columns);

                    command.CommandText = $"INSERT INTO {tableName} ({columnsBuilder}) VALUES ({valuesBuilder})";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddData(string tablename, Dictionary<string, object> data, string dataSource)
        {
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

                    // Remove the last commas
                    columnsBuilder.Length--;
                    valuesBuilder.Length--;

                    // Creating table if it doesn't exist
                    var columns = new Dictionary<string, string>
                    {
                        { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                        { "Username", "TEXT NOT NULL" },
                        { "StartDate", "TEXT NOT NULL" },
                        { "EndDate", "TEXT NOT NULL" }
                    };
                    CreateDatabaseTable(tablename, columns);

                    // Adding data to the table
                    command.CommandText = $"INSERT INTO {tablename} ({columnsBuilder}) VALUES ({valuesBuilder})";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ShowData(string username)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader.GetString(0);
                            ShowDataFromTable(tableName, username);
                        }
                    }
                }
            }
        }

        private void ShowDataFromTable(string tableName, string username)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"PRAGMA table_info({tableName})";
                    using (var reader = command.ExecuteReader())
                    {
                        bool hasUsernameColumn = false;
                        while (reader.Read())
                        {
                            if (reader.GetString(reader.GetOrdinal("name")).Equals("Username", StringComparison.OrdinalIgnoreCase))
                            {
                                hasUsernameColumn = true;
                                break;
                            }
                        }

                        if (!hasUsernameColumn)
                        {
                            return;
                        }
                    }
                    command.CommandText = $"SELECT * FROM {tableName} WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StringBuilder output = new StringBuilder();
                            output.Append($"Table: {tableName}, ");
                            output.Append($"StartDate: {reader.GetValue(reader.GetOrdinal("StartDate"))}, ");
                            output.Append($"EndDate: {reader.GetValue(reader.GetOrdinal("EndDate"))}");
                            Console.WriteLine(output);
                        }
                    }
                }
            }
        }

        public void ShowDataWithinTimePeriod(List<string> columnNames)
        {
            var activeUser = UsersDB.GetActiveUser();
            if (activeUser == null)
            {
                Console.WriteLine("Programs logic has crashed a bit, relogin to make it working");
                Program.RMain();
            }

            TimeSpan totalSpan = TimeSpan.Zero;
            bool firstInput = true;
            while (true)
            {
                Console.WriteLine("Enter how long ago to see. 1.Month 2.Weeks 3.Days 4.Hours" + (firstInput ? "" : " 5.Continue"));
                int timeUnit = Convert.ToInt32(Console.ReadLine());
                if (timeUnit == 5 && !firstInput)
                {
                    break;
                }
                Console.WriteLine("Enter the amount of time units.");
                int amount = Convert.ToInt32(Console.ReadLine());

                TimeSpan timeSpan;
                switch (timeUnit)
                {
                    case 1:
                        timeSpan = TimeSpan.FromDays(amount * 30); // Approximate a month as 30 days
                        break;
                    case 2:
                        timeSpan = TimeSpan.FromDays(amount * 7); // A week is 7 days
                        break;
                    case 3:
                        timeSpan = TimeSpan.FromDays(amount);
                        break;
                    case 4:
                        timeSpan = TimeSpan.FromHours(amount);
                        break;
                    default:
                        Console.WriteLine("Invalid time unit.");
                        return;
                }
                totalSpan += timeSpan;
                firstInput = false;
            }

            DateTime cutoff = DateTime.Now - totalSpan;

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader.GetString(0);
                            ShowDataFromTableWithinTimePeriod(tableName, columnNames, activeUser.GetUsername(), cutoff);
                        }
                    }
                }
            }
        }

        private void ShowDataFromTableWithinTimePeriod(string tableName, List<string> columnNames, string username, DateTime cutoff)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"SELECT * FROM {tableName} WHERE username = @Username AND endDate >= @Cutoff";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Cutoff", cutoff);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StringBuilder output = new StringBuilder();
                            for (int i = 0; i < columnNames.Count; i++)
                            {
                                if (columnNames[i] != "username")
                                {
                                    if (columnNames[i] == "endDate" && Convert.ToDateTime(reader.GetValue(i)) < cutoff)
                                    {
                                        output.Append($"{columnNames[i]}: {cutoff}, ");
                                    }
                                    else
                                    {
                                        output.Append($"{columnNames[i]}: {reader.GetValue(i)}, ");
                                    }
                                }
                            }
                            // Remove the last comma and space
                            output.Length -= 2;
                            Console.WriteLine(output);
                        }
                    }
                }
            }
        }

        public void UpdateData(int id, string username, string typeOfData, DateTime startDate, DateTime endDate)
        {
            // Creating table if it doesn't exist
            var columns = new Dictionary<string, string>
            {
                { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                { "Username", "TEXT NOT NULL" },
                { "StartDate", "TEXT NOT NULL" },
                { "EndDate", "TEXT NOT NULL" }
            };
            CreateDatabaseTable(typeOfData, columns);

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"UPDATE {typeOfData} SET username = @Username, startDate = @StartDate, endDate = @EndDate WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDataWithStartDate(int id, string username, string typeOfData, DateTime startDate)
        {
            // Creating table if it doesn't exist
            var columns = new Dictionary<string, string>
            {
                { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                { "Username", "TEXT NOT NULL" },
                { "StartDate", "TEXT NOT NULL" },
                { "EndDate", "TEXT NOT NULL" }
            };
            CreateDatabaseTable(typeOfData, columns);

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"UPDATE {typeOfData} SET username = @Username, startDate = @StartDate WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDataWithEndDate(int id, string username, string typeOfData, DateTime endDate)
        {
            // Creating table if it doesn't exist
            var columns = new Dictionary<string, string>
            {
                { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                { "Username", "TEXT NOT NULL" },
                { "StartDate", "TEXT NOT NULL" },
                { "EndDate", "TEXT NOT NULL" }
            };
            CreateDatabaseTable(typeOfData, columns);

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"UPDATE {typeOfData} SET username = @Username, endDate = @EndDate WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}