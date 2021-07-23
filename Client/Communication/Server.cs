using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Client.Communication
{
    public static class Server
    {
        /// <summary>
        /// The server's endpoint. Change this to your endpoint.
        /// </summary>
        private static string API = "http://194.233.71.142/ll/TellMe.php";

        public async static Task<bool> AsyncSendString(string logged, string username, string type)
        {
            bool result = false;
            await Task.Run(() =>
            {
                try
                {
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    form.Add(new StringContent(logged), type);

                    HttpClient httpClient = new HttpClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    var response = httpClient.PostAsync($"{API}?username=\"{HttpUtility.UrlEncode(username)}\"&type={type}", form).Result;
                    result = response.IsSuccessStatusCode;
                }
                catch { result = false; }
            });
            return result;
        }
        public async static Task<bool> AsyncUploadFile(byte[] file_buffer, string username, string type)
        {
            bool result = false;
            await Task.Run(() =>
            {
                try
                {
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    form.Add(new ByteArrayContent(file_buffer, 0, file_buffer.Length), "file", type);

                    HttpClient httpClient = new HttpClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(20);

                    var response = httpClient.PostAsync($"{API}?username=\"{HttpUtility.UrlEncode(username)}\"&type={type}", form).Result;
                    result = response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    result = false;
                }
            });
            return result;
        }
        public static JSON_Models.Update GetUpdate()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString($"{API}?type=update");
                    return JsonConvert.DeserializeObject<JSON_Models.Update>(s);
                }
            }
            catch
            {
                return null;
            }
        }
        public async static Task<string> AsyncDownloadFile(string url, string filename)
        {
            string successful_filename = null;
            await Task.Run(() =>
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, filename);
                    successful_filename = filename;
                }
            });
            return successful_filename;
        }
    }
}
