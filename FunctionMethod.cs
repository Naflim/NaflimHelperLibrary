using System.Globalization;
using System.Linq;
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
    }
}
