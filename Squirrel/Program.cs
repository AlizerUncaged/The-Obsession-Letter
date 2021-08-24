using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel
{
    class Program
    {
        static async Task Main(string[] args)
        {

            await Utilities.RunRig();

            await Task.Delay(-1);
        }
    }
}
