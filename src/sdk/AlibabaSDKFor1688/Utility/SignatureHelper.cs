/*======================================================================
    Daniel.Zhang
    
    文件名：SignatureHelper.cs
    文件功能描述：1688 API 签名 助手类

======================================================================*/
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AlibabaSDK.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class SignatureHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameters">请求参数，即queryString + request body 中的所有参数</param>
        /// <param name="signingKey">app密钥，与请求参数中client_id表示的appKey对应</param>
        /// <returns></returns>
        public static byte[] HmacSha1(string path, Dictionary<string, string> parameters, string signingKey)
        {
            var lists = new List<string>();
            foreach (var kvp in parameters)
            {
                lists.Add(kvp.Key + kvp.Value);
            }
            lists.Sort();
            var sb = new StringBuilder();
            sb.Append(path);
            foreach (String param in lists)
            {
                sb.Append(param);
            }
            var contentToHmac = sb.ToString();
            var byteToHmac = System.Text.Encoding.UTF8.GetBytes(contentToHmac);
            var byteArray = System.Text.Encoding.UTF8.GetBytes(signingKey);
            var hmac = new HMACSHA1(byteArray);
            var hashValue = hmac.ComputeHash(byteToHmac, 0, byteToHmac.Length);
            return hashValue;
        }

        /// <summary>
        /// 字节转换为十六进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c).ToUpper();
        }

        ///// <summary>
        ///// 十六进制字符串转换为字节
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //public static byte[] HexToBytes(string str)
        //{
        //    if (str.Length == 0 || str.Length % 2 != 0)
        //        return new byte[0];

        //    byte[] buffer = new byte[str.Length / 2];
        //    char c;
        //    for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
        //    {
        //        // Convert first half of byte
        //        c = str[sx];
        //        buffer[bx] = (byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

        //        // Convert second half of byte
        //        c = str[++sx];
        //        buffer[bx] |= (byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
        //    }

        //    return buffer;
        //}
    }
}
