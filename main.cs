using System;

namespace Project
{
    class Program
    {
        static void Main(string[] args) 
        {
            Registration registration = new Registration();
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
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice) 
                    {
                        case 1:
                            registration.Register();
                            if (registration.LogStatus())
                            {
                                Console.WriteLine("You are logged in!");
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
            if (registration.LogStatus())
            {
                Logic.MainLogic(); 
            } 
        }
    }
}