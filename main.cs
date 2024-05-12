using System;
using System.Data.SQLite;

namespace Project
{
    class Program
    {
        static void Main(string[] args) 
        {
            RMain();
        }
        public static void RMain()
        {
            DataDB dataDB = new DataDB();
            UsersDB usersDB = new UsersDB(dataDB);
            Registration registration = new Registration(usersDB);  
            bool t = true;
            while (t)
            {
                if (registration.LogStatus())
                {
                    t = false; 
                    break;
                }
                Console.WriteLine("Choose an option using a corresponding number:\n1. Register\n2. Login\n3. Exit");
                try
                {
                    int choice = int.Parse(Console.ReadLine()?? throw new ArgumentException("Choice cannot be empty!"));
                    switch (choice)
                    {
                        case 1:
                            registration.Register();
                            if (registration.LogStatus())
                            {
                                Console.WriteLine("You are logged in!");
                            }
                            if (registration.isLogged)
                            {   
                                Logic.MainLogic(); 
                            }
                            break;
                        case 2:
                            registration.Login();
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Invalid input! Please enter a number.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                } 
            }
        }
    }   
}



// private static string connectionString = "Data Source=data.db;Version=3;";
//  using (SQLiteConnection connection = new SQLiteConnection(connectionString))
//             {
//                 connection.Open();
//                 CreateTable(connection);
//                 InsertData(connection, "John", 25);
//                 InsertData(connection, "Jane", 30);
//             }
//         }

//         private static void CreateTable(SQLiteConnection connection)
//         {
//             string createTableQuery = "CREATE TABLE IF NOT EXISTS dataTEST (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, age INTEGER)";
//             using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
//             {
//                 command.ExecuteNonQuery();
//             }
//         }

//         private static void InsertData(SQLiteConnection connection, string name, int age)
//         {
//             string insertDataQuery = "INSERT INTO dataTEST (name, age) VALUES (@name, @age)";
//             using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
//             {
//                 command.Parameters.AddWithValue("@name", name);
//                 command.Parameters.AddWithValue("@age", age);
//                 command.ExecuteNonQuery();
//             }