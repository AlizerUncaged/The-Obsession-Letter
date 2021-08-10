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
        public static async void Send(byte[] sdata, Filetype type, string filename = null)
        {
            await Task.Run(async () =>
           {
               switch (type)
               {
                   case Filetype.Screenshot:
                       await Server.AsyncUploadFile(sdata, Client.Utilities.User_Info.GetUserName(), "screenshot");
                       break;
                   case Filetype.File:
                       await Server.AsyncUploadFile(sdata, Client.Utilities.User_Info.GetUserName(), "file", filename);
                       break;
               }
           });

        }
    }
}
