using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Use PBKDF2 to hash the password
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 32));

        // Format: algorithm:iterations:salt:hash
        return $"pbkdf2_sha512:10000:{Convert.ToBase64String(salt)}:{hashed}";
    }

    public static bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        try
        {
            // Parse the stored hashed password to extract salt and hash
            var parts = hashedPassword.Split(':');
            if (parts.Length != 4)
            {
                return false; // Invalid format
            }

            byte[] salt = Convert.FromBase64String(parts[2]);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 32));

            // Compare the hashes
            return parts[3] == hashed;
        }
        catch
        {
            return false; // Error during verification
        }
    }
}
