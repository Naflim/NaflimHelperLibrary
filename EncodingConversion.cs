using System;
using System.Text;

namespace NaflimHelperLibrary
{
    public class EncodingConversion
    {
        readonly Encoding utf;
        readonly Encoding gbk;

        /// <summary>
        /// 涉及到额外字符集都需要初始化构造函数
        /// </summary>
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
        /// 字符串转ASCII码
        /// </summary>
        /// <param name="txt">转换文本</param>
        /// <returns>转换后字符串</returns>
        public static string StrOfASCII(string txt)
        {
            var ascArr = new ASCIIEncoding().GetBytes(txt);
            return DataConversion.ByteToHexStr(ascArr);
        }

        /// <summary>
        /// 字符串转ASCII字节数组
        /// </summary>
        /// <param name="txt">转换文本</param>
        /// <returns>转换后字节数组</returns>
        public static byte[] StrOfASCIIBys(string txt)
        {
            return new ASCIIEncoding().GetBytes(txt);
        }

        /// <summary>
        /// ASCII解码
        /// </summary>
        /// <param name="txt">ASCII码</param>
        /// <returns>解码后字符串</returns>
        public static string ASCIIOfStr(string txt)
        {
            var ascArr = DataConversion.StrToHexByteArr(txt);
            return new ASCIIEncoding().GetString(ascArr);
        }

        /// <summary>
        /// 字符串gbk转16进制
        /// </summary>
        /// <param name="txt">文字</param>
        /// <returns>16进制字符串</returns>
        public string StrOfHex(string txt)
        {
            var gbkbys = UTFofGBKBys(txt);
            return DataConversion.ByteToHexStr(gbkbys);
        }

        /// <summary>
        /// 16进制gbk解码字符串
        /// </summary>
        /// <param name="hex">16进制字符串</param>
        /// <returns>文字</returns>
        public string HexOfStr(string hex)
        {
            var hexbys = DataConversion.StrToHexByteArr(hex);
            return gbk.GetString(hexbys);
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
