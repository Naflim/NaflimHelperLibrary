using Newtonsoft.Json;

namespace NaflimHelperLibrary
{
    public static class TypeConversion
    {
        /// <summary>
        /// 对象转json
        /// </summary>
        /// <param name="obj">转换对象</param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// json转对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
