using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NaflimHelperLibrary
{
    /// <summary>
    /// 接口请求帮助类
    /// </summary>
    public class InterfaceHelper
    {
        /// <summary>
        /// 拼接get请求字符串
        /// </summary>
        /// <param name="baseUrl">根路由</param>
        /// <param name="param">参数</param>
        /// <returns>完整路由</returns>
        public static string GetParameterUrl(string baseUrl, Dictionary<string, string> param)
        {
            StringBuilder builder = new();
            builder.Append(baseUrl);
            if (param.Count > 0)
            {
                builder.Append('?');
                int i = 0;
                foreach (var item in param)
                {
                    if (i > 0)
                        builder.Append('&');
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Get请求接口
        /// </summary>
        /// <param name="body">上传数据</param>
        /// <param name="url">上传地址</param>
        /// <returns>回调json字符串</returns>
        async public static Task<string> GetRequest(string url)
        {
            using HttpClient client = new();
            return await client.GetStringAsync(url);
        }

        /// <summary>
        /// Post请求接口
        /// </summary>
        /// <param name="body">上传数据</param>
        /// <param name="url">上传地址</param>
        /// <returns>回调json字符串</returns>
        async public static Task<string> PostRequest(object body, string url)
        {
            StringContent data = new(TypeConversion.ObjectToJson(body),Encoding.UTF8, "application/json");
            using HttpClient client = new();
            var response = await client.PostAsync(url, data);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Post请求接口
        /// </summary>
        /// <param name="header">请求头</param>
        /// <param name="body">上传数据</param>
        /// <param name="url">上传地址</param>
        /// <returns>回调json字符串</returns>
        async public static Task<string> PostRequest(Dictionary<string, string> header, object body, string url)
        {
            StringContent data = new(TypeConversion.ObjectToJson(body), Encoding.UTF8, "application/json");
            using HttpClient client = new();
            foreach (KeyValuePair<string, string> pair in header)
                client.DefaultRequestHeaders.Add(pair.Key, pair.Value);

            var response = await client.PostAsync(url, data);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 表单请求接口
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="headers">请求头</param>
        /// <param name="body">请求体</param>
        /// <returns>结果</returns>
        async public static Task<string> FormRequest(string url, Dictionary<string, string> headers, Dictionary<string, string> body)
        {
            using HttpClient client = new();
            MultipartFormDataContent postContent = new();
            postContent.Headers.Add("ContentType", $"multipart/form-data");
            foreach (var head in headers)
                postContent.Headers.Add(head.Key, head.Value);
            foreach (var content in body)
                postContent.Add(new StringContent(content.Value), content.Key);
            var response = await client.PostAsync(url, postContent).ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
