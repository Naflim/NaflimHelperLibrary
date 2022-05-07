/*************************************************************************************
 *
 * 文 件 名:   ExtensionMethod
 * 描    述:   扩展方法类
 * 
 * 版    本：  V1.0
 * 创 建 者：  Naflim 
 * 创建时间：  2022/5/7 15:50:03
 * ======================================================
 * 历史更新记录
 * 版本： V          修改时间：         修改人：
 * 修改内容：
 * ======================================================
*************************************************************************************/

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NaflimHelperLibrary
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// 深度克隆，字典和List可以直接.Clone，如果是类的话那个类必须得打上[Serializable]标签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T Clone<T>(this T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
    }
}
