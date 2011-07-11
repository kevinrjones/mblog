using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MBlog.Infrastructure
{
    public static class CookieEncryption
    {
        private static readonly byte[] Salt = Encoding.Default.GetBytes(ConfigurationManager.AppSettings["salt"]);
        private static readonly Rfc2898DeriveBytes KeyGenerator = new Rfc2898DeriveBytes(ConfigurationManager.AppSettings["keyphrase"], Salt);
        private static byte[] _keyValue = null;
        private static byte[] _ivValue = null;

        private static byte[] Key
        {
            get
            {
                if (_keyValue == null)
                    _keyValue = KeyGenerator.GetBytes(16);
                return _keyValue;
            }
        }

        private static byte[] InitializationVector
        {
            get
            {
                if (_ivValue == null)
                    _ivValue = KeyGenerator.GetBytes(16);
                return _ivValue;
            }
        }

        public static byte[] Encrypt(this string plainText)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");

            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = InitializationVector;

                ICryptoTransform transform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                encrypted = Encrypt(plainText, transform);
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static string Decrypt(this byte[] cipherText)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length == 0)
                throw new ArgumentNullException("cipherText");

            string plaintext = null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = InitializationVector;

                ICryptoTransform transform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                plaintext = Decrypt(cipherText, transform);
            }
            return plaintext;
        }

        private static byte[] Encrypt(string plainText, ICryptoTransform transform)
        {
            byte[] encrypted;
            // Create the streams used for encryption.
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, transform, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                        swEncrypt.Flush();
                    }
                }
                encrypted = msEncrypt.ToArray();
            }
            return encrypted;
        }

        private static string Decrypt(byte[] cipherText, ICryptoTransform transform)
        {
            string plaintext;
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}