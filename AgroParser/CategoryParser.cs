using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                list.Add($"Category = {categoryName}"); //Отправляем вытащенное имя в список, который выведем пользователю на экран
            }
            return list.ToArray();
        }

        private static async Task PutToDataBase(string categoryName)
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
                command.CommandText = $"INSERT INTO category (parentId, categoryName, link) VALUES ('0', '{categoryName}', '0')";
                command.ExecuteNonQuery();
                //Проверяем, успешно ли данные легли в БД
                command.CommandText = $"SELECT * FROM category WHERE categoryName = '{categoryName}'";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["categoryName"].ToString() == categoryName)
                        check = true;
                }
                connection.Close();
            }
            if (check)
                Console.WriteLine($"Success! CategoryName {categoryName} was inserted in database");
            else
                Console.WriteLine($"Error! CategoryName {categoryName} was not inserted in database");
        }
    }
}
