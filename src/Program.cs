using System;
using System.Text;
using DotNetEnv;

class Program
{
    static string filePath = "passwords.txt"; // File to store passwords


    static PasswordService passwordService = new PasswordService(Env.GetString("ENCRYPTION_KEY"));

    static void Main()
    {
        // Load values from .env file
        Env.Load();
        // Load passwords from file
        passwordService.LoadPasswordsFromFile(filePath);

        Console.WriteLine("Password Manager");

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Display Passwords");
            Console.WriteLine("2. Add Password");
            Console.WriteLine("3. Exit");

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    passwordService.DisplayPasswords();
                    break;
                case ConsoleKey.D2:
                    AddPassword();
                    break;
                case ConsoleKey.D3:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    static void AddPassword()
    {


        Console.Clear();
        Console.WriteLine("Password Manager - Add Password\n");

        string accountName;
        do
        {
            Console.Write("Enter Account Name: ");
            accountName = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(accountName))
            {
                Console.WriteLine("Account Name is required. Please try again.");
            }
        } while (string.IsNullOrWhiteSpace(accountName));

        Console.Write("Enter Password (leave empty to generate a random password): ");
        var password = Console.ReadLine();

        // Generate a random password if left empty
        if (string.IsNullOrWhiteSpace(password))
        {
            password = passwordService.GenerateRandomPassword();
            Console.WriteLine($"Generated Password: {password}");
        }

        Console.Write("Enter Alias: ");
        var alias = Console.ReadLine();

        Console.Clear();
        Console.WriteLine("Confirm Entered Values:\n");
        Console.WriteLine($"Account Name: {accountName}");
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"Alias: {alias}\n");

        Console.WriteLine("1. Confirm");
        Console.WriteLine("2. Discard");

        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.D1:
                var passwordModel = new PasswordModel
                {
                    AccountName = accountName ?? "",
                    Password = password,
                    Alias = alias ?? ""
                };

                passwordService.AddPassword(passwordModel);
                passwordService.SavePasswordsToFile(filePath); // Save passwords to file after adding
                break;
            case ConsoleKey.D2:
                // Discard, do nothing
                break;
        }
    }


    
}
