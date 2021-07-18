using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication
{
    public static class File_Stacker
    {
        public enum Filetype
        {
            Screenshot, Document
        }

        private static List<Tuple<byte[], Filetype>> _idata = new List<Tuple<byte[], Filetype>>();
        public static async void Send(byte[] sdata, Filetype type)
        {
            await Task.Run(() =>
            {
                var data = new Tuple<byte[], Filetype>(sdata, type);
                _idata.Add(data);
                bool failed = false;
                foreach (var i in _idata.ToList())
                {
                    switch (i.Item2)
                    {
                        case Filetype.Screenshot:
                            if (Server.AsyncUploadScreenshot(i.Item1, Environment.UserName).Result)
                            {
                                _idata.Remove(data);
                            }
                            else failed = true;
                            break;
                    }
                    /// Didn't send, maybe try in the next request.
                    if (failed) break;
                }
            });

        }
    }
}
