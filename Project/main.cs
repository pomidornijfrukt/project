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

        // Main routine for registration and login
        public static void RMain()
        {
            Console.Clear();
            DataDB dataDB = new DataDB();
            UsersDB usersDB = new UsersDB(dataDB);
            Registration registration = new Registration(usersDB);  
            bool t = true;
            while (t)
            {
                // Check if the user is already logged in
                if (registration.LogStatus())
                {
                    t = false; 
                    break;
                }
                Console.WriteLine("Choose an option using a corresponding number:\n1. Register\n2. Login\n3. Exit");
                try
                {
                    int choice = int.Parse(Console.ReadLine() ?? throw new ArgumentException("Choice cannot be empty!"));
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
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
                            Console.Clear();
                            registration.Login();
                            if (registration.isLogged)
                            {   
                                Logic.MainLogic(); 
                            }
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