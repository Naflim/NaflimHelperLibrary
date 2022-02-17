using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace NaflimHelperLibrary
{
    /// <summary>
    /// 数据转换
    /// </summary>
    public class DataConversion
    {
        /// <summary>
        /// 返回EPC数组
        /// </summary>
        /// <param name="bytes">标签数据</param>
        /// <returns>EPC数组</returns>
        public static string[] GetEPC(byte[] bytes)
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

                strarr[count] = ByteToHexStr(snaparr);
                count++;
                Grouping(strarr, byarr, index + byarr[index] + 2, count);
            }
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes">字符数组</param>
        /// <returns></returns>
        private static string ByteToHexStr(byte[] bytes)
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
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取json项的值
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <param name="key">项名</param>
        /// <returns>项值</returns>
        public static List<string> GetJsonValue(string jsonString, string key)
        {
            string pattern = $"\"{key}\":\"(.*?)\\\"";
            MatchCollection matches = Regex.Matches(jsonString, pattern, RegexOptions.IgnoreCase);
            List<string> lst = new List<string>();
            foreach (Match m in matches)
                lst.Add(m.Groups[1].Value);

            return lst;
        }

        /// <summary>
        /// 获取json项的值
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <param name="item">项名</param>
        /// <returns>项值</returns>
        public static string GetJsonItem(string json,string[] item)
        {

            JavaScriptSerializer Jss = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(json);

            foreach(string keyName in item)
            {
                if (!DicText.ContainsKey(keyName))
                    return null;
                else
                {
                    if (DicText[keyName] is string || DicText[keyName] is int)
                        return DicText[keyName].ToString();
                    else
                        DicText = DicText[keyName] as Dictionary<string, object>;
                }
            }
            return null;
        }

        /// <summary>
        /// BCC校验
        /// </summary>
        /// <param name="data">校验数据</param>
        /// <param name="temp">输出校验结果</param>
        public static int BCC(byte[] data)
        {
            int temp = 0;
            for (int index = 0; index < data.Length; index++)
            {
                temp ^= data[index];
            }
            return temp;
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString">数据字符串</param>
        /// <returns>16进制字节数组</returns>
        public static byte[] StrToHexByteArr(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
