using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using AgroParser.Core;
using System.Linq;
using System.Threading.Tasks;

namespace AgroParser.ParserWorkers
{
    class SubCategoryParser : DbWorker
    {
        public async Task<string[]> Parse(int i, string link)
        {
            IHtmlDocument document = await Parser.GetDocument(i, link);
            var list = new List<string>();
            var items = document.QuerySelectorAll("a").Where(item => item.ParentElement.ParentElement.ParentElement.ParentElement.Id == "menu"); // Здесь вытаскиваем подрубрику
            string tempCategory = null;
            string parsedCat = null;
            string catId = null;
            addCategoryToDictionary(); // добавляем ID и название рубрики из БД в словарь

            foreach (var item in items)
            {
                string subCategoryName = item.TextContent;
                parsedCat = item.ParentElement.ParentElement.ParentElement.FirstChild.TextContent; // узнаем рубрику у подрубрики, которую вытащили
                string subCategoryHref = item.GetAttribute("href");
                //проверяем, если категория пропарсенной категории совпадает временному элементу tempCategory, то соответсвующий ID кладем в базу
                //если не соответсвует, то отправляем эту категорию в словарь, чтобы вытащить соответсвующий ID
                if (parsedCat != tempCategory)
                {
                    catId = FindCatInDic(parsedCat);
                    await PutToDataBase(catId, subCategoryName, subCategoryHref);
                }
                else
                {
                    await PutToDataBase(catId, subCategoryName, subCategoryHref);
                }
                //отправляем вытащенную информацию в список, который потом выведем на экран юзеру
                list.Add($"{catId} | {parsedCat} | {subCategoryName} | {subCategoryHref}");
            }

            return list.ToArray();
        }
        
        string FindCatInDic(string category)
        {
            string keyToId = category;
            foreach (KeyValuePair<string, string> entry in categoryById)
            {
                if (category == entry.Value)
                    keyToId = entry.Key;
            }
            return keyToId;
        }
    }
}
