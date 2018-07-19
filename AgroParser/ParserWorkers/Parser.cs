using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser.ParserWorkers
{
    class Parser
    {
        public static async Task<AngleSharp.Dom.Html.IHtmlDocument> GetDocument(int i, string link)
        {
            HtmlLoader loader = new HtmlLoader();
            var source = await loader.GetSource(i, $"{link}");
            if (source == "404")
                return null;
            var domParser = new HtmlParser();
            var document = await domParser.ParseAsync(source);
            return document;

        }
    }
}
