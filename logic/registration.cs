using System;

class registration {
    private string username;
    private string password;
    private string email;
    private bool isLogged = false;
    public static void registration(string username, string password, string email) {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
            throw new ArgumentException("Username and password cannot be empty");
        }
        this.username = username;
        this.password = password;
        this.email = email;
        isLogged = true;
    }

    public static void register() {
        Console.WriteLine("Enter your username: ");
        string username = string.IsNullOrEmpty(Console.ReadLine()) ? throw new ArgumentException("Username cannot be empty") : username;

        Console.WriteLine("Enter your password: ");
        string password = string.IsNullOrEmpty(Console.ReadLine()) ? throw new ArgumentException("Password cannot be empty") : password;

        Console.WriteLine("Enter your email: ");
        string input = Console.ReadLine();
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
         if (!Regex.IsMatch(input, emailPattern)) {
            throw new ArgumentException("Error: Invalid email!");
            return;
        string email = input;
        registration(username, password, email);
    }

    public static void login() {
        Console.WriteLine("Enter your username: ");
        string username = Console.ReadLine();
        Console.WriteLine("Enter your password: ");
        string password = Console.ReadLine();
        if (username == this.username && password == this.password) {
            Console.WriteLine("You are logged in!");
        } else {
            Console.WriteLine("Invalid username or password!");
        }
    }

    public static void logout() {
        isLogged = false;
    }

    public static bool LogStatus() {
        return isLogged;
    }
    }


    
}
// Path: logic/registration.cs