using System.Security.Cryptography;
using System.Text;

namespace NaflimHelperLibrary
{
    public class DataConversion
    {
        /// <summary>
        /// 返回EPC数组
        /// </summary>
        /// <param name="bytes">标签数据</param>
        /// <returns>EPC数组</returns>
        public static string[] getEPC(byte[] bytes)
        {
            string[] strarr = new string[1000];
            Grouping(strarr, bytes, 0, 0);
            return strarr;
        }

        /// <summary>
        /// 递归将byte数组分割为16进制EPC字符串数组
        /// </summary>
        /// <param name="strarr">EPC字符串数组</param>
        /// <param name="byarr">byte数组</param>
        /// <param name="index">当前递归的bype数组索引</param>
        /// <param name="count">当前递归的EPC字符串数组索引</param>
        public static void Grouping(string[] strarr, byte[] byarr, int index, int count)
        {
            byte[] snaparr = new byte[byarr[index]];
            if (byarr[index] != 0 && snaparr.Length >= byarr[index])
            {
                for (int i = index + 1, j = 0; i <= index + byarr[index]; i++, j++)
                    snaparr[j] = byarr[i];

                strarr[count] = byteToHexStr(snaparr);
                count++;
                Grouping(strarr, byarr, index + byarr[index] + 2, count);
            }
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes">字符数组</param>
        /// <returns></returns>
        private static string byteToHexStr(byte[] bytes)
        {
            string returnStr = null;
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt">要加密字符串</param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                    sb.Append(newBuffer[i].ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
