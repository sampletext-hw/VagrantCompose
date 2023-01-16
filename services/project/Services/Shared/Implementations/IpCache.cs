using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Models.DTOs.Misc;
using Newtonsoft.Json;
using Services.ExternalServices;
using Services.Shared.Abstractions;

namespace Services.Shared.Implementations
{
    public class IpCache : IIPCache
    {
        private readonly IMemoryCache _memoryCache;

        public IpCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<(bool, string)> GetIsValidISP(string ip)
        {
            var isp = await _memoryCache.GetOrCreateAsync(ip, e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return LoadIsp(ip);
            });

            bool isBlocked = false;
            switch (isp)
            {
                case "ADMAN":
                case "Adman LLC":
                case "Admin LLC":
                case "AgotoZ HK Limited":
                case "Alibaba.com LLC":
                case "Alibaba.com Singapore E-Commerce Private Limited":
                case "Altech Radio Holdings (Pty) Ltd":
                case "Baraka Streaming Technologies Inc.":
                case "Bharat Sanchar Nigam Limited":
                case "Biterika Group LLC":
                case "Biterika Grupp LLC":
                case "Borodin Alexey Nikolaevich":
                case "BuyVM":
                case "Cablemas Telecomunicaciones SA de CV":
                case "ChinaNet Guangdong Province Network":
                case "ChinaNet Guangxi Province Network":
                case "DigitalOcean LLC":
                case "EPM Telecomunicaciones S.A. E.S.P.":
                case "F3 Netze E.V.":
                case "Foreningen for Digitala Fri- och Rattigheter":
                case "Foundation for Applied Privacy":
                case "Frantech Solutions":
                case "Giganet Internet Szolgaltato Kft":
                case "Hetzner Online AG":
                case "IONOS SE":
                case "Invitech ICT Services Kft.":
                case "Jose Manuel Palacios Vazquez":
                case "Kar-Tel LLC":
                case "Kustbandet AB":
                case "LIR LLC":
                case "Linode LLC":
                case "M247 Europe SRL":
                case "Macronet System Kft":
                case "Meverywhere Sp. z o.o.":
                case "Mingjing Technology Co. Ltd.":
                case "NetInformatik Inc.":
                case "Never Afk":
                case "Not SURF Net":
                case "OOO Network of Data-Centers Selectel":
                case "OVH SAS":
                case "Planet A A.S.":
                case "Quintex Alliance Consulting":
                case "Scaleway":
                case "StormyCloud Inc":
                case "Tachyon Communications Pvt Ltd":
                case "Telekom Malaysia Berhad":
                case "Tencent Cloud Computing (Beijing) Co. Ltd.":
                case "TerraHost AS":
                case "The Infrastructure Group B.V.":
                case "Triple T Broadband Public Company Limited":
                case "Viettel - CHT Company Ltd":
                case "Vodafone Hungary Ltd.":
                case "Vultr Holdings LLC":
                case "WiCAM Corporation Ltd":
                case "XET Kft.":
                case "Zencurity ApS":
                case "Zwiebelfreunde E.V.":
                case "dogado GmbH":
                case "nbiserv":
                case "netcup GmbH":
                    isBlocked = true;
                    break;
            }
            
            return (isBlocked, isp);
        }

        private async Task<string> LoadIsp(string ip)
        {
            try
            {
                using var client = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(5)
                    // DefaultRequestHeaders =
                    // {
                    //     {"accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"},
                    //     {"accept-encoding", "gzip, deflate, br"},
                    //     {"accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7"},
                    //     {"cookie", "__utmzz=utmcsr=(direct)|utmcmd=(none)|utmccn=(not set); _ga=GA1.2.1819784184.1657317122; ezosuibasgeneris-1=4432dd0b-dc03-48a0-75e6-b63c5a54e4af; __qca=P0-233099183-1657317157307; PHPSESSID=6fr782e5ud8fjmops5ra5bjg03; __utmzzses=1; _gid=GA1.2.855634465.1663190360"},
                    //     {"sec-ch-ua", "\"Google Chrome\";v=\"105\", \"Not)A;Brand\";v=\"8\", \"Chromium\";v=\"105\""},
                    //     {"sec-ch-ua-mobile", "?0"},
                    //     {"sec-ch-ua-platform", "\"Windows\""},
                    //     {"sec-fetch-dest", "document"},
                    //     {"sec-fetch-mode", "navigate"},
                    //     {"sec-fetch-site", "none"},
                    //     {"sec-fetch-user", "?1"},
                    //     {"upgrade-insecure-requests", "1"},
                    //     {"user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36"},
                    // },
                };
                var ipLocation = await client.GetStringAsync($"https://api.iplocation.net/?ip={ip}");

                var deserializeObject = JsonConvert.DeserializeObject<dynamic>(ipLocation);
                var isp = deserializeObject.isp;
                return isp;
            }
            catch
            {
                await TelegramAPI.Send($"Failed to determine ISP of {ip}");
                return ip;
            }
        }
    }
}