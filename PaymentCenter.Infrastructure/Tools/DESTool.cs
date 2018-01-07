using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PaymentCenter.Infrastructure.Tools
{
    /// <summary>
    /// DES加解密工具
    /// </summary>
    public sealed class DESTool
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintextBuffer">明文</param>
        /// <returns></returns>
        public static string Encryption(string key, byte[] iv, byte[] plaintextBuffer)
        {
            ICryptoTransform transform = GetTripleDES(key, iv).CreateEncryptor();
            byte[] cipherTextBuffer = transform.TransformFinalBlock(plaintextBuffer, 0, plaintextBuffer.Length);
            transform.Dispose();
            return Convert.ToBase64String(cipherTextBuffer);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="key">密匙</param>
        /// <param name="iv">IV向量</param>
        /// <param name="cipherTextBuffer">密文</param>
        /// <returns></returns>
        public static string Decryption(string key, byte[] iv, byte[] cipherTextBuffer)
        {
            ICryptoTransform transform = GetTripleDES(key, iv).CreateDecryptor();
            byte[] decryption = transform.TransformFinalBlock(cipherTextBuffer, 0, cipherTextBuffer.Length);
            transform.Dispose();
            return Encoding.UTF8.GetString(decryption);
        }

        private static TripleDESCryptoServiceProvider GetTripleDES(string key, byte[] iv)
        {
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            //与java一致
            var key24 = new byte[24];
            Array.Copy(Encoding.UTF8.GetBytes(key), key24, 24);
            tripleDES.Key = key24;
            tripleDES.IV = iv;
            return tripleDES;
        }
    }
}
