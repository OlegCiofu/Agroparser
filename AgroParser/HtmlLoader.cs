using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser
{
    class HtmlLoader
    {
        readonly HttpClient client;

        public HtmlLoader()
        {
            client = new HttpClient();
        }

        public async Task<string> GetSource(int id, string startLink)
        {
            var currentUrl = startLink.Replace(".html", $"_p{id.ToString()}.html");
            var response = await client.GetAsync(currentUrl);
            byte[] bytes = null;
            string recodedResponse = null;
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                bytes = await response.Content.ReadAsByteArrayAsync();
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
            }
            if (recodedResponse.Contains("comp-tit") == true)
                return recodedResponse;
            else
                return "404";
        }

        public async Task<string> GetSource(string link)
        {
            var response = await client.GetAsync(link);
            byte[] bytes = null;
            string recodedResponse = null;
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                bytes = await response.Content.ReadAsByteArrayAsync();
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
            }
            return recodedResponse;
        }
    }
}
