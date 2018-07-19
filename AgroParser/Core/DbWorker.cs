using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser.Core
{
    class DbWorker
    {

        private SqlConnection connection = new SqlConnection();
        protected Dictionary<string, string> categoryById = new Dictionary<string, string>();

        protected static async Task PutToDataBase(string categoryName)
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

        public static async Task PutToDataBase(string compLink, int idCategory)
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
                command.CommandText = $"INSERT INTO temp (link, categoryId, isParsed) VALUES ('{compLink}', '{idCategory}', '0' )";
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

        protected static async Task PutToDataBase(string parentId, string categoryName, string categoryLink)
        {
            bool check = false;
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
                        check = true;
                }
                connection.Close();
            }
            if (check)
                Console.WriteLine($"Success! parentId: {parentId}, categoryName: {categoryName}, link: {categoryLink} was iserted in database");
            else
                Console.WriteLine($"Error! parentId: {parentId}, categoryName: {categoryName}, link: {categoryLink} was not iserted in database");
        }

        protected void addCategoryToDictionary()
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
                Console.WriteLine("Error adding category to dictionary" + ex);
            }
        }

        protected static async Task<int> PutCompToDb(int categoryId, string name, string region, string address)
        {
            bool check = false;
            int id = 0;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
           // address = ParserWorkers.RemoveD.RemoveDiacritics(address);
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
                    if ((reader["categoryId"].ToString() == categoryId.ToString()) && (reader["region"].ToString() == region))
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
        
        protected static async Task<int> PutContactToDb(int companyId, string contactType, string personName, string personPost)
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
            if (check)
                Console.WriteLine($"Success! Person {personName} was inserted in database");
            else
                Console.WriteLine($"Error! Person {personName} was not inserted in database");

            return contactId;
        }

        protected static async Task PutPhoneToDb(int contactId, string phone, string fax, string email)
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
            if (check)
                Console.WriteLine($"Success! Phone {phone} was inserted in database");
            else
                Console.WriteLine($"Error! Phone {phone} was not inserted in database");
        }

        protected async Task<bool> FindPersonInDb(int id, string name)
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

        protected async Task<bool> FindPhoneInDb(int contactId, string phone)
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

        protected async Task<bool> FindMailInDb(string email)
        {
            bool answer = false;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM phone WHERE email = '{email}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (reader["email"].ToString() == email)
                        answer = true;
                }
                connection.Close();
            }
            if (answer)
                Console.WriteLine($"Verifying if email exist in DB. Email {email} exists in DB");
            else
                Console.WriteLine($"Verifying if email exist in DB. Email {email} doesn't exists in DB");
            return answer;
        }

        
    }
}
