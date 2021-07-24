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
    }
}
