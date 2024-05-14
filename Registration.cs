using System;
using System.Text.RegularExpressions;
namespace Project
{
    class Registration
    {
        private UsersDB usersDB; // Added this line
        public bool isLogged = false;

        public Registration(UsersDB usersDB) // Added this line
        {
            this.usersDB = usersDB; // Added this line
        }

        public void Register()
        {
            Console.WriteLine("Enter your username: ");
            string username = Console.ReadLine();
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty");
            }

            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();
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

            Console.WriteLine("Enter your email: ");
            string input = Console.ReadLine() ?? throw new ArgumentException("Email cannot be empty!");
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (!Regex.IsMatch(input, emailPattern)) 
            {
                throw new ArgumentException("Error: Invalid email!");
            }
            string email = input;

            usersDB.AddUser(username, password);
            isLogged = true;
        }

        public void Login()
        {
            Console.WriteLine("Enter your username: ");
            string inputUsername = Console.ReadLine() ?? throw new ArgumentException("Username cannot be empty!");
            if (!usersDB.ValidateUserUsername(inputUsername))
            {
                throw new ArgumentException("Invalid username!");
            }
            Console.WriteLine("Enter your password: ");
            string inputPassword = Console.ReadLine() ?? throw new ArgumentException("Password cannot be empty!");
            if (usersDB.ValidateUser(inputUsername, inputPassword))
            {
                Console.WriteLine("You are logged in!");
                isLogged = true;
                // usersDB.SetActiveUser(newUser);
            }
            else
            {
                Console.WriteLine("Invalid username or password!");
            }

        }

        public void Logout()
        {
            isLogged = false;
        }

        public bool LogStatus() => isLogged;
    }
}