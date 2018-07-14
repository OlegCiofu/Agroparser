using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser
{
    class DetailCompanyParser
    {
        private SqlConnection connection = new SqlConnection();

        public async Task<string[]> Parse(IHtmlDocument document, int categoryId)
        {
            var list = new List<string>();
            var items = document.QuerySelectorAll("div").Where(item => item.Id == "incontent"); //Условие для вытаскивания главной рубрики

            foreach (var item in items)
            {
                string fax = "0";
                string allhtml = item.InnerHtml.ToString(); //весь нужный отрывок кода
                string findBy = "Тел."; //ключевое слово, чтобы вычислить кол-во отделов
                int countContacts = (allhtml.Length - allhtml.Replace(findBy, "").Length) / findBy.Length;// Количество отделов для итерации
                string companyName = await ParseName(allhtml);  //Имя компании 
                companyName = companyName.Replace("'", "ь");
                int categoryID = categoryId;
                string address = await ParseAdress(allhtml); //Адрес компании
                address = address.Replace("'", "ь");
                string region = address.Remove(0, (address.IndexOf(',') + 1));//address.Replace("Украина , ", "");
                region = region.Substring(0, region.IndexOf(',')); //Вытаскиваем регион
                int companyId = await PutCompToDb(categoryID, companyName, region, address); //асинхронно отправляем вытащенное название рубрики в БД и сразу вытаскиваем id 
                list.Add($"|{categoryID} | {companyName} | {region} | {address}"); //Отправляем вытащенное имя в список, который выведем пользователю на экран

                string mainContactType = await ParseMainContactType(allhtml);//comp14-cont cadv-whomain
                string mainMail = await ParseMainMail(allhtml);
                list.Add($"      {mainContactType} | {mainMail}");

                string mainPersonAll = await ParseMainPerson(allhtml);
                int countFags = (mainPersonAll.Length - mainPersonAll.Replace(";", "").Length) + 1;// Количество человек в главном отделе.
                string mainPerson = "";
                string mainPhone = "";
                int contactId = 0;
                string leftpad = "";
                string post = "0";
                for (int i = 1; i <= countFags; i++)
                {
                    string mainPersonWithPhone = mainPersonAll;
                    mainPersonWithPhone = mainPersonWithPhone.Trim(new Char[] { '\t', '\n'});
                    if (mainPersonWithPhone == "")
                        break;
                    for (int j = 1; j < i; j++)
                        mainPersonWithPhone = mainPersonWithPhone.Remove(0, (mainPersonWithPhone.IndexOf(';') + 1));

                    if (mainPersonWithPhone.Contains(";"))
                        mainPersonWithPhone = mainPersonWithPhone.Substring(0, mainPersonWithPhone.IndexOf(';'));

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

                    if (i > 1)
                        personExist = await FindPersonInDb(contactId, mainPerson);

                    if (personExist == false)
                    {
                        list.Add($"{probel.PadLeft(15)}| {mainPerson}\t | {mainPhone}");
                        contactId = await PutContactToDb(companyId, mainContactType, mainPerson, post);
                        leftpad = mainPerson;
                    }
                    else
                    {
                        if (phoneExists == false)
                            list.Add($"{probel.PadLeft(leftpad.Length * 2 + 16)}\t | {mainPhone} ");
                    }

                    await PutPhoneToDb(contactId, mainPhone, fax, mainMail);
                   
                }
                var sublist = new List<string>();
                sublist = await ParseSubWorkers(allhtml, companyId);
                foreach (string line in sublist)
                {
                    list.Add(line);
                }
                Console.WriteLine($"Пропарсили |{categoryID} | {companyName} | {region} | {address} \n");
            }
            
            list.Add("\n");
            return list.ToArray();
        }

        #region ParseDetails

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

        async Task<string> ParseAdress(string allHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "advadr"); //Условие для вытаскивания главной рубрики
            foreach (var item in items)
            {
                answer = item.TextContent;
            }
            return answer;
        }

        async Task<string> ParseMainContactType(string allHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName == "comp14-cont cadv-whomain"); 
            foreach (var item in items)
            {
                answer = item.ParentElement.FirstElementChild.TextContent;
            }
            return answer;
        }

        async Task<string> ParseMainMail(string allHtml)
        {
            string answer = "";
            var parser = new HtmlParser();
            var document = await parser.ParseAsync(allHtml);
            var items = document.QuerySelectorAll("td").Where(item => item.TextContent == "E-mail"); 
            foreach (var item in items)
            {
                answer = item.NextElementSibling.FirstElementChild.TextContent.ToString();
            }
            return answer;
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

        #region DB
        
        private static async Task<int> PutCompToDb(int categoryId, string name, string region, string address)
        {
            bool check=false;
            int id = 0;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            address = RemoveD.RemoveDiacritics(address);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //асинхронно открываем коннект
                await connection.OpenAsync();
                //Отправляем в БД пропарсенные значения
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO company (categoryId, name, region, address) VALUES ('{categoryId}', '{name}', '{region}', '{address}')";
                command.ExecuteNonQuery();
                //Проверяем, успешно ли данные легли в БД
                command.CommandText = $"SELECT * FROM company WHERE name = '{name}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await (reader.ReadAsync()))
                {
                    if ((reader["categoryId"].ToString() == categoryId.ToString()) && (reader["region"].ToString() == region) && (reader["address"].ToString() == address))
                    {
                        check = true;
                        id = Convert.ToInt32(reader["id"].ToString());
                    }
                }
                connection.Close();
            }
            if (check)
                Console.WriteLine($"Success! Company {name} was inserted in database");
            else
                Console.WriteLine($"Error!!! Company {name} was not inserted in database!!!\n");

            return id;
        }

        private static async Task<int> PutContactToDb(int companyId, string contactType, string personName, string personPost)
        {
            bool check = false;
            int contactId = 0;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //асинхронно открываем коннект
                await connection.OpenAsync();
                //Отправляем в БД пропарсенные значения
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO contact (companyId, contactType, personName, personPost) VALUES ('{companyId}', '{contactType}', '{personName}', '{personPost}')";
                command.ExecuteNonQuery();
                //Проверяем, успешно ли данные легли в БД
                command.CommandText = $"SELECT * FROM contact WHERE companyId = '{companyId}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if ((reader["contactType"].ToString() == contactType) && (reader["personName"].ToString() == personName))
                    {
                        check = true;
                        contactId = Convert.ToInt32(reader["id"].ToString());
                    }
                }
                connection.Close();
            }
            if(check)
                Console.WriteLine($"Success! Person {personName} was inserted in database");
            else
                Console.WriteLine($"Error! Person {personName} was not inserted in database");

            return contactId;
        }

        private static async Task PutPhoneToDb(int contactId, string phone, string fax, string email)
        {
            bool check = false;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //асинхронно открываем коннект
                await connection.OpenAsync();
                //Отправляем в БД пропарсенные значения
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO phone (contactId, phone, fax, email) VALUES ('{contactId}', '{phone}', '{fax}', '{email}')";
                command.ExecuteNonQuery();
                //Проверяем, успешно ли данные легли в БД
                command.CommandText = $"SELECT * FROM phone WHERE contactId = '{contactId}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if ((reader["phone"].ToString() == phone) && (reader["fax"].ToString() == fax) && (reader["email"].ToString() == email))
                        check = true;
                }
                connection.Close();
            }
            if(check)
                Console.WriteLine($"Success! Phone {phone} was inserted in database");
            else
                Console.WriteLine($"Error! Phone {phone} was not inserted in database");
        }

        public async Task<bool> FindPersonInDb(int id, string name)
        {
            bool answer = false;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM contact WHERE id = '{id}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (reader["personName"].ToString() == name)
                        answer = true;
                }
                connection.Close();
            }
            if (answer)
                Console.WriteLine($"Verifying if person exist in DB. Person {name} exists in DB");
            else
                Console.WriteLine($"Verifying if person exist in DB. Person {name} doesn't exists in DB");

            return answer;
        }

        public async Task<bool> FindPhoneInDb(int contactId, string phone)
        {
            bool answer = false;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM phone WHERE contactId = '{contactId}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (reader["phone"].ToString() == phone)
                        answer = true;
                }
                connection.Close();
            }
            if (answer)
                Console.WriteLine($"Verifying if phone exist in DB. Phone {phone} exists in DB");
            else
                Console.WriteLine($"Verifying if phone exist in DB. Phone {phone} doesn't exists in DB");
            return answer;
        }

        public async Task<bool> FindMailInDb(string email)
        {
            bool answer = false;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT email FROM phone";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (reader["email"].ToString() == email)
                        answer = true;
                }
                connection.Close();
            }
            if(answer)
                Console.WriteLine($"Verifying if email exist in DB. Email {email} exists in DB");
            else
                Console.WriteLine($"Verifying if email exist in DB. Email {email} doesn't exists in DB");
            return answer;
        }
        #endregion
    }

    static class RemoveD
    {
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
