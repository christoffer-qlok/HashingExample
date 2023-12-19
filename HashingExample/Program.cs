using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        // Generate a random salt
        byte[] salt = GenerateSalt();

        // Läs in ett lösenord från användaren
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Skapa en hash baserat på lösenordet och en salt
        string hashedPassword = HashPassword(password, salt);
        string base64Salt = Convert.ToBase64String(salt);

        // Skriv ut hash och salt. I verkligheten skulle det här spara i t.ex. en databas
        Console.WriteLine($"Salt as bytes: {string.Join(',',salt)}");
        Console.WriteLine($"Salt as base64: {base64Salt}");
        Console.WriteLine($"Hashed Password: {hashedPassword}");

        // Läs in ett nytt lösenord och validera det mot vår hash
        Console.Write("Write your password again:");
        password = Console.ReadLine();
        if (ValidatePassword(password, hashedPassword, base64Salt))
        {
            Console.WriteLine("Correct password!");
        }
        else
        {
            Console.WriteLine("Wrong password :(");
        }
    }

    static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        // Använd kryptografiskt säker slumptalgenerator
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    static string HashPassword(string password, byte[] salt)
    {
        int iterations = 10000; // Antal iterationer hashen kör, högre betyder att det tar längre att skapa den

        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
        {
            byte[] hash = pbkdf2.GetBytes(32);

            // Returnera våra hash bytes kodade i base64
            return Convert.ToBase64String(hash);
        }
    }

    static bool ValidatePassword(string password, string storedHash, string storedSalt)
    {
        // Packa upp våra salt bytes från base64 strängen
        byte[] salt = Convert.FromBase64String(storedSalt);
        string hashedPasswordToCheck = HashPassword(password, salt);

        return hashedPasswordToCheck == storedHash;
    }
}
