using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser
{
    class SubCategoryParser
    {
        private SqlConnection connection = new SqlConnection();
        Dictionary<string, string> categoryById = new Dictionary<string, string>();

        public async Task<string[]> Parse(IHtmlDocument document)
        {
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

        void addCategoryToDictionary()
        {
            connection.ConnectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT id, categoryName FROM category";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    categoryById.Add(reader["id"].ToString(), reader["categoryName"].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
        }

        private static async Task PutToDataBase(string parentId, string categoryName, string categoryLink)
        {
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO category (parentId, categoryName, link) VALUES ('{parentId}', '{categoryName}', '{categoryLink}')";
                command.ExecuteNonQuery();

                command.CommandText = $"SELECT * FROM category WHERE parentId = '{parentId}' AND categoryName = '{categoryName}' AND link = '{categoryLink}'";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if ((reader["parentId"].ToString() == parentId) && (reader["categoryName"].ToString() == categoryName) && (reader["link"].ToString() == categoryLink))
                        Console.WriteLine($"INSERT INTO category (parentId, categoryName, categoryLink) VALUES ('{parentId}', '{categoryName}', '{categoryLink}' SUCCESS...");
                    else
                        Console.WriteLine($"ERROR!!! INSERT INTO category (parentId, categoryName, link) VALUES ('{parentId}', '{categoryName}', '{categoryLink}' with ERROR!");
                }
                connection.Close();
            }
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
