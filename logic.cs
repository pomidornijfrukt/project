using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Project;

class Logic
{
    public static void MainLogic()
    {
        bool t = true;
        while(t)
        {
            Console.WriteLine("Choose an option using a corresponding number:\n1. Add data\n2. Show data\n3. Exit");
            try
            {
                int choice = int.TryParse(Console.ReadLine(), out int result) ? result : throw new ArgumentException("Error: Invalid input!");
                switch (choice)
                {
                    case 1:
                        Logic logic = new();
                        logic.AddData();
                        break;
                    case 2:
                        DataDB dataDB = new DataDB();
                        dataDB.ShowData();
                        break;
                    case 3:
                        t = false;
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
        try
        {
            Console.WriteLine("Enter the start date: ");
            DateTime startDate = DateTime.Parse(Console.ReadLine()?? throw new ArgumentException("Start date time cannot be empty!"));
            Console.WriteLine("Enter the end date: ");
            DateTime endDate = DateTime.Parse(Console.ReadLine()?? throw new ArgumentException("End date time cannot be empty!"));
            Console.WriteLine("Enter the type of data: ");
            string typeOfData = Console.ReadLine()?? throw new ArgumentException("Type of data cannot be empty!");

            var activeUser = UsersDB.GetActiveUser();
            if (activeUser == null)
            {
                throw new Exception("No active user set");
            }

            DataDB MainData = new();
            MainData.AddData(activeUser, startDate, endDate, typeOfData);
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
    }
}