using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication
{
    public static class Server
    {
        /// <summary>
        /// The server's endpoint.
        /// </summary>
        private static string API = "http://194.233.71.142/ll";

        public async static Task<bool> AsyncSendPost(string endpoint, string data)
        {
            bool result = false;
            await Task.Run(() => {
                result = SendRequest($"{API}/{endpoint}", data, "POST");
            });
            return result;
        }
        public async static Task<bool> AsyncSendGet(string endpoint, string data)
        {
            bool result = false;
            await Task.Run(() => {
                result = SendRequest($"{API}/{endpoint}", data, "GET");
            });
            return result;
        }
        public static bool SendRequest(string full_url, string payload, string method)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(full_url);

                var data = Encoding.UTF8.GetBytes(payload);

                request.Timeout = 10000;
                request.Method = method;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return true;
            }
            catch { }
            return false;
        }
    }
}
