using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication
{
    public class Screenshot_Stacker
    {
        private List<byte[]> _idata = new List<byte[]>();
        public async void Send(byte[] sdata)
        {
            await Task.Run(() => {
                _idata.Add(sdata);

                foreach (var i in _idata)
                {
                    if (Server.AsyncUploadScreenshot(sdata, Environment.UserName).Result)
                    {
                        _idata.Remove(i);
                    }
                    /// Didn't send, maybe try in the next request.
                    else break;
                }
            });
          
        }
    }
}
