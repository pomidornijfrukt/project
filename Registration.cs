using System;
using System.Text.RegularExpressions;

class Registration {
    private string? username;
    private string? password;
    private string? email;
    private bool isLogged = false;

    public void RegistrationMethod(string username, string password, string? email) {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email)) {
            throw new ArgumentException("Username or password or email cannot be empty");
        }
        this.username = username  ?? throw new ArgumentException("Username cannot be empty!");
        this.password = password  ?? throw new ArgumentException("Password cannot be empty!");
        this.email = email  ?? throw new ArgumentException("Email cannot be empty!");
        isLogged = true;
    }

    public void Register() {
        Console.WriteLine("Enter your username: ");
        string username = Console.ReadLine() ?? throw new ArgumentException("Username cannot be empty!");
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be empty");
        }

        Console.WriteLine("Enter your password: ");
        string password = Console.ReadLine() ?? throw new ArgumentException("Password cannot be empty!");
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be empty");
        }

        Console.WriteLine("Enter your email: ");
        string input = Console.ReadLine() ?? throw new ArgumentException("Email cannot be empty!");
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        if (!Regex.IsMatch(input, emailPattern)) {
            throw new ArgumentException("Error: Invalid email!");
        }
        string email = input;
        RegistrationMethod(username, password, email);
    }

    public void Login() {
        Console.WriteLine("Enter your username: ");
        string inputUsername = Console.ReadLine() ?? throw new ArgumentException("Username cannot be empty!");
        Console.WriteLine("Enter your password: ");
        string inputPassword = Console.ReadLine() ?? throw new ArgumentException("Password cannot be empty!");
        if (inputUsername == this.username && inputPassword == this.password) {
            Console.WriteLine("You are logged in!");
            isLogged = true;
        } else {
            Console.WriteLine("Invalid username or password!");
        }
    }

    public void Logout() {
        isLogged = false;
    }

    public bool LogStatus() => isLogged;
}