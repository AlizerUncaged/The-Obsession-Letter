using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public static class Process_Utils
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int TOKEN_ASSIGN_PRIMARY = 0x1;
        private const int TOKEN_DUPLICATE = 0x2;
        private const int TOKEN_IMPERSONATE = 0x4;
        private const int TOKEN_QUERY = 0x8;
        private const int TOKEN_QUERY_SOURCE = 0x10;
        private const int TOKEN_ADJUST_GROUPS = 0x40;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x20;
        private const int TOKEN_ADJUST_SESSIONID = 0x100;
        private const int TOKEN_ADJUST_DEFAULT = 0x80;
        private const int TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_SESSIONID | TOKEN_ADJUST_DEFAULT);

        public static bool IsProcessOwnerAdmin(Process proc)
        {
            // if proc is null then wtf
            if (proc == null) return false;

            bool result = false;

            try
            {
                // if we cant get filepath of exe then its admin
                if (string.IsNullOrWhiteSpace(proc.MainModule.FileName)) return true;

                IntPtr ph = IntPtr.Zero;

                OpenProcessToken(proc.Handle, TOKEN_ALL_ACCESS, out ph);

                WindowsIdentity iden = new WindowsIdentity(ph);


                foreach (IdentityReference role in iden.Groups)
                {
                    if (role.IsValidTargetType(typeof(SecurityIdentifier)))
                    {
                        SecurityIdentifier sid = role as SecurityIdentifier;

                        if (sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) || sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
                        {
                            result = true;
                            break;
                        }
                    }
                }

                CloseHandle(ph);
            }
            // if an error occured then its admin
            catch { return true; }

            return result;
        }

        public static Process GetRandomRunningProcessThatIsNotAdmin()
        {
            Process proc = null;
            do
            {
                proc = GetRandomRunninProcess();
            }
            while (IsProcessOwnerAdmin(proc));
            return proc;
        }

        public static Process GetRandomRunninProcess()
        {
            var procs = Process.GetProcesses();
            return procs[Constants.Rand.Next(procs.Length)];
        }
    }
}
