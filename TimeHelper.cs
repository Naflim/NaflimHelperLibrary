using System;

namespace NaflimHelperLibrary
{
    class TimeHelper
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns>时间戳</returns>
        public static string GetTimestamp()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }

        /// <summary>
        ///  获取当前毫秒时间戳
        /// </summary>
        /// <returns>毫秒时间戳</returns>
        public static string GetMillTimestamp()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
        }
    }
}
