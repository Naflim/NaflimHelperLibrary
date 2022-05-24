﻿using System.Globalization;
using System.Linq;
using System.Net;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaflimHelperLibrary
{
    /// <summary>
    /// 功能方法
    /// </summary>
    public class FunctionMethod
    {
        /// <summary>
        /// 播报语音
        /// </summary>
        /// <param name="saying">播报语音</param>
        public static void Speaking(string saying)
        {
            string say = saying;
            Task.Run(() =>
            {
                SpeechSynthesizer speech = new SpeechSynthesizer
                {
                    Volume = 100 //音量
                };
                CultureInfo keyboardCulture = InputLanguage.CurrentInputLanguage.Culture;
                InstalledVoice neededVoice = speech.GetInstalledVoices(keyboardCulture).FirstOrDefault();
                if (neededVoice == null)
                    say = "未知的操作";
                else
                    speech.SelectVoice(neededVoice.VoiceInfo.Name);

                speech.Speak(say);
            });
        }

        /// <summary>
        /// 获取本机ip
        /// </summary>
        /// <returns>本机ip</returns>
        public static string GetIp()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
            var addressV = iPHostEntry.AddressList.FirstOrDefault(q => q.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);//ip4地址
            return addressV.ToString();
        }
    }
}
