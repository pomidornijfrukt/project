using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
namespace Project
{
    class DataDB
    {
        protected UsersDB? usersDB;
        protected string dataSource = "Data Source=data.db";

        // public DataDB(UsersDB usersDB)
        // {
        //     this.usersDB = usersDB;
        // }

        public void AddData(UsersDB usersDB, DateTime startDate, DateTime endDate, string typeOfData)
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO data (user, startDate, endDate, typeOfData) VALUES (@user, @startDate, @endDate, @typeOfData)";
                    command.Parameters.AddWithValue("@user", usersDB);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@typeOfData", typeOfData);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ShowData()
        {
            using (var connection = new SQLiteConnection(dataSource))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM data";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"User: {reader.GetString(0)}, Start date: {reader.GetDateTime(1)}, End date: {reader.GetDateTime(2)}, Type of data: {reader.GetString(3)}");
                        }
                    }
                }
            }
        }

    }
}