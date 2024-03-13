using System;

class registration {
    private string username;
    private string password;
    private string email;
    private bool isLogged = false;
    static void registration(string username, string password, string email) {
        this.username = username;
        this.password = password;
        this.email = email;
        isLogged = true;
    }

    static void register() {
        Console.WriteLine("Enter your username: ");
        string username = Console.ReadLine();
        Console.WriteLine("Enter your password: ");
        string password = Console.ReadLine();
        Console.WriteLine("Enter your email: ");
        string email = Console.ReadLine();
        registration(username, password, email);
    }

    static void login() {
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

    static void logout() {
        isLogged = false;
    }

    static bool LogStatus() {
        return isLogged;
    }


    
}
// Path: logic/registration.cs