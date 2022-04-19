using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace NaflimHelperLibrary
{
    /// <summary>
    /// 日志
    /// </summary>
    public class Log
    {
        string log;
        string input;
        string output;

        public string FileName { get; set; }

        string head;
        string foot;

        public Log()
        {
            FileName = $"log.txt";
        }

        public Log(string fileName)
        {
            FileName = $"{fileName}.txt";
        }

        void LordLog()
        {
            log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}{(string.IsNullOrEmpty(head) ? null : head + Environment.NewLine)}{log}";
            if (!string.IsNullOrEmpty(input))
                log += $"输入为：{Environment.NewLine}{input}{Environment.NewLine}";
            if (!string.IsNullOrEmpty(output))
                log += $"输出为：{Environment.NewLine}{output}{Environment.NewLine}";
            log += $"{foot}{Environment.NewLine}";
        }

        void WriteLog(string log)
        {
            try
            {
                if (Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/log/{DateTime.Now:yyyy-MM-dd}") == false)
                    Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}/log/{DateTime.Now:yyyy-MM-dd}");

                File.AppendAllText($"{AppDomain.CurrentDomain.BaseDirectory}/log/{DateTime.Now:yyyy-MM-dd}/{FileName}", log);
            }
            catch (IOException)
            {
                WriteLog(log);
            }
            catch (Exception)
            {
                throw;
            }
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
        public static string ConversionLog<T>(T t, string separate)
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
                logVal += $"{item.Key}:{item.Value}{Environment.NewLine}";
            return logVal;
        }

        /// <summary>
        /// 自定义日志头部
        /// </summary>
        /// <param name="val">日志头部</param>
        public Log SetHead(string val)
        {
            head = val;
            return this;
        }

        /// <summary>
        /// 自定义日志尾部
        /// </summary>
        /// <param name="val">日志尾部</param>
        public Log SetFoot(string val)
        {
            foot = val;
            return this;
        }

        /// <summary>
        /// 生成表格
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">表格数据</param>
        public static void WriteTable(string fileName, DataTable data)
        {
            string tabTxt = null;
            List<string> tabHead = new List<string>();
            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/log/{DateTime.Now:yyyy-MM-dd}";
            string file = $"{filePath}/{fileName}.md";

            if (Directory.Exists(filePath) == false)
                Directory.CreateDirectory(filePath);

            if (!File.Exists(file))
            {
                foreach (DataColumn col in data.Columns)
                    tabHead.Add(col.ColumnName);

                tabTxt = $"# {fileName}{Environment.NewLine}";

                tabTxt += "|记录时间";
                for (int i = 0; i < tabHead.Count; i++)
                    tabTxt += $"|{tabHead[i]}";
                tabTxt += $"|{Environment.NewLine}";

                for (int i = 0; i < tabHead.Count + 1; i++)
                    tabTxt += "|:---:";
                tabTxt += $"|{Environment.NewLine}";
            }
            else
            {
                string head = null;
                StreamReader sr = new StreamReader(file);

                for (int i = 0; i < 2; i++)
                    head = sr.ReadLine();  //将值插入

                sr.Close();

                tabHead = head.Split('|').ToList();
                tabHead.RemoveAll(v => v == "");
                tabHead.RemoveAt(0);
            }

            int len = data.Rows.Count;
            for (int i = 0; i < len; i++)
            {
                tabTxt += $"|{DateTime.Now}";
                tabHead.ForEach(v => tabTxt += $"|{data.Rows[i][v]}");
                tabTxt += $"|{Environment.NewLine}";
            }
            File.AppendAllText(file, tabTxt);
            Console.WriteLine($"已打印:{fileName}.md");
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="log">日志内容</param>
        public void PrintLog(string log)
        {
            this.log = log;
            LordLog();
            WriteLog(this.log);
            Console.WriteLine(this.log);
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="output">输出</param>
        public void PrintLog(string input, string output)
        {
            this.input = input;
            this.output = output;
            LordLog();
            WriteLog(log);
            Console.WriteLine(log);
        }

        /// <summary>
        /// 模板输出日志
        /// </summary>
        /// <param name="path"></param>
        public void TemplateLog(string path, string log = "", string input = "", string output = "")
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
            try
            {
                string errorInfo = $"发生异常{ex.GetType()}：{DateTime.Now}{Environment.NewLine}异常信息：{Environment.NewLine}{ex.Message}{Environment.NewLine}详细信息：{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
                Console.WriteLine(errorInfo);
                if (Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/log/{DateTime.Now:yyyy-MM-dd}") == false)
                    Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}/log/{DateTime.Now:yyyy-MM-dd}");

                File.AppendAllText($"{AppDomain.CurrentDomain.BaseDirectory}/log/{DateTime.Now:yyyy-MM-dd}/error.txt", errorInfo);
            }
            catch (IOException)
            {
                PrintError(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }


    internal class LogExceptionModel : Exception
    {
        internal LogExceptionModel(string message) : base(message)
        {

        }
    }
}
