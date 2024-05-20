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
        
        // Adjusts the input DateTime to GMT+3
        public static DateTime GetTimeInGTMPlus3(DateTime dateTime)
        {
            return dateTime.AddHours(3);
        }
        
        // Truncates the input DateTime to the nearest minute
        public static DateTime TruncateToMinute(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }

        // Deletes a record from the specified table based on dataId and active user's username
        public void DeleteData(int dataId, string tableName)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    UsersDB user = UsersDB.GetActiveUser();
                   
                    string sqlQuery = $"DELETE FROM {tableName} WHERE Id = @Id AND Username = @Username";
                    command.Parameters.AddWithValue("@Id", dataId);
                    command.Parameters.AddWithValue("@Username", user.GetUsername());
                    
                    command.CommandText = sqlQuery;
                    command.ExecuteNonQuery();
                }
            }
        }

        // Creates a table with the specified columns if it doesn't exist
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

        // Overloaded method to create a table with a custom data source
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

        // Adds data to the specified table
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

        // Overloaded method to add data with a custom data source
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

        // Displays all data for the specified user
        public void ShowData(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Data source or username is null or empty.");
                return;
            }
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            Console.WriteLine("Failed to execute reader.");
                            return;
                        }
                        while (reader.Read())
                        {
                            string tableName = reader.GetString(0);
                            ShowDataFromTable(tableName, username);
                        }
                    }
                }
            }
        }

        // Displays data from a specific table for the specified user
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
                            output.Append($"ID: {reader.GetValue(reader.GetOrdinal("ID"))}, ");
                            output.Append($"StartDate: {reader.GetValue(reader.GetOrdinal("StartDate"))}, ");
                            output.Append($"EndDate: {reader.GetValue(reader.GetOrdinal("EndDate"))}");
                            Console.WriteLine(output);
                        }
                    }
                }
            }
        }

        // Displays data within a user-specified time period
        public void ShowDataWithinTimePeriod()
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
                Console.WriteLine("Enter how long ago to see. 1.Years 2.Month 3.Weeks 4.Days 5.Hours 6.Minutes" + (firstInput ? "" : " 5.Continue"));
                if (!int.TryParse(Console.ReadLine(), out int timeUnit))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }
                if (timeUnit == 5 && !firstInput)
                {
                    break;
                }
                Console.WriteLine("Enter the amount of time units.");
                if (!int.TryParse(Console.ReadLine(), out int amount) || amount < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a positive number.");
                    continue;
                }

                TimeSpan timeSpan;
                switch (timeUnit)
                {
                    case 1:
                        timeSpan = TimeSpan.FromDays(amount * 365); // Approximate a year as 365 days
                        break;
                    case 2:
                        timeSpan = TimeSpan.FromDays(amount * 30); // Approximate a month as 30 days
                        break;
                    case 3:
                        timeSpan = TimeSpan.FromDays(amount * 7); // A week is 7 days
                        break;
                    case 4:
                        timeSpan = TimeSpan.FromDays(amount);
                        break;
                    case 5:
                        timeSpan = TimeSpan.FromHours(amount);
                        break;
                    case 6:
                        timeSpan = TimeSpan.FromMinutes(amount);
                        break;
                    default:
                        Console.WriteLine("Invalid time unit.");
                        return;
                }
                totalSpan += timeSpan;
                firstInput = false;
            }

            Console.Clear();

            DateTime cutoff = TruncateToMinute(GetTimeInGTMPlus3(DateTime.Now)) - totalSpan;

            try
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
                                ShowDataFromTableWithinTimePeriod(tableName, activeUser.GetUsername(), cutoff);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Displays data from a specific table within a specified time period for the user
        private void ShowDataFromTableWithinTimePeriod(string tableName, string username, DateTime cutoff)
        {
            // Skip the sqlite_sequence table
            if (tableName == "sqlite_sequence")
            {
                return;
            }

            TimeSpan totalSpan = TimeSpan.Zero;

            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // Get column names
                    command.CommandText = $"PRAGMA table_info({tableName})";
                    var columnNames = new List<string>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnNames.Add(reader.GetString(1)); // Column name is in the second column of the result set
                        }
                    }

                    // Get data
                    command.CommandText = $"SELECT * FROM {tableName} WHERE Username = @Username AND EndDate >= @Cutoff";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Cutoff", cutoff);
                    using (var reader = command.ExecuteReader())
                    {
                        DateTime StartDate = new(), EndDate = new();
                        while (reader.Read())
                        {
                            for (int i = 0; i < columnNames.Count; i++)
                            {
                                if (columnNames[i] != "Username")
                                {
                                    if (columnNames[i] == "EndDate")
                                    {
                                        EndDate = Convert.ToDateTime(reader.GetValue(i));
                                        if (EndDate < cutoff)
                                        {
                                            continue;
                                        }
                                        else if (EndDate > GetTimeInGTMPlus3(DateTime.Now))
                                        {
                                            EndDate = TruncateToMinute(GetTimeInGTMPlus3(DateTime.Now));
                                        }
                                    } 
                                    else if (columnNames[i] == "StartDate")
                                    {
                                        StartDate = Convert.ToDateTime(reader.GetValue(i));
                                        if (StartDate < cutoff)
                                        {
                                            StartDate = cutoff;
                                        }
                                    }                           
                                }
                            }
                            totalSpan += EndDate - StartDate;
                        }
                    }
                }
            }

            Console.WriteLine($"Total time for {tableName}: {TimeSpanDiff(DateTime.Now, DateTime.Now - totalSpan)}");
        }

        // Calculates the difference between two DateTime objects and formats it as a string
        public static string TimeSpanDiff(DateTime dateTime, DateTime dateTime2)
        {
            TimeSpan difference = dateTime - dateTime2;

            int years = difference.Days / 365; // Number of years
            difference = difference.Add(TimeSpan.FromDays(-years * 365)); // Subtract the years

            int months = difference.Days / 30; // Number of months
            difference = difference.Add(TimeSpan.FromDays(-months * 30)); // Subtract the months

            int weeks = difference.Days / 7; // Number of weeks
            difference = difference.Add(TimeSpan.FromDays(-weeks * 7)); // Subtract the weeks

            int days = difference.Days; // Number of days
            int hours = difference.Hours; // Number of hours
            int minutes = difference.Minutes; // Number of minutes

            StringBuilder output = new StringBuilder();

            var timeUnits = new List<(int, string)>
            {
                (years, "year(s)"),
                (months, "month(s)"),
                (weeks, "week(s)"),
                (days, "day(s)"),
                (hours, "hour(s)"),
                (minutes, "minute(s)")
            };

            foreach (var (value, unit) in timeUnits)
            {
                switch (value)
                {
                    case int v when v > 0:
                        output.Append($"{value} {unit}, ");
                        break;
                }
            }

            // Remove the last comma and space
            if (output.Length > 0)
            {
                output.Remove(output.Length - 2, 2);
            }

            return output.ToString();
        }

        // Updates a record in the specified table with new data
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
                    command.CommandText = $"UPDATE {typeOfData} SET Username = @Username, StartDate = @StartDate, EndDate = @EndDate WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Updates either the StartDate or EndDate of a record in the specified table
        public void UpdateDataWithDate(int id, string username, string typeOfData, DateTime date, bool isStartDate)
        {
            try
            {
                // Ensure the table exists
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
                        DateTime truncatedDate = TruncateToMinute(date);
                        string dateType = isStartDate ? "StartDate" : "EndDate";

                        command.CommandText = $"UPDATE {typeOfData} SET {dateType} = @{dateType} WHERE Id = @Id AND Username = @Username";
                        command.Parameters.AddWithValue($"@{dateType}", truncatedDate.ToString("yyyy-MM-dd HH:mm")); // Ensure correct format
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Username", username);

                        // Debugging: Print the final SQL query and parameters
                        Console.WriteLine($"Executing SQL: UPDATE {typeOfData} SET {dateType} = @{dateType} WHERE Id = @Id AND Username = @Username");
                        Console.WriteLine($"Parameters: {dateType}={truncatedDate}, Id={id}, Username={username}");

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("No rows updated. Please check the ID and username.");
                        }
                        else
                        {
                            Console.WriteLine($"{dateType} updated successfully!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating {typeOfData}: {ex.Message}");
            }
        }
    }
}