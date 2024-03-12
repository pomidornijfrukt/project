using System;
using registration;

namespace Project
{
    class Program {
        static void Main(string[] args) {
            while (true){
                Console.WriteLine("Choose an option using a corresponding number:\n1. Register\n2. Login\n3. Exit");
                int choice = int.TryParse(Console.ReadLine(), out int result) ? result : ArgumentException("Error: Invalid input!");
                switch choice {
                    case 1:
                        registraion.register();
                        break;
                    case 2:
                        registration.login();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }
    }
}
