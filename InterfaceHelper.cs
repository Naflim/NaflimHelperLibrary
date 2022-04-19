using System.Collections.Generic;
using System.IO;
using System.Net;
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
            StringBuilder builder = new StringBuilder();
            builder.Append(baseUrl);
            if (param.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in param)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 请求接口并进行数据处理
        /// </summary>
        /// <param name="body">上传数据</param>
        /// <param name="url">上传地址</param>
        /// <returns>回调json字符串</returns>
        public static string GetRequestInterface(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //获取请求的方式
            request.Method = "get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream getStream = response.GetResponseStream();
            StreamReader streamreader = new StreamReader(getStream);
            return streamreader.ReadToEnd();
        }

        /// <summary>
        /// 请求接口并进行数据处理
        /// </summary>
        /// <param name="body">上传数据</param>
        /// <param name="url">上传地址</param>
        /// <returns>回调json字符串</returns>
        public static string RequestInterface(object body, string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //获取请求的方式
            request.Method = "post";
            request.ContentType = "application/json";
            var param = TypeConversion.ObjectToJson(body);
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            return GetReportsContent(request, bytes);
        }

        /// <summary>
        /// 请求接口并进行数据处理
        /// </summary>
        /// <param name="header">请求头</param>
        /// <param name="body">上传数据</param>
        /// <param name="url">上传地址</param>
        /// <returns>回调json字符串</returns>
        public static string RequestInterface(Dictionary<string, string> header, object body, string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //获取请求的方式
            request.Method = "post";
            request.ContentType = "application/json";
            foreach (KeyValuePair<string, string> pair in header)
                request.Headers.Add(pair.Key, pair.Value);
            var param = TypeConversion.ObjectToJson(body);
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            return GetReportsContent(request, bytes);
        }

        /// <summary>
        /// 请求接口
        /// </summary>
        /// <param name="http">http对象</param>
        /// <param name="bytes">body字节</param>
        /// <returns>回应内容</returns>
        private static string GetReportsContent(HttpWebRequest http, byte[] bytes)
        {
            http.ContentLength = bytes.Length;
            Stream streamPager = http.GetRequestStream();
            streamPager.Write(bytes, 0, bytes.Length);
            streamPager.Close();
            //获取设置身份认证及请求超时时间
            SetWebRequest(http);
            //就收应答
            HttpWebResponse httpResponse = (HttpWebResponse)http.GetResponse();
            Stream streamRespond = httpResponse.GetResponseStream();
            string responseContent = new StreamReader(streamRespond, Encoding.UTF8).ReadToEnd();
            streamRespond.Close();
            return responseContent;
        }

        /// <summary>
        /// 上传等待最长时间
        /// </summary>
        /// <param name="request"></param>
        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 30000;
        }

        async public static Task<string> FormInterface(string url, Dictionary<string, string> headers, Dictionary<string, string> body)
        {
            string res = null;
            HttpClient _httpClient = new HttpClient();
            var postContent = new MultipartFormDataContent();
            postContent.Headers.Add("ContentType", $"multipart/form-data");
            foreach (var head in headers)
                postContent.Headers.Add(head.Key, head.Value);
            foreach (var content in body)
                postContent.Add(new StringContent(content.Value), content.Key);
            var response = await _httpClient.PostAsync(url, postContent).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                res = response.Content.ReadAsStringAsync().Result;
            return res;
        }

       
    }
}
