using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace NaflimHelperLibrary
{
    public static class Log
    {
        private static readonly string DirectoryPath = System.AppDomain.CurrentDomain.BaseDirectory + "/日志";
        /// <summary>
        /// 日志路径
        /// </summary>
       // private static string path = Path.Combine(DirectoryPath, string.Format("{0}.txt",DateTime.Now.ToString("yyyyMMdd")));

        /// <summary>
        /// 记录已创建日志行数
        /// </summary>
        private static int LogLine = 0;

        /// <summary>
        /// 线程锁对象
        /// </summary>
        public static object obj = new object();

        /// <summary>
        /// 是否创建文本
        /// </summary>
        private static bool isCreateText = true;

        /// <summary>
        /// 保留日志行数
        /// </summary>
        private const int RETAIN_LINE = 500;

        static Log()
        {
        }


        private static void WriteLog(string msg, FileMode fileMode,string path)
        {
            try
            {
                msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + msg + "\r\n";

                Console.WriteLine(msg);

                if (!isCreateText) return;

                using (FileStream fileStream = new FileStream(path, fileMode, FileAccess.Write, FileShare.ReadWrite))
                {
                    //byte[] by = System.Text.Encoding.UTF8.GetBytes(msg + "\r\n");
                    byte[] by = System.Text.Encoding.Default.GetBytes(msg + "\r\n");
                    fileStream.Write(by, 0, by.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + "写入异常\t" + ex.Message);
            }
            finally
            {
                Debug.WriteLine(msg);
            }
        }

        private static int ReadLog(string path)
        {

            if (!isCreateText) return -1;


            int line = 0;

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (reader.ReadLine() != null)
                    {
                        line += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取异常\t" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + ex.Message);

            }

            return line;

        }


        public static void CreateLog(string msg)
        {
            new Thread(() => Write(msg))
            {
                IsBackground = true
            }.Start();
        }

        public static void CreateLog(string msg,string directoryPath)
        {
            new Thread(() => Write(msg,directoryPath))
            {
                IsBackground = true
            }.Start();
        }

        private static void Write(string msg,string directoryPath =null)
        {
            lock (obj)
            {
                string path = CreatePath(ref directoryPath);

                if (!File.Exists(path))
                {
                    if(!Directory.Exists(directoryPath))
                     Directory.CreateDirectory(directoryPath);

                    WriteLog(msg, FileMode.Create, path);
                    LogLine = 1;
                }
             
           /*     if (LogLine > RETAIN_LINE && ReadLog() > RETAIN_LINE)
                {
                    WriteLog(msg, FileMode.Create);
                    LogLine = 1;
                }
                else
                {*/
                    WriteLog(msg, FileMode.Append,path);
                    LogLine += 1;
                //}
                

            }
        }

        private static string CreatePath(ref string directoryPath)
        {
            directoryPath = Path.Combine(DirectoryPath, directoryPath??"");
            return Path.Combine(directoryPath, string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd")));
        }
    }
}
