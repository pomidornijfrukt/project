using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

class Logic
{
    public static void MainLogic()
    {
        bool t = true;
        while(t){
            Console.WriteLine("Choose an option using a corresponding number:\n1. Add data\n2. Show data\n3. Exit");
        }
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
    public void AddData()
    {
        Console.WriteLine("Enter the start date: ");
        DateTime startDate = DateTime.Parse(Console.ReadLine()?? throw new ArgumentException("Start date time cannot be empty!"));
        Console.WriteLine("Enter the end date: ");
        DateTime endDate = DateTime.Parse(Console.ReadLine()?? throw new ArgumentException("End date time cannot be empty!"));
        Console.WriteLine("Enter the type of data: ");
        string typeOfData = Console.ReadLine()?? throw new ArgumentException("Type of data cannot be empty!");
        UsersDB someData = new(UsersDB.password, UsersDB.username);
        DataDB MainData = new();
        MainData.AddData(Registraion.currentUser, startDate, endDate, typeOfData);
    }
}