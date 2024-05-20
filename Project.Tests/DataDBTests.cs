using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Xunit;

namespace Project.Tests
{
    public class DataDBTests
    {
        private readonly string testDbPath = "/workspaces/project/Project.Tests/data.db";
        private readonly string connectionString;

        public DataDBTests()
        {
            connectionString = $"Data Source={testDbPath}";
            if (File.Exists(testDbPath))
            {
                File.Delete(testDbPath);
            }
            SetupTestDatabase();
        }

        private void SetupTestDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        CREATE TABLE Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL
                        );

                        CREATE TABLE TestTable (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL,
                            StartDate TEXT NOT NULL,
                            EndDate TEXT NOT NULL
                        );

                        INSERT INTO Users (Username) VALUES ('testuser');";

                    command.ExecuteNonQuery();
                }
            }
        }

        private void SetDataSource(DataDB dataDB, string dataSource)
        {
            var dataSourceField = typeof(DataDB).GetField("dataSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (dataSourceField != null)
            {
                dataSourceField.SetValue(dataDB, dataSource);
            }
            else
            {
                throw new Exception("dataSource field not found");
            }
        }

        private void SetActiveUser(string username)
        {
            var user = new UsersDB();
            user.SetUsername(username);
            UsersDB.SetActiveUser(user);
        }

        [Fact]
        public void GetTimeInGTMPlus3_ShouldAddThreeHours()
        {
            // Arrange
            DateTime input = new DateTime(2024, 5, 20, 12, 0, 0);
            DateTime expected = new DateTime(2024, 5, 20, 15, 0, 0);

            // Act
            DateTime result = DataDB.GetTimeInGTMPlus3(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TruncateToMinute_ShouldTruncateSeconds()
        {
            // Arrange
            DateTime input = new DateTime(2024, 5, 20, 12, 34, 56);
            DateTime expected = new DateTime(2024, 5, 20, 12, 34, 0);

            // Act
            DateTime result = DataDB.TruncateToMinute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TimeSpanDiff_ShouldReturnCorrectDifference()
        {
            // Arrange
            DateTime start = new DateTime(2024, 5, 20, 12, 0, 0);
            DateTime end = new DateTime(2024, 5, 20, 15, 45, 30);
            string expected = "3 hour(s), 45 minute(s)";

            // Act
            string result = DataDB.TimeSpanDiff(end, start);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateDatabaseTable_ShouldExecuteCreateTableCommand()
        {
            // Arrange
            var dataDB = new DataDB();
            SetDataSource(dataDB, connectionString);
            var columns = new Dictionary<string, string>
            {
                { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                { "Username", "TEXT NOT NULL" }
            };

            // Act
            dataDB.CreateDatabaseTable("NewTable", columns);

            // Assert
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='NewTable';";
                    var result = command.ExecuteScalar();
                    Assert.NotNull(result);
                }
            }
        }

        [Fact]
        public void AddData_ShouldInsertData()
        {
            // Arrange
            var dataDB = new DataDB();
            SetDataSource(dataDB, connectionString);
            SetActiveUser("testuser");

            var data = new Dictionary<string, object>
            {
                { "Username", "testuser" },
                { "StartDate", "2024-05-20 12:00" },
                { "EndDate", "2024-05-20 15:00" }
            };

            // Act
            dataDB.AddData("TestTable", data);

            // Assert
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM TestTable WHERE Username = 'testuser';";
                    var result = command.ExecuteScalar();
                    Assert.Equal(1L, result);
                }
            }
        }

        [Fact]
        public void DeleteData_ShouldThrowExceptionWhenNoActiveUser()
        {
            // Arrange
            var dataDB = new DataDB();
            SetDataSource(dataDB, connectionString);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => dataDB.DeleteData(1, "TestTable"));
            Assert.Equal("The code logic has been broken! returning to the login...", exception.Message);
        }

        [Fact]
        public void UpdateDataWithDate_ShouldExecuteUpdateCommand()
        {
            // Arrange
            var dataDB = new DataDB();
            SetDataSource(dataDB, connectionString);
            SetActiveUser("testuser");

            dataDB.AddData("TestTable", new Dictionary<string, object>
            {
                { "Username", "testuser" },
                { "StartDate", "2024-05-20 12:00" },
                { "EndDate", "2024-05-20 15:00" }
            });
            var newDate = new DateTime(2024, 5, 21, 12, 0, 0);

            // Act
            dataDB.UpdateDataWithDate(1, "testuser", "TestTable", newDate, false);

            // Assert
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT EndDate FROM TestTable WHERE Id = 1;";
                    var result = command.ExecuteScalar();
                    Assert.Equal(newDate.ToString("yyyy-MM-dd HH:mm"), result);
                }
            }
        }
    }

    public class UsersDB
    {
        private static UsersDB? activeUser;
        private string? username;

        public static UsersDB GetActiveUser() => activeUser;

        public void SetUsername(string username) => this.username = username;
        public string GetUsername() => username ?? throw new InvalidOperationException("Username is not set.");

        public static void SetActiveUser(UsersDB user) => activeUser = user;
    }
}