using System.Security.Cryptography;
using System;
using System.Text;

namespace Hospital_Appointment_Booking_System.Helpers
{
    public class PasswordHasher
    {
        public static string EncryptPassword(string password)
        {
            byte[] salt;
            byte[] encryptedPassword;

            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
          
                salt = new byte[16];
                rngCryptoServiceProvider.GetBytes(salt);
            }

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                encryptedPassword = rfc2898DeriveBytes.GetBytes(20);
            }

            byte[] combinedBytes = new byte[salt.Length + encryptedPassword.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(encryptedPassword, 0, combinedBytes, salt.Length, encryptedPassword.Length);

            string encryptedPasswordBase64 = Convert.ToBase64String(combinedBytes);

            return encryptedPasswordBase64;
        }

        public static bool DecryptPassword(string encryptedPassword, string password)
        {
          
            byte[] combinedBytes = Convert.FromBase64String(encryptedPassword);

            byte[] salt = new byte[16];
            byte[] storedPassword = new byte[20];
            Array.Copy(combinedBytes, 0, salt, 0, salt.Length);
            Array.Copy(combinedBytes, salt.Length, storedPassword, 0, storedPassword.Length);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
               
                byte[] derivedPassword = rfc2898DeriveBytes.GetBytes(20);

                return storedPassword.AsSpan().SequenceEqual(derivedPassword);
            }
        }
    }
}
