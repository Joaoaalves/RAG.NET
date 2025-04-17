using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using RAGNET.Domain.Services.ApiKey;

namespace RAGNET.Application.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly byte[] _key;

        public CryptoService(IConfiguration configuration)
        {
            var keyString = configuration["Encryption:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new ArgumentException("Encryption key is not configured.");
            }

            _key = Encoding.UTF8.GetBytes(keyString);

            if (_key.Length != 32)
            {
                throw new ArgumentException("Encryption key must be 32 bytes long.");
            }
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Close();

            var encryptedBytes = ms.ToArray();
            var result = Convert.ToBase64String(iv.Concat(encryptedBytes).ToArray());
            return result;
        }

        public string Decrypt(string encryptedText)
        {
            var fullBytes = Convert.FromBase64String(encryptedText);
            var iv = fullBytes.Take(16).ToArray();
            var cipherBytes = fullBytes.Skip(16).ToArray();

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            var plaintext = sr.ReadToEnd();
            return plaintext;
        }
    }
}