using System.Text.RegularExpressions;

namespace NaflimHelperLibrary
{
    /// <summary>
    /// 正则判断
    /// </summary>
    public class RegularJudgment
    {
        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="value">判断的字符串</param>
        /// <returns></returns>
        public static bool IsNum(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 验证是否为ip地址
        /// </summary>
        /// <param name="value">判断的字符串</param>
        /// <returns></returns>
        public static bool IsIp(string value)
        {
            return Regex.IsMatch(value, @"((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))");
        }

        /// <summary>
        /// 判断字符串是否为EPC
        /// </summary>
        /// <param name="value">判断的字符串</param>
        /// <returns></returns>
        public static bool IsEPC(string value)
        {
            return value.Length % 4 == 0 && Regex.IsMatch(value, "^[0-9A-Fa-f]+$");
        }
    }
}
