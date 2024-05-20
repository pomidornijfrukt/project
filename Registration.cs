using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Project
{
    class Registration
    {
        private UsersDB usersDB; // Database of users

        public bool isLogged = false; // Flag to check if user is logged in

        // Constructor that initializes the user database
        public Registration(UsersDB usersDB)
        {
            this.usersDB = usersDB;
        }

        // Method to get password from user input, masking the input with '*'
        public static string GetPassword()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.SetCursorPosition(x - 1, y);
                    Console.Write(" ");
                    Console.SetCursorPosition(x - 1, y);
                }
                else if( key.KeyChar < 32 || key.KeyChar > 126 )
                {
                    Trace.WriteLine("Output suppressed: no key char"); //catch non-printable chars, e.g F1, CursorUp and so ...
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    input.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            return input.ToString();
        }

        // Method to register a new user
        public void Register()
        {
            // Get username
            Console.WriteLine("Enter your username: ");
            string username = Console.ReadLine();
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty");
            }

            // Get password
            Console.WriteLine("Enter your password: ");
            string password = GetPassword();
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty");
            }
            if (password == username)
            {
                throw new ArgumentException("Password cannot be the same as username");
            }
            if (password.Length <= 6 )
            {
                throw new ArgumentException("Password must be longer than 6 characters");
            } 

            // Get email
            Console.WriteLine("Enter your email: ");
            string input = Console.ReadLine() ?? throw new ArgumentException("Email cannot be empty!");
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (!Regex.IsMatch(input, emailPattern)) 
            {
                throw new ArgumentException("Error: Invalid email!");
            }
            string email = input;

            // Add user to database
            usersDB.AddUser(username, password);
            isLogged = true;
        }

        // Method to log in a user
        public void Login()
        {
            // Get username
            Console.WriteLine("Enter your username: ");
            string inputUsername = Console.ReadLine() ?? throw new ArgumentException("Username cannot be empty!");
            if (!usersDB.ValidateUserUsername(inputUsername))
            {
                throw new ArgumentException("Invalid username!");
            }

            // Get password
            Console.WriteLine("Enter your password: ");
            string inputPassword = GetPassword() ?? throw new ArgumentException("Password cannot be empty!");
            if (usersDB.ValidateUser(inputUsername, inputPassword))
            {
                Console.WriteLine("You are logged in!");
                isLogged = true;
            }
            else
            {
                Console.WriteLine("Invalid username or password!");
            }
        }

        // Method to log out a user
        public void Logout()
        {
            isLogged = false;
        }

        // Method to check if a user is logged in
        public bool LogStatus() => isLogged;
    }
}