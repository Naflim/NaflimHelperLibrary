using Microsoft.Win32;
using System.Collections.Generic;

namespace NaflimHelperLibrary
{
    /// <summary>
    /// 注册表帮助类
    /// </summary>
    public class RegistryHelper
    {
        readonly static RegistryKey hkcu = Registry.CurrentUser;
        private readonly string application;

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
        /// 注册表获取数据
        /// </summary>
        /// <param name="item">获取的项</param>
        /// <returns>注册表数据</returns>
        public string GetRegistData(string item)
        {
            RegistryKey software = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            string val = software.GetValue(item).ToString();
            software.Close();
            return val;
        }

        /// <summary>
        /// 注册表获取当前所有项
        /// </summary>
        /// <returns></returns>
        public string[] GetRegistItem()
        {
            RegistryKey software = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            string[] registItem = software.GetSubKeyNames();
            return registItem;
        }

        /// <summary>
        /// 注册表获取指定所有项
        /// </summary>
        /// <param name="url">项路径</param>
        /// <returns></returns>
        public static string[] GetRegistItem(string url)
        {
            RegistryKey software = hkcu.OpenSubKey(url, true);
            string[] registItem = software.GetSubKeyNames();
            return registItem;
        }

        /// <summary>
        /// 判断项是否存在
        /// </summary>
        /// <param name="sKeyName">项民</param>
        /// <param name="url">项地址</param>
        /// <returns>结果</returns>
        public static bool IsRegistryKeyExist(string sKeyName, string url)
        {
            string[] sKeyNameColl;
            RegistryKey hkSoftWare = hkcu.OpenSubKey(url);
            sKeyNameColl = hkSoftWare.GetSubKeyNames(); 
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
            sValueNameColl = software.GetValueNames(); 
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
        /// 判断值是否存在
        /// </summary>
        /// <param name="sValueName">值名</param>
        /// <returns>结果</returns>
        public bool IsRegistryValueNameExist(string[] sValueName)
        {
            bool flag = false;
            string[] sValueNameColl;
            RegistryKey software = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            sValueNameColl = software.GetValueNames(); 
            foreach (string name in sValueName)
            {
                flag = false;
                foreach (string sName in sValueNameColl)
                {
                    if (sName == name)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    return false;
            }
            software.Close();
            if (flag)
                return true;
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
        /// 设置注册表键值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public void SetRegistryValue(string key,string val)
        {
            RegistryKey hkSoftWare = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            hkSoftWare.SetValue(key, val, RegistryValueKind.String);


            hkSoftWare.Close();
        }

        /// <summary>
        /// 删除注册表键值
        /// </summary>
        /// <param name="valuePairs">删除的键</param>
        public void DelRegistryValue(string[] valuePairs)
        {
            RegistryKey hkSoftWare = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            foreach (string val in valuePairs)
                hkSoftWare.DeleteValue(val);

            hkSoftWare.Close();
        }

        /// <summary>
        /// 删除当前子项
        /// </summary>
        /// <param name="itemName">项名</param>
        public void DelRegistryItem(string itemName)
        {
            RegistryKey hkSoftWare = hkcu.OpenSubKey(@"SOFTWARE\NaflimPreject\" + application, true);
            hkSoftWare.DeleteSubKey(itemName);
            hkSoftWare.Close();
        }

        /// <summary>
        /// 关闭注册表链接
        /// </summary>
        public void RegistryClose()
        {
            hkcu.Close();
        }

        /// <summary>
        /// 开机自启
        /// </summary>
        /// <param name="path">程序路径</param>
        /// <param name="exeName">程序名</param>
        /// <param name="flag">是否自启</param>
        public static void SelfStarting(string exeName,bool flag)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string keyName = path.Substring(path.LastIndexOf("\\") + 1);
            RegistryKey Rkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (flag)
            {
                if (Rkey == null)
                    Rkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

                Rkey.SetValue(keyName, path + $"{exeName}.exe");
            }
            else
            {
                if (Rkey != null)
                    Rkey.DeleteValue(keyName, false);
            }
        }
    }
}
