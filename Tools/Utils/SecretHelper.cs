using System;
using System.Security.Cryptography;
using System.Text;

namespace Tools.Utils
{
    /// <summary>
    /// 加解密辅助类
    /// </summary>
    public class SecretHelper
    {
        #region Aes

        /// <summary>
        /// Aes加密
        /// CipherMode.ECB;PaddingMode.PKCS7;
        /// 密钥长度必须32位
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>加密后的字符串</returns>
        public static string AesEncrypt(string source, string key)
        {
            using (var aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = Encoding.UTF8.GetBytes(key);
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;//随机扰动
                using (var cryptoTransform = aesProvider.CreateEncryptor())
                {
                    var inputBuffers = Encoding.UTF8.GetBytes(source);
                    var results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    aesProvider.Dispose();
                    return System.Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        /// <summary>
        /// Aes解密
        /// 密钥长度必须32位
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>解密后的字符串</returns>
        public static string AesDecrypt(string source, string key)
        {
            using (var aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = Encoding.UTF8.GetBytes(key);
                aesProvider.Mode = CipherMode.ECB;
                aesProvider.Padding = PaddingMode.PKCS7;
                using (var cryptoTransform = aesProvider.CreateDecryptor())
                {
                    var inputBuffers = Convert.FromBase64String(source);
                    var results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                    aesProvider.Clear();
                    return Encoding.UTF8.GetString(results);
                }
            }
        }

        #endregion

        #region Md5

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            MD5 m = new MD5CryptoServiceProvider();
            byte[] s = m.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(s).Replace("-", "").ToUpper();
        }

        #endregion
    }
}
