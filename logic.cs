using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Project;
namespace Project
{
    class Logic
    {
        public static void MainLogic()
        {
            bool t = true;
            while(t)
            {
                Console.WriteLine("Choose an option using a corresponding number:\n1. Add data\n2. Show data\n3. Edit data\n4. Go back");
                try
                {
                    int choice = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");
                    switch (choice)
                    {
                        case 1:
                            Logic logic1 = new();
                            logic1.AddData();
                            break;
                        case 2:
                            DataDB dataDB = new DataDB();
                            var columns = new List<string> { "startDate", "endDate", "typeOfData", "username"};
                            dataDB.ShowData("data", columns);
                            break;
                        case 3:
                            Logic logic = new Logic();
                            logic.EditData();
                            break;
                        case 4:
                            Program.RMain();
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public void AddData()
        {
            while (true)
            {
                Console.WriteLine("Choose an option using a corresponding number:\n1. Add data\n2. Start adding data.\n3. Go back.");
                int choice = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");
                switch (choice)
                {
                    case 1:
                        try
                        {
                            Console.WriteLine("Enter the start date(MM/dd/yyyy HH:mm): ");
                            DateTime startDate = DateTime.Parse(Console.ReadLine()?? throw new ArgumentException("Start date time cannot be empty!"));
                            Console.WriteLine("Enter the end date(MM/dd/yyyy HH:mm): ");
                            DateTime endDate = DateTime.Parse(Console.ReadLine()?? throw new ArgumentException("End date time cannot be empty!"));
                            Console.WriteLine("Enter the type of data: ");
                            string typeOfData = Console.ReadLine()?? throw new ArgumentException("Type of data cannot be empty!");

                            var activeUser = UsersDB.GetActiveUser();
                            if (activeUser == null)
                            {
                                Console.WriteLine("Problem with code logic, relogin please.");
                                MainLogic();
                            }
                            DataDB MainData = new();
                            var data = new Dictionary<string, object>
                            {
                                { "startDate", startDate },
                                { "endDate", endDate },
                                { "typeOfData", typeOfData },
                                { "username", activeUser.GetUsername() }
                            };
                            MainData.AddData("data", data);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Error: Invalid date format! Please enter a valid date.");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                        break;
                    case 2:
                        ContinueLogic();
                        break;
                    case 3:
                        MainLogic();
                        break;
                }
            }
        }
        public void EditData()
        {
            DataDB dataDB = new DataDB();
            var columns = new List<string> { "startDate", "endDate", "typeOfData", "username" };
            dataDB.ShowData("data", columns);

            Console.WriteLine("Enter the ID of the data you want to edit: ");
            int id = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");

            Console.WriteLine("Choose an option using a corresponding number:\n1. Edit start date\n2. Edit end date\n3. Go back");
            int choice = int.TryParse(Console.ReadLine(), out int option) ? option : throw new ArgumentException("Error: Invalid input!");

            var activeUser = UsersDB.GetActiveUser();

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the new start date(MM/dd/yyyy HH:mm): ");
                    DateTime newStartDate = DateTime.Parse(Console.ReadLine() ?? throw new ArgumentException("Start date time cannot be empty!"));
                    dataDB.UpdateDataWithStartDate(id, activeUser.GetUsername(), "startDate", newStartDate);
                    Console.WriteLine("Start date updated successfully!");
                    break;
                case 2:
                    Console.WriteLine("Enter the new end date(MM/dd/yyyy HH:mm): ");
                    DateTime newEndDate = DateTime.Parse(Console.ReadLine() ?? throw new ArgumentException("End date time cannot be empty!"));
                    dataDB.UpdateDataWithEndDate(id, activeUser.GetUsername(), "endDate", newEndDate);
                    Console.WriteLine("End date updated successfully!");
                    break;
                case 3:
                    MainLogic();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

        
        public void ContinueLogic()
        {
            bool t = true;
            while(t)
            {
                Console.WriteLine("Choose an option using a corresponding number:\n1. Add data\n3. Go Back");
                try
                {
                    int choice = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");
                    switch (choice)
                    {
                        case 1:
                            Logic logic = new();
                            logic.AddData();
                            break;
                        // case 2:
                        //     DataDB dataDB = new DataDB();
                        //     dataDB.ShowData();
                        //     break;
                        case 3:
                            MainLogic();
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}