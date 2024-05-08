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
                if (registration.LogStatus() == true)
                {
                    t = false;  
                }  
                Console.WriteLine("Choose an option using a corresponding number:\n1. Register\n2. Login\n3. Exit");
                int choice = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");
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
            if (registration.LogStatus() == true)
            {
                Logic.MainLogic(); 
            } 
        }
    }

}