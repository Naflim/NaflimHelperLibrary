using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NaflimHelperLibrary
{
    public class XMLHelper
    {
        /// <summary>
        /// xml转实体类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strXML">xml文本</param>
        /// <returns></returns>
        public static T DESerializer<T>(string strXML) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception)
            {
                new Log(strXML).PrintLog();
                throw;
            }
        }

        /// <summary>
        /// xml转实体类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="strXML">xml文本</param>
        /// <returns></returns>
        public static T DESerializer<T>(XmlNode[] xmlArr) where T : class
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration xmldecl = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            document.AppendChild(xmldecl);
            XmlElement root = document.CreateElement(typeof(T).Name);//创建根节点
            document.AppendChild(root);
            foreach (XmlNode xmlNode in xmlArr)
                document.DocumentElement.AppendChild(document.ImportNode(xmlNode, true));

            return DESerializer<T>(document.OuterXml);
        }

        /// <summary>
        /// 实体类转xml
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">实体类</param>
        /// <returns>xml文档</returns>
        public static string XmlSerialize<T>(T obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
        }

        /// <summary>
        /// 返回指定节点值
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <param name="nodes">节点列表</param>
        /// <returns></returns>
        public static string GetElementVal(string xml,string[] nodes)
        {
            StringReader stream = new StringReader(xml);//读取字符串为数据量
            XElement xe1 = XElement.Load(stream);
            string val = null;

            for(int i = 0; i < nodes.Length; i++)
            {
                if(i == nodes.Length -1)
                    val = xe1.Element(nodes[i]).Value;
                else
                {
                    var Result = xe1.Descendants(nodes[i]);
                    xe1 = Result.SingleOrDefault();
                }
            }
            return val;
        }
    }

    /// <summary>
    /// 实体类转xml
    /// </summary>
    public static class ObjectToXML
    {
        private static readonly Type[] WriteTypes = new[] {
        typeof(string), typeof(DateTime),typeof(decimal), typeof(Guid),
        };
        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || WriteTypes.Contains(type) || type.IsEnum;
        }
        public static XElement ToXml(this object input)
        {
            return input.ToXml(null);
        }

        private static string GetXMLElementAttributeName(PropertyInfo info)
        {
            string attributeName = "";

            var attr = info.GetCustomAttributes(true);
            if (attr != null && attr.Length > 0)
            {

                foreach (var item in attr)
                {
                    if (item is XmlElementAttribute)
                    {
                        var temp = item as XmlElementAttribute;
                        attributeName = temp.ElementName;


                    }
                    else if (item is XmlRootAttribute)
                    {
                        var temp = item as XmlRootAttribute;
                        attributeName = temp.ElementName;
                    }
                }
            }
            return attributeName;
        }

        private static object GetPropertyValue(object input, PropertyInfo info)
        {
            if (info.PropertyType.IsEnum)
            {
                return (int)info.GetValue(input);
            }
            return info.GetValue(input);
        }

        public static XElement ToXml(this object input, string element)
        {
            if (input == null)
                return null;

            if (string.IsNullOrEmpty(element))
                element = "object";

            element = XmlConvert.EncodeName(element);
            var ret = new XElement(element);

            if (input != null)
            {
                var type = input.GetType();
                var props = type.GetProperties();
                var elements = from prop in props
                               let name = XmlConvert.EncodeName(GetXMLElementAttributeName(prop) == "" ? prop.Name : GetXMLElementAttributeName(prop))
                               let val = GetPropertyValue(input, prop)
                               let value = prop.PropertyType.IsSimpleType()
                                    ? new XElement(name, val)
                                    : val.ToXml(name)
                               where value != null
                               select value;

                ret.Add(elements);
            }

            return ret;
        }
    }
}
