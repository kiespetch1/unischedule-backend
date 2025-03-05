using System.Security.Cryptography;
using System.Text;

namespace UniSchedule.Identity.Database;

/// <summary>
///     Утилита для генерации паролей
/// </summary>
public class PasswordUtils
{
        /// <summary>
    ///     Хэширование пароля
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <param name="salt">Соль для пароля</param>
    public static string HashPassword(string password, string salt)
    {
        var keyBytes = Encoding.UTF8.GetBytes(salt);

        byte[] encryptedBytes;
        using (var chipherAlgorithm = Aes.Create())
        {
            chipherAlgorithm.BlockSize = 128;
            chipherAlgorithm.Key = keyBytes;
            chipherAlgorithm.Mode = CipherMode.CBC;
            chipherAlgorithm.IV = keyBytes;
            var encryptor = chipherAlgorithm.CreateEncryptor(chipherAlgorithm.Key, chipherAlgorithm.IV);
            using (var encryptStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(encryptStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(password);
                    }

                    encryptedBytes = encryptStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(encryptedBytes);
    }

    /// <summary>
    ///     Генерация последовательности для генерации пароля
    /// </summary>
    /// <param name="length">Длина последовательности. По умолчанию - 16</param>
    public static string GenerateSequence(int length = 16)
    {
        var random = new Random();
        var password = new StringBuilder();
        length = length > 0 ? length : random.Next(6, 9);
        for (var i = 0; i < length; ++i)
        {
            var probability = random.Next(0, 100);
            if (probability >= 0 && probability <= 33)
            {
                password.Append((char)random.Next(48, 58));
            }
            else if (probability >= 34 && probability <= 66)
            {
                password.Append((char)random.Next(65, 91));
            }
            else
            {
                password.Append((char)random.Next(97, 123));
            }
        }

        return password.ToString();
    }

}