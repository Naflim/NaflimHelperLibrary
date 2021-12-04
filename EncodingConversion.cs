using System;
using System.Text;

namespace NaflimHelperLibrary
{
    public class EncodingConversion
    {
        Encoding utf;
        Encoding gbk;

        public EncodingConversion()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            utf = Encoding.GetEncoding("utf-8");
            gbk = Encoding.GetEncoding("gb2312");
        }

        /// <summary>
        /// gbk转utf-8
        /// </summary>
        /// <param name="txt">转换文本</param>
        /// <returns>转换后字符串</returns>
        public string GBKofUTF(string txt)
        {
            return TransferEncoding(gbk, utf, txt);
        }

        /// <summary>
        /// gbk转utf-8
        /// </summary>
        /// <param name="txt">转换文本</param>
        /// <returns>转换后字符串</returns>
        public byte[] GBKofUTFBys(string txt)
        {
            return TransferEncodingBys(gbk, utf, txt);
        }

        /// <summary>
        /// utf-8转gbk
        /// </summary>
        /// <param name="txt">转换文本</param>
        /// <returns>转换后字符串</returns>
        public string UTFofGBK(string txt)
        {
            return TransferEncoding(utf, gbk, txt);
        }

        /// <summary>
        /// utf-8转gbk
        /// </summary>
        /// <param name="txt">转换文本</param>
        /// <returns>转换后字节数组</returns>
        public byte[] UTFofGBKBys(string txt)
        {
            return TransferEncodingBys(utf, gbk, txt);
        }



        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="srcEncoding">原编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <param name="srcStr">原字符串</param>
        /// <returns>转换后字符串</returns>
        public static string TransferEncoding(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            byte[] bytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
            return dstEncoding.GetString(bytes);
        }

        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="srcEncoding">原编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <param name="srcStr">原字符串</param>
        /// <returns>转换后字节数组</returns>
        public static byte[] TransferEncodingBys(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            return Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
        }
    }
}
