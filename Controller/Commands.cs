﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Controller
{
    // all params are string because c# doesnt auto convert fuk
    public class Commands
    {
        [Command(Help = "Sets current active shell to be controlled.\r\nUsage: open [index] Ex. open 0")]
        public void Open(string index) {
            try
            {
                int _index = int.Parse(index);

                Program.ActiveClient = Program.MainServer.Clients[_index];
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

    public class Command : Attribute {
        public string Help;
    }
}