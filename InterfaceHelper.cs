using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NaflimHelperLibrary
{
    public class InterfaceHelper
    {
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
            return getReportsContent(request, bytes);
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
            return getReportsContent(request, bytes);
        }

        /// <summary>
        /// 请求接口
        /// </summary>
        /// <param name="http">http对象</param>
        /// <param name="bytes">body字节</param>
        /// <returns>回应内容</returns>
        private static string getReportsContent(HttpWebRequest http, byte[] bytes)
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
