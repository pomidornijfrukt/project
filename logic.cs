using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
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
                            Console.Clear();
                            new Logic().AddData();
                            break;
                        case 2:
                            Console.Clear();
                            DataDB dataDB = new DataDB();
                            dataDB.ShowDataWithinTimePeriod();
                            break;
                        case 3:
                            Console.Clear();
                            Logic logic = new Logic();
                            EditData();
                            break;
                        case 4:
                            Console.Clear();
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
                        Console.Clear();
                        try
                        {
                            DateTime startDate;
                            while (true)
                            {
                                Console.Write("Enter the start date(MM/dd/yyyy HH:mm:ss):\n(Ctrl + C to go back): ");
                                string startDateInput = Console.ReadLine();
                                if (DateTime.TryParseExact(startDateInput, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Start date time is not in the correct format! Please try again.");
                                }
                            }

                            DateTime endDate;
                            while (true)
                            {
                                Console.WriteLine("Enter the end date(MM/dd/yyyy HH:mm:ss): ");
                                string endDateInput = Console.ReadLine();
                                if (DateTime.TryParseExact(endDateInput, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("End date time is not in the correct format! Please try again.");
                                }
                            }

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
                                { "Username", activeUser.GetUsername() },
                                { "StartDate", startDate },
                                { "EndDate", endDate }
                            };
                            MainData.AddData(typeOfData, data);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 2:
                        Console.Clear();
                        LogicBasicInput();
                        break;
                    case 3:
                        Console.Clear();
                        MainLogic();
                        break;
                }
            }
        }
        public static void EditData()
        {
            Console.Clear();
            // Showing data to user
            DataDB dataDB = new DataDB();
            var activeUser = UsersDB.GetActiveUser();
            if (activeUser == null)
            {
                Console.WriteLine("Problem with code logic, relogin please.");
                MainLogic();
            }
            dataDB.ShowData(activeUser.GetUsername());

            Console.WriteLine("Enter the table name of the data that you want to edit: ");
            string tablename = Console.ReadLine() ?? throw new ArgumentException("Error: Invalid input!");
            Console.WriteLine("Enter the ID of the data that you want to edit: ");
            int id = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");

            Console.WriteLine("Choose an option using a corresponding number:\n1. Edit start date\n2. Edit end date\n3. DeleteData\n4. Go back");
            int choice = int.TryParse(Console.ReadLine(), out int option) ? option : throw new ArgumentException("Error: Invalid input!");
            
            switch (choice)
            {
                case 1:
                    Console.Write("Enter the start date(MM/dd/yyyy HH:mm:ss):\n ");
                    DateTime newStartDate = DateTime.Parse(Console.ReadLine());
                    dataDB.UpdateDataWithDate(id, activeUser.GetUsername(), tablename, newStartDate, true);
                    Console.WriteLine("Start date updated successfully!");
                    break;
                case 2:
                    Console.Write("Enter the end date(MM/dd/yyyy HH:mm:ss):\n ");
                    DateTime newEndDate = DateTime.Parse(Console.ReadLine() ?? throw new ArgumentException("End date time cannot be empty!"));
                    dataDB.UpdateDataWithDate(id, activeUser.GetUsername(), tablename, newEndDate, false);
                    Console.WriteLine("End date updated successfully!");
                    break;
                case 3:
                    Console.Clear();
                    dataDB.DeleteData(id, tablename);
                    Console.WriteLine("Fine! there is your new data!\n");
                    dataDB.ShowData(activeUser.GetUsername());
                    break;
                case 4:
                    MainLogic();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

        
        public void LogicBasicInput()
        {
            bool t = true;
            while(t)
            {
                Console.WriteLine("Choose an option using a corresponding number:\n1. Add data\n2. Go Back");
                try
                {
                    int choice = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");
                    switch (choice)
                    {
                        case 1:
                            DateTime now = DateTime.Now;
                            DateTime startDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                            Console.WriteLine("The timer has started. Press any key to stop the timer.");
                            while (!Console.KeyAvailable) // Wait until a key is pressed
                            {
                                Thread.Sleep(100);
                            }
                            now = DateTime.Now;
                            DateTime endDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
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
                                { "username", activeUser.GetUsername() }
                            };
                            MainData.AddData(typeOfData, data);
                            break;
                        case 2:
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