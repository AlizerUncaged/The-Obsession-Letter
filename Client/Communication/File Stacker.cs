using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication
{
    /// <summary>
    /// A class that stacks file requests to the server incase it didn't get sent.
    /// </summary>
    public static class File_Stacker
    {
        public enum Filetype
        {
            Screenshot, File
        }

        private static List<Tuple<byte[], Filetype>> _idata = new List<Tuple<byte[], Filetype>>();
        public static async void Send(byte[] sdata, Filetype type, string filename = null)
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
                            if (Server.AsyncUploadFile(i.Item1, Environment.UserName, "screenshot").Result)
                            {
                                _idata.Remove(data);
                            }
                            else failed = true;
                            break;
                        case Filetype.File:
                            if (Server.AsyncUploadFile(i.Item1, Environment.UserName, "file", filename).Result)
                            {
                                _idata.Remove(data);
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
