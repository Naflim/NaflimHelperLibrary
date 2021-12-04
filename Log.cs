using System;
using System.Collections.Generic;
using System.IO;

namespace NaflimHelperLibrary
{
    public class Log
    {
        public string log { get; set; }
        public string input { get; set; }
        public string output { get; set; }

        string head;
        string foot;
        bool cusHead = false;
        bool cusFoot = false;

        public Log()
        {

        }

        public Log(string log)
        {
            this.log = log + "\r\n";
        }

        public Log(string input, string output)
        {
            this.input = input;
            this.output = output;
        }
        void LordLog()
        {
            log = (cusHead ? $"{head}\r\n" : $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n") + log;
            if (!string.IsNullOrEmpty(input))
                log += $"输入为：\r\n{input}\r\n";
            if (!string.IsNullOrEmpty(output))
                log += $"输出为：\r\n{output}\r\n";
            log += (cusFoot ? this.foot + "\r\n" : "\r\n");
        }

        void WriteLog(string log)
        {
            if (Directory.Exists($"log/{DateTime.Now:yyyy-MM-dd}") == false)
                Directory.CreateDirectory($"log/{DateTime.Now:yyyy-MM-dd}");

            File.AppendAllText($"log/{DateTime.Now:yyyy-MM-dd}/log.txt", log);
        }

        /// <summary>
        /// 将数据转换成日志字符串
        /// </summary>
        /// <typeparam name="T">范型数据类型</typeparam>
        /// <param name="t">数据</param>
        /// <returns>日志字符串</returns>
        public static string ConversionLog<T>(T t)
        {
            if (typeof(T) == typeof(Dictionary<string, string>))
                return DictionaryParsing(t as Dictionary<string, string>);
            if (typeof(T) == typeof(string[]))
                return string.Join(",", t as string[]);
            if (typeof(T) == typeof(List<string>))
                return string.Join(",", t as List<string>);
            else
                throw new LogExceptionModel("不支持此类型参数解析！");
        }

        /// <summary>
        /// 将数据转换成日志字符串(字符串型)
        /// </summary>
        /// <typeparam name="T">范型数据类型</typeparam>
        /// <param name="t">数据</param>
        /// <param name="separate">分割符号</param>
        /// <returns>日志字符串</returns>
        public static string ConversionLog<T>(T t,string separate)
        {
            if (typeof(T) == typeof(string[]))
                return string.Join(separate, t as string[]);
            if (typeof(T) == typeof(List<string>))
                return string.Join(separate, t as List<string>);
            else
                throw new LogExceptionModel("不支持此类型参数解析！");
        }

        /// <summary>
        /// 将字典类型转换为字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string DictionaryParsing(Dictionary<string, string> dic)
        {
            if (dic.Count <= 0)
                return null;

            string logVal = null;
            foreach (var item in dic)
                logVal += $"{item.Key}:{item.Value}\r\n";
            return logVal;
        }

        /// <summary>
        /// 自定义日志头部
        /// </summary>
        /// <param name="val">日志头部</param>
        public void SetHead(string val)
        {
            cusHead = true;
            head = val;
        }

        /// <summary>
        /// 自定义日志尾部
        /// </summary>
        /// <param name="val">日志尾部</param>
        public void SetFoot(string val)
        {
            cusFoot = true;
            foot = val;
        }

        /// <summary>
        /// 生成表格
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">表格数据</param>
        public void WriteTable(string fileName, Dictionary<string, string>[] data)
        {
            string tabTxt = null;
            if (Directory.Exists($"log/{DateTime.Now:yyyy-MM-dd}") == false)
                Directory.CreateDirectory($"log/{DateTime.Now:yyyy-MM-dd}");
            if (!File.Exists($"log/{DateTime.Now:yyyy-MM-dd}/{fileName}.md"))
            {
                tabTxt = $"# {fileName}\r\n";
                List<string> tabHead = new List<string>(data[0].Keys);
                tabHead.Insert(0, "时间");
                for (int i = 0; i < tabHead.Count; i++)
                {
                    tabTxt += "|" + tabHead[i];
                    if (i == tabHead.Count - 1)
                        tabTxt += "|\r\n";
                }
                for (int i = 0; i < tabHead.Count; i++)
                {
                    tabTxt += "|:---:";
                    if (i == tabHead.Count - 1)
                        tabTxt += "|\r\n";
                }
            }
            foreach (var item in data)
            {
                List<string> tabBody = new List<string>(item.Values);
                tabBody.Insert(0, DateTime.Now.ToString());
                for (int i = 0; i < tabBody.Count; i++)
                {
                    tabTxt += "|" + tabBody[i];
                    if (i == tabBody.Count - 1)
                        tabTxt += "|\r\n";
                }
            }
            File.AppendAllText($"log/{DateTime.Now:yyyy-MM-dd}/{fileName}.md", tabTxt);
            Console.WriteLine($"已打印:{fileName}.md");
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        public void PrintLog()
        {
            LordLog();
            WriteLog(log);
            Console.WriteLine(log);
        }

        /// <summary>
        /// 模板输出日志
        /// </summary>
        /// <param name="path"></param>
        public void PrintLog(string path)
        {
            string temp = File.ReadAllText(path);
            string tempLog = temp;
            if (!string.IsNullOrEmpty(log))
                tempLog = tempLog.Replace("[log]", log);
            if (!string.IsNullOrEmpty(input))
                tempLog = tempLog.Replace("[input]", input);
            if (!string.IsNullOrEmpty(output))
                tempLog = tempLog.Replace("[output]", output);
            WriteLog(tempLog);
            Console.WriteLine(tempLog);
        }

        /// <summary>
        /// 异常输出
        /// </summary>
        /// <param name="ex">发生的异常</param>
        public static void PrintError(Exception ex)
        {
            string errorInfo = $"发生异常：{DateTime.Now}\r\n异常信息：\r\n{ex.Message}\r\n详细信息：\r\n{ex.StackTrace}\r\n";
            Console.WriteLine(errorInfo);
            if (Directory.Exists($"log/{DateTime.Now:yyyy-MM-dd}") == false)
                Directory.CreateDirectory($"log/{DateTime.Now:yyyy-MM-dd}");

            File.AppendAllText($"log/{DateTime.Now:yyyy-MM-dd}/error.txt", errorInfo);
        }
    }


    internal class LogExceptionModel : Exception
    {
        internal LogExceptionModel(string message) : base(message)
        {

        }
    }
}
