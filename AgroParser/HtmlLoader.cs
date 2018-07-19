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
            var currentUrl = "";
            if (id == 0)
            {
                currentUrl = startLink;
            }
            else
            {
                currentUrl = startLink.Replace(".html", $"_p{id.ToString()}.html");
            }
            
            var response = await client.GetAsync(currentUrl);
            byte[] bytes = null;
            string recodedResponse = null;
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                bytes = await response.Content.ReadAsByteArrayAsync();
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
            }
            if (recodedResponse.Contains("comp-tit") == true || recodedResponse.Contains("comptitle") == true)
                return recodedResponse;
            else
                return "404";
        }
        
    }
}
