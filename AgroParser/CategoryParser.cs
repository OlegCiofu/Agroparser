using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using AngleSharp.Dom.Html;

namespace AgroParser
{
    class CategoryParser
    {
        private SqlConnection connection = new SqlConnection();

        public async Task<string[]> Parse(IHtmlDocument document)
        {

            var list = new List<string>();
            var items = document.QuerySelectorAll("span").Where(item => item.ParentElement.ParentElement.Id == "menu"); //Условие для вытаскивания главной рубрики

            foreach (var item in items)
            {
                string categoryName = item.TextContent; //получаем Название рубрики из вытащенного элемента
                await PutToDataBase(categoryName); //асинхронно отправляем вытащенное название рубрики в БД
                list.Add($"ParentId = 0; Category = {categoryName}; HREF = 0"); //Отправляем вытащенное имя в список, который выведем пользователю на экран
            }
            return list.ToArray();
        }

        private static async Task PutToDataBase(string categoryName)
        {
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //асинхронно открываем коннект
                await connection.OpenAsync();
                //Отправляем в БД пропарсенные значения
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO category (parentId, categoryName, link) VALUES ('0', '{categoryName}', '0')";
                command.ExecuteNonQuery();
                //Проверяем, успешно ли данные легли в БД
                command.CommandText = $"SELECT * FROM category WHERE categoryName = '{categoryName}'";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["categoryName"].ToString() == categoryName)
                        Console.WriteLine($"INSERT INTO category (categoryName) VALUE {categoryName} was DONE...");
                    else
                        Console.WriteLine($"INSERT INTO category (categoryName) VALUE {categoryName} was NOT DONE! ERROR!!!");
                }
                connection.Close();
            }
        }
    }
}
