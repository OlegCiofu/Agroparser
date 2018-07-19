using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using AgroParser.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser.ParserWorkers
{
    class LinkToCompanyParser : DbWorker
    {
        public async Task<string[]> Parse(int i, string link, int idCategory)
        {
            IHtmlDocument document = await Parser.GetDocument(i, link);
            if (document == null)
                return null;

            var list = new List<string>();
            var items = document.QuerySelectorAll("a").Where(item => item.ParentElement.ClassName == "comp-tit");//вытаскиваем элемент с ссылкой на компанию
            string linkTo = null;
            foreach (var item in items)
            {
                linkTo = item.GetAttribute("href");//вытаскиваем ссылку из элемента
                string compName = item.TextContent;
                string postlink = linkTo.Replace(".html", "-cont.html");//меняем ссылку компании на ссылку контакта этой компании

                list.Add($"Category: {idCategory}, Company: {compName}, Link = {postlink}");//кидаем вытащенную инфу в список, который выведется на экран пользователя
                PutToDataBase(postlink, idCategory).GetAwaiter();//отправляем инфу в БД
            }
            return list.ToArray();


        }

        
    }
}
