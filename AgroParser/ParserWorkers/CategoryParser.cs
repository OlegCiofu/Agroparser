using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgroParser.Core;
using AngleSharp.Dom.Html;

namespace AgroParser.ParserWorkers
{
    class CategoryParser : DbWorker
    {
        public async Task<string[]> Parse(int i, string link) 
        {
            IHtmlDocument document = await Parser.GetDocument(i,link); 
            var list = new List<string>();
            var items = document.QuerySelectorAll("span").Where(item => item.ParentElement.ParentElement.Id == "menu"); //Условие для вытаскивания главной рубрики

            foreach (var item in items)
            {
                string categoryName = item.TextContent; //получаем Название рубрики из вытащенного элемента
                await PutToDataBase(categoryName); //асинхронно отправляем вытащенное название рубрики в БД
                list.Add($"Category = {categoryName}"); //Отправляем вытащенное имя в список, который выведем пользователю на экран
            }
            return list.ToArray();
        }
    }
}
