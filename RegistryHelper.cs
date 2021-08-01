using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaflimHelperLibrary
{
    public class RegistryHelper
    {
        readonly RegistryKey hkcu = Registry.CurrentUser;
        private string application;

        public RegistryHelper(string app)
        {
            application = app;
        }
        /// <summary>
        /// 注册表获取数据
        /// </summary>
        /// <param name="item">获取的项</param>
        /// <returns>注册表数据</returns>
        public string[] GetRegistData(string[] item)
        {
            string[] registData = new string[item.Length];
            RegistryKey software = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            for(int i = 0; i < item.Length; i++)
                registData[i] = software.GetValue(item[i]).ToString();
            software.Close();
            return registData;
        }

        /// <summary>
        /// 判断项是否存在
        /// </summary>
        /// <param name="sKeyName">项民</param>
        /// <param name="url">项地址</param>
        /// <returns>结果</returns>
        public bool IsRegistryKeyExist(string sKeyName, string url)
        {
            string[] sKeyNameColl;
            RegistryKey hkSoftWare = hkcu.OpenSubKey(url);
            sKeyNameColl = hkSoftWare.GetSubKeyNames(); //获取SOFTWARE下所有的子项
            foreach (string sName in sKeyNameColl)
            {
                if (sName == sKeyName)
                {
                    hkSoftWare.Close();
                    return true;
                }
            }
            hkSoftWare.Close();
            return false;
        }

        /// <summary>
        /// 判断值是否存在
        /// </summary>
        /// <param name="sValueName">值名</param>
        /// <returns>结果</returns>
        public bool IsRegistryValueNameExist(string sValueName)
        {
            string[] sValueNameColl;
            RegistryKey software = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            sValueNameColl = software.GetValueNames(); //获取test下所有键值的名称
            foreach (string sName in sValueNameColl)
            {
                if (sName == sValueName)
                {
                    software.Close();
                    return true;
                }
            }
            software.Close();
            return false;
        }

        /// <summary>
        /// 创建注册表项
        /// </summary>
        public void CreateRegistryKey()
        {
            RegistryKey hkSoftWare = hkcu.CreateSubKey(@"SOFTWARE\NaflimPreject\" + application);
            hkSoftWare.Close();
        }
        /// <summary>
        /// 设置注册表键值
        /// </summary>
        /// <param name="valuePairs">设置的键值</param>
        public void SetRegistryValue(Dictionary<string,string> valuePairs)
        {
            RegistryKey hkSoftWare = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            foreach (KeyValuePair<string, string> kvp in valuePairs)
                hkSoftWare.SetValue(kvp.Key, kvp.Value, RegistryValueKind.String);

            hkSoftWare.Close();
        }
        /// <summary>
        /// 关闭注册表链接
        /// </summary>
        public void RegistryClose()
        {
            hkcu.Close();
        }
    }
}
