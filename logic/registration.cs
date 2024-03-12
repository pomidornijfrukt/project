using System;

class registration {
    private string username;
    private string password;
    private string email;
    static void registration(string username, string password, string email) {
        this.username = username;
        this.password = password;
        this.email = email;
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

    
}
// Path: logic/registration.cs