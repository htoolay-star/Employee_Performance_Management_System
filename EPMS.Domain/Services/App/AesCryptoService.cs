using EPMS.Domain.Interface.IService.App;
using EPMS.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Domain.Services.App
{
    public class AesCryptoService : ICryptoService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public AesCryptoService(IOptions<CryptoSettings> options)
        {
            var settings = options.Value;

            if (string.IsNullOrEmpty(settings.Key) || string.IsNullOrEmpty(settings.IV))
                throw new ArgumentNullException("Encryption Key or IV is missing in settings!");

            _key = Encoding.UTF8.GetBytes(settings.Key.PadRight(32).Substring(0, 32));
            _iv = Encoding.UTF8.GetBytes(settings.IV.PadRight(16).Substring(0, 16));
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return string.Empty;

            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
