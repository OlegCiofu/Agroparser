using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser
{
    class LinkToCompanyParser
    {
        private SqlConnection connection = new SqlConnection();

        public async Task<string[]> Parse(IHtmlDocument document, int idCategory)
        {
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

        private static async Task PutToDataBase(string compLink, int idCategory)
        {
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            bool check = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Открываем асинхронно коннект с БД
                await connection.OpenAsync();
                //Отправляем в БД необходимые значения
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO temp (link, categoryId) VALUES ('{compLink}', '{idCategory}')";
                await command.ExecuteNonQueryAsync();
                //Проверяем, успешно ли отправленные значения сохранились
                command.CommandText = $"SELECT * FROM temp WHERE link = '{compLink}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (reader["link"].ToString() == compLink)
                        check = true;
                }
                connection.Close();
            }
            if (check)
                Console.WriteLine($"Success! Link {compLink} was inserted in database");
            else
                Console.WriteLine($"Error! Link {compLink} was not inserted in database");
        }
    }
}
