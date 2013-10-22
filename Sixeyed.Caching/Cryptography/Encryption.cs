using Sixeyed.Caching.Configuration;
using Sixeyed.Caching.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sixeyed.Caching.Cryptography
{
    /// <summary>
    /// Simple crypto using <see cref="RijndaelManaged"/> with configured key and IV
    /// </summary>
    public static class Encryption
    {
        private static RijndaelManaged _Provider;
        private static UTF8Encoding _Encoder;
        private static byte[] _Key;
        private static byte[] _Iv;

        static Encryption()
        {
            _Provider = new RijndaelManaged();
            _Encoder = new System.Text.UTF8Encoding();
            _Iv = _Encoder.GetBytes(CacheConfiguration.Current.Encryption.InitializationVector);
            _Key = _Encoder.GetBytes(CacheConfiguration.Current.Encryption.Key);
            if (_Iv.Length != 16 || _Key.Length != 32)
            {
                _Provider = null;
                Log.Warn("Configured encryption key and iv incorrect - must be 32 and 16 characters. *NOT ENCRYPTING*");
            }
        }

        /// <summary>
        /// Encrypts plain text and returns as base-64 encoded string
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            if (_Provider == null)
                return plainText;

            var inputBytes = _Encoder.GetBytes(plainText);
            using (var input = new MemoryStream(inputBytes))
            {
                using (var output = new MemoryStream())
                {
                    var encryptor = _Provider.CreateEncryptor(_Key, _Iv);
                    using (var cryptStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
                    {
                        var buffer = new byte[1024];
                        var read = input.Read(buffer, 0, buffer.Length);
                        while (read > 0)
                        {
                            cryptStream.Write(buffer, 0, read);
                            read = input.Read(buffer, 0, buffer.Length);
                        }
                        cryptStream.FlushFinalBlock();
                        var encryptedBytes = output.ToArray();
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts base-64 encoded encrypted string
        /// </summary>
        /// <param name="cipherBytes"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            if (_Provider == null)
                return cipherText;

            var cipherBytes = Convert.FromBase64String(cipherText);
            using (var input = new MemoryStream(cipherBytes))
            {
                using (var output = new MemoryStream())
                {
                    var decryptor = _Provider.CreateDecryptor(_Key, _Iv);
                    using (var cryptStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
                    {
                        var buffer = new byte[1024];
                        var read = cryptStream.Read(buffer, 0, buffer.Length);
                        while (read > 0)
                        {
                            output.Write(buffer, 0, read);
                            read = cryptStream.Read(buffer, 0, buffer.Length);
                        }
                        cryptStream.Flush();
                        return _Encoder.GetString(output.ToArray());
                    }
                }
            }
        }
    }
}
