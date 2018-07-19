using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using AgroParser.Core;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser.ParserWorkers
{
    class DetailCompanyParser : DbWorker
    {
        string fax = "0";
        string companyName;
        string address;
        string region;
        int companyId;
        string mainContactType;
        string mainMail;
        string mainPerson;
        string mainPhone;
        int contactId;

        public async Task<string[]> Parse(int i, string link, int categoryId)
        {
            IHtmlDocument document = await Parser.GetDocument(i, link);
            var list = new List<string>();
            var items = document.QuerySelectorAll("div").Where(item => item.Id == "incontent"); //Условие для вытаскивания главной рубрики

            foreach (var item in items)
            {
                string allhtml = item.InnerHtml.ToString(); //весь нужный отрывок кода
                //string findBy = "Тел."; //ключевое слово, чтобы вычислить кол-во отделов
                //int countContacts = (allhtml.Length - allhtml.Replace(findBy, "").Length) / findBy.Length;// Количество отделов для итерации
                await GetCompanyName(allhtml);
                await ParseAdress(allhtml); //Адрес компании
                
                companyId = await PutCompToDb(categoryId, companyName, region, address); //асинхронно отправляем вытащенное название рубрики в БД и сразу вытаскиваем id 
                list.Add($"|{categoryId} | {companyName} | {region} | {address}"); //Отправляем вытащенное имя в список, который выведем пользователю на экран

                await ParseMainContactType(allhtml);//comp14-cont cadv-whomain
                await ParseMainMail(allhtml);

                list.Add($"      {mainContactType} | {mainMail}");

                var mainBlock = await GetMainBlock(allhtml);
                foreach (string line in mainBlock)
                {
                    list.Add(line);
                }
                //string mainPerson = "";
                //string mainPhone = "";
                //int contactId = 0;
                var sublist = new List<string>();
                sublist = await ParseSubWorkers(allhtml, companyId);
                foreach (string line in sublist)
                {
                    list.Add(line);
                }
                Console.WriteLine($"Пропарсили |{categoryId} | {companyName} | {region} | {address} \n");
            }
            
            list.Add("\n");
            return list.ToArray();
        }

        #region ParseDetails

        async Task<List<string>> GetMainBlock(string allHtml)
        {
            var list = new List<string>();
            string mainPersonAll = await ParseMainPerson(allHtml);
            int countFags = (mainPersonAll.Length - mainPersonAll.Replace(";", "").Length) + 1;// Количество человек в главном отделе.
            string leftpad = "";
            for (int k = 1; k <= countFags; k++)
            {
                string mainPersonWithPhone = mainPersonAll.Trim(new Char[] { '\t', '\n' }); 

                if (mainPersonWithPhone == "")
                    break;

                for (int j = 1; j < k; j++)
                    mainPersonWithPhone = mainPersonWithPhone.Remove(0, (mainPersonWithPhone.IndexOf(';') + 1));

                if (mainPersonWithPhone.Contains(";"))
                    mainPersonWithPhone = mainPersonWithPhone.Substring(0, mainPersonWithPhone.IndexOf(';'));
                if (!(mainPersonWithPhone.Contains("--")))
                {
                    if (mainPersonWithPhone.Contains(" -"))
                    {
                        mainPerson = mainPersonWithPhone.Substring(0, mainPersonWithPhone.IndexOf('-'));
                        mainPerson = mainPerson.Trim(new Char[] { ' ', '-' }); //финальное имя персоны основноего блока
                        mainPerson = mainPerson.Replace("'", "ь");
                        //mainPerson = mainPerson.Replace(",", " ");
                        //mainPerson = mainPerson.Replace(".", "");
                        mainPhone = mainPersonWithPhone.Substring(mainPersonWithPhone.IndexOf("- "));
                    }
                    else
                        mainPhone = mainPersonWithPhone;
                }

                mainPhone = mainPhone.Trim(new Char[] { ' ', '-', '+', '\t', '\n', '(', ')' }); //финальный номер телефона этой персоны
                mainPhone = mainPhone.Replace(" ", "");
                mainPhone = mainPhone.Replace("-", "");
                mainPhone = mainPhone.Replace("(", "");
                mainPhone = mainPhone.Replace(")", "");
                if (mainPhone[0] == '0')
                    mainPhone = mainPhone.Insert(0, "38");
                string probel = "";
                bool personExist = false;
                bool phoneExists = await FindPhoneInDb(contactId, mainPhone);

                if (k > 1)
                    personExist = await FindPersonInDb(contactId, mainPerson);

                if (personExist == false)
                {
                    list.Add($"{probel.PadLeft(15)}| {mainPerson}\t | {mainPhone}");
                    contactId = await PutContactToDb(companyId, mainContactType, mainPerson, "0");
                    leftpad = mainPerson;
                }
                else
                {
                    if (phoneExists == false)
                        list.Add($"{probel.PadLeft(leftpad.Length * 2 + 16)}\t | {mainPhone} ");
                }

                await PutPhoneToDb(contactId, mainPhone, fax, mainMail);

            }
            return list;
        }
        
        async Task GetCompanyName(string allHtml)
        {
            companyName = await ParseName(allHtml);  //Имя компании 
            companyName = companyName.Replace("'", "ь");
        }
        
        async Task<string> ParseName(string allHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("h1").Where(item => item.ParentElement.ClassName == "cabout-hdr ta_center"); //Условие для вытаскивания главной рубрики
            foreach (var item in items)
            {
                answer = item.TextContent;
            }
            answer = answer.Replace("Контакты компании ", ""); //Финальное  Название компании
            return answer;
        }

        async Task ParseAdress(string allHtml)
        {
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "advadr"); //Условие для вытаскивания главной рубрики
            foreach (var item in items)
            {
                address = item.TextContent;
            }
            address = address.Replace("'", "ь");
            region = address.Remove(0, (address.IndexOf(',') + 1));//address.Replace("Украина , ", "");
            region = region.Substring(0, region.IndexOf(',')); //Вытаскиваем регион
        }

        async Task ParseMainContactType(string allHtml)
        {
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "comp14-cont cadv-whomain"); 
            foreach (var item in items)
            {
                mainContactType = item.ParentElement.FirstElementChild.TextContent;
            }
        }

        async Task ParseMainMail(string allHtml)
        {
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("td").Where(item => item.TextContent == "E-mail"); 
            foreach (var item in items)
            {
                mainMail = item.NextElementSibling.FirstElementChild.TextContent.ToString();
            }
        }

        async Task<string> ParseMainPerson(string allHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("td").Where(item => item.TextContent == "Телефон"); 
            foreach (var item in items)
            {
                answer = item.NextElementSibling.FirstElementChild.TextContent.ToString();
            }
            return answer;
        }

        async Task<List<string>> ParseSubWorkers(string allHtml, int companyId)
        {
            var list = new List<string>();
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "cadv-who2"); //Условие для вытаскивания главной рубрики
            foreach (var item in items)
            {
                string allOuterHtml = item.OuterHtml.ToString(); // весь внутренний блок 1 сотрудника
                string mainRubric = item.ParentElement.PreviousElementSibling.TextContent; //главный отдел, куда входит сотрудник
                string post = item.FirstElementChild.FirstChild.TextContent; //Должность сотрудника
                if (post == "")
                    post = "NoPost";
                string name = item.FirstElementChild.TextContent; //для получения Имени вытаскиваем Имя сотрудника и Должность
                if (name == "")
                    name = "NoName";
                if(name.Replace(post, "") != "")
                    name = name.Replace(post, ""); // Получаем Имя сотрудника, путем вычитания должности
                name = name.Replace("'", "ь");
                if (name == "")
                    name = "NoName";
                post = post.TrimEnd(' ', ':');//Убираем лишние символы в строке сотрудника
                if (post == "0")
                    post = "NoPost";
                string phone = await ParseSlavePhone(allOuterHtml);
                string fax = await ParseSlaveFax(allOuterHtml);
                string mail = await ParseSlaveMail(allOuterHtml);
                bool mailExistsinDb = await FindMailInDb(mail);
                if (mailExistsinDb == true)
                    mail = "0";
                int contactId = await PutContactToDb(companyId, mainRubric, name, post);
                await PutPhoneToDb(contactId, phone, fax, mail);

                list.Add($"-----{mainRubric} | {post} | {name} | {phone}/{ fax} | {mail}");
            }
            return list;
        }
        
        async Task<string> ParseSlavePhone(string allOuterHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allOuterHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "cadv-who2-row" && item.TextContent.Contains("Тел.:"));
            foreach (var item in items)
            {
                answer = item.TextContent;
                answer = answer.Replace("Тел.: ", "");
                answer = answer.Replace("(", "");
                answer = answer.Replace(")", "");
                answer = answer.Replace("+", "");
                answer = answer.Replace(" ", "");
                answer = answer.Replace("-", "");
                if (answer != "")
                    if (answer[0] == '0')
                        answer = answer.Insert(0, "38");
            }
            if (answer == "")
                answer = "0";
            return answer;
        }

        async Task<string> ParseSlaveFax(string allOuterHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allOuterHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "cadv-who2-row" && item.TextContent.Contains("Факс"));
            foreach (var item in items)
            {
                answer = item.TextContent;
                answer = answer.Replace("Факс: ", "");
                answer = answer.Replace("(", "");
                answer = answer.Replace(")", "");
                answer = answer.Replace("+", "");
                answer = answer.Replace(" ", "");
                answer = answer.Replace("-", "");
                if (answer != "")
                    if (answer[0] == '0')
                        answer = answer.Insert(0, "38");
            }
            if (answer == "")
                answer = "0";
            return answer;
        }

        async Task<string> ParseSlaveMail(string allOuterHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allOuterHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "cadv-who2-row" && item.TextContent.Contains("E-mail"));
            foreach (var item in items)
            {
                answer = item.TextContent;
                answer = answer.Replace("E-mail: ", "");
            }
            if (answer == "")
                answer = "0";
            return answer;
        }

        #endregion

    }

    //static class RemoveD
    //{
    //    public static string RemoveDiacritics(this string text)
    //    {
    //        if (string.IsNullOrWhiteSpace(text))
    //            return text;
    //        text = text.Normalize(NormalizationForm.FormD);
    //        var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
    //        return new string(chars).Normalize(NormalizationForm.FormC);
    //    }
    //}
}
