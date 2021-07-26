using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// The abstract attribute for all methods as commands.
    /// </summary>
    public class Command : Attribute
    {
        public string Help;
        public string Usage;
    }
    public enum Command_Types
    {
        CMDCommand = 0x0, LCommand = 0x1
    }
    public enum Message_Types
    {
        Exited = 0x0, CMDMessage = 0x1
    }
}
