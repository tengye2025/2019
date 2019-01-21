using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace WindowsFormsApplication1.Screat
{
    class ClassAES
    {


        #region AES加解密
        /// <summary>
        ///AES加密（加密步骤）
        ///1，加密字符串得到2进制数组；
        ///2，将2禁止数组转为16进制；
        ///3，进行base64编码
        /// </summary>
        /// <param name="toEncrypt">要加密的字符串</param>
        /// <param name="key">密钥</param>
        public static String Encrypt(String toEncrypt, String key)
        {
            Byte[] _Key = Encoding.ASCII.GetBytes(key);
            Byte[] _Source = Encoding.UTF8.GetBytes(toEncrypt);
            Aes aes = Aes.Create("AES");
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _Key;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            Byte[] cryptData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
            String HexCryptString = Hex_2To16(cryptData);
            Byte[] HexCryptData = Encoding.UTF8.GetBytes(HexCryptString);
            String CryptString = Convert.ToBase64String(HexCryptData);
            return CryptString;
        }

        /// <summary>
        /// AES解密（解密步骤）
        /// 1，将BASE64字符串转为16进制数组
        /// 2，将16进制数组转为字符串
        /// 3，将字符串转为2进制数据
        /// 4，用AES解密数据
        /// </summary>
        /// <param name="encryptedSource">已加密的内容</param>
        /// <param name="key">密钥</param>
        public static String Decrypt(string encryptedSource, string key)
        {
            Byte[] _Key = Encoding.ASCII.GetBytes(key);
            Aes aes = Aes.Create("AES");
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _Key;
            ICryptoTransform cTransform = aes.CreateDecryptor();

            Byte[] encryptedData = Convert.FromBase64String(encryptedSource);
            String encryptedString = Encoding.UTF8.GetString(encryptedData);
            Byte[] _Source = Hex_16To2(encryptedString);
            Byte[] originalSrouceData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
            String originalString = Encoding.UTF8.GetString(originalSrouceData);
            return originalString;
        }

        /// <summary>
        /// 2进制转16进制
        /// </summary>
       private static String Hex_2To16(Byte[] bytes)
        {
            String hexString = String.Empty;
            Int32 iLength = 65535;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                if (bytes.Length < iLength)
                {
                    iLength = bytes.Length;
                }

                for (int i = 0; i < iLength; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 16进制转2进制
        /// </summary>
       private static Byte[] Hex_16To2(String hexString)
        {
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }
            Byte[] returnBytes = new Byte[hexString.Length / 2];
            for (Int32 i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }


        #endregion



    }
}
