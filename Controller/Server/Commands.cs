using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("Help");
        }
    }
}
