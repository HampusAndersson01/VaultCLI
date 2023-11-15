using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DotNetEnv;
public class PasswordService
{
    private List<PasswordModel> passwords = new List<PasswordModel>();
    private readonly string encryptionKey = string.Empty;

    public void AddPassword(PasswordModel password)
    {
        password.Password = Encrypt(password.Password);
        passwords.Add(password);
    }

    public void DisplayPasswords()
    {
        Console.Clear();
        Console.WriteLine("Password Manager - Display Passwords\n");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("1. Show All Passwords");
        Console.WriteLine("2. Search by Account or Alias");
        Console.WriteLine("3. Back to Main Menu");
        Console.ResetColor();

        var optionKey = Console.ReadKey(true).Key;

        switch (optionKey)
        {
            case ConsoleKey.D1:
                ShowAllPasswords();
                break;
            case ConsoleKey.D2:
                SearchPasswords();
                break;
            case ConsoleKey.D3:
                // Back to the main menu
                break;
        }
    }

    public void ShowAllPasswords()
    {
        Console.Clear();
        Console.WriteLine("All Passwords:\n");

        for (int i = 0; i < Count(); i++)
        {
            PrintPasswordDetails(GetPassword(i));
        }

        Console.WriteLine("\nPress any key to go back to the main menu.");
        Console.ReadKey(true);
    }

    public void SearchPasswords()
    {
        Console.Clear();
        Console.WriteLine("Search Passwords\n");

        Console.Write("Enter search term: ");
        var searchTerm = Console.ReadLine()?.ToLowerInvariant(); // Convert to lowercase for case-insensitive search

        Console.WriteLine("\nSearch Results:\n");

        for (int i = 0; i < Count(); i++)
        {
            var password = GetPassword(i);

            if (password.AccountName.ToLowerInvariant().Contains(searchTerm) || password.Alias.ToLowerInvariant().Contains(searchTerm))
            {
                PrintPasswordDetails(password);
            }
        }

        Console.WriteLine("\nPress any key to go back to the main menu.");
        Console.ReadKey(true);
    }

    public void PrintPasswordDetails(PasswordModel password)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Account: {password.AccountName}");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Password: {Decrypt(password.Password)}");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Alias: {password.Alias}\n");
        Console.ResetColor();
    }
    public PasswordModel GetPassword(int index)
    {
        return passwords[index];
    }

    public int Count()
    {
        return passwords.Count;
    }


    public PasswordService(string encryptionKey)
    {
        this.encryptionKey = encryptionKey;
    }

    public string Encrypt(string text)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = new byte[16]; // Initialization Vector

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    private string Decrypt(string cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey); // Fix the typo here
            aesAlg.IV = new byte[16]; // Use the same IV that was used for encryption

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }

    public void LoadPasswordsFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            try
            {
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        var passwordModel = new PasswordModel
                        {
                            AccountName = parts[0],
                            Password = parts[1],
                            Alias = parts[2]
                        };

                        passwords.Add(passwordModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading passwords: {ex.Message}");
            }
        }
    }

    public void SavePasswordsToFile(string filePath)
    {
        try
        {
            var lines = new List<string>();
            foreach (var password in passwords)
            {
                lines.Add($"{password.AccountName},{password.Password},{password.Alias}");
            }

            File.WriteAllLines(filePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving passwords: {ex.Message}");
        }
    }


    public string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
        var random = new Random();
        var password = new StringBuilder(12);

        for (int i = 0; i < 12; i++)
        {
            password.Append(chars[random.Next(chars.Length)]);
        }

        return password.ToString();
    }
}
