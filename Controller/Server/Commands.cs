using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Server
{

    /// <summary>
    /// Commands for the client.
    /// </summary>
    public class Commands
    {
        [Command(Help = "Displays help.")]
        public void Help()
        {
            Console.WriteLine(string.Empty);

            Type commandtype = this.GetType();

            var methods = commandtype.GetMethods().Where(m => m.GetCustomAttributes(typeof(Command), false).Length > 0);

            int maxmethodnamelength = methods.OrderByDescending(x => x.Name.Length).FirstOrDefault().Name.Length;

            foreach (var method in methods)
            {
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
