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

        private static byte[] IV
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
            return Encrypt(plainText, Key, IV);
        }

        public static string Decrypt(this byte[] cipherText)
        {
            return Decrypt(cipherText, Key, IV);
        }

        private static byte[] Encrypt(this string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string Decrypt(this byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}