using System;
using System.Text;

namespace NaflimHelperLibrary
{
    public class EncodingConversion
    {
        /// <summary>
        /// 字符串转uft-8
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string Get_utf8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }
    }
}
