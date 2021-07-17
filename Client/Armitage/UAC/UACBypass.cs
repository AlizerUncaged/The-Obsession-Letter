using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.UAC
{
    public enum UACMethods
    {
        ICMLuaUtil = 0
    }
    public class UACBypass
    {

        public static bool QuickStart(string execpath, string args, UACMethods method)
        {
            try
            {

                return true;
            }
            catch
            {

            }
            return false;

        }

    }
}
