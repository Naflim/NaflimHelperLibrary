using System.Text.RegularExpressions;

namespace NaflimHelperLibrary
{
    public class RegularJudgment
    {
        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="value">判断的字符串</param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 验证是否为ip地址
        /// </summary>
        /// <param name="value">判断的字符串</param>
        /// <returns></returns>
        public static bool IsIp(string value)
        {
            return Regex.IsMatch(value, @"^(([1-9]\d?)|(1\d{2})|(2[01]\d)|(22[0-3]))(\.((1?\d\d?)|(2[04]/d)|(25[0-5]))){3}$");
        }
    }
}
