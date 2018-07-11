using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser.Core
{
    class DbManipulations
    {
        Visual msg = new Visual();
        

        private string createDBcommand = "CREATE DATABASE parserDB ON PRIMARY " +
                "(NAME = parserDB, " +
                $"FILENAME = '{System.Windows.Forms.Application.StartupPath}\\Database.mdf', " + //C:\\Agroparser\\
                "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
                "LOG ON (NAME = Database_Log, " +
                $"FILENAME = '{System.Windows.Forms.Application.StartupPath}\\Database.ldf', " + //C:\\Agroparser\\
                "SIZE = 1MB, " +
                "MAXSIZE = 5MB, " +
                "FILEGROWTH = 10%)";

        private string createDBTables = "USE parserDB;" +
                "CREATE TABLE category " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "parentId INT, " +
                "categoryName NVARCHAR(40), " +
                "link NVARCHAR(100)); " +

                "CREATE TABLE company " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "categoryId INT, " +
                "name NVARCHAR(100), " +
                "region NVARCHAR(100), " +
                "address NVARCHAR(300) COLLATE Cyrillic_General_100_CI_AS_SC, " +
                "FOREIGN KEY(categoryID) REFERENCES category(id) ON DELETE CASCADE); " +

                "CREATE TABLE contact " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "companyId INT, " +
                "contactType NVARCHAR(40), " +
                "personName NVARCHAR(50), " +
                "personPost NVARCHAR(100), " +
                "FOREIGN KEY(companyId) REFERENCES company(id) ON DELETE CASCADE); " +

                "CREATE TABLE phone " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "contactId INT REFERENCES contact (id), " +
                "phone NVARCHAR(60), " +
                "fax NVARCHAR(60), " +
                "email NVARCHAR(50), " +
                "FOREIGN KEY(contactId) REFERENCES contact(id) ON DELETE CASCADE); " +

                "CREATE TABLE temp " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "link NVARCHAR (150)," +
                "categoryId INT)";

        private string deleteDataCommand = "DELETE FROM category;" +
                                           "DELETE FROM company;" +
                                           "DELETE FROM contact;" +
                                           "DELETE FROM phone;" +
                                           "DELETE FROM temp;";

        private string dropDataBaseCommand = "ALTER DATABASE parserDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                                             "DROP DATABASE parserDB;";





        public void CreateDB()
        {
            string connectionString = @"Data Source=.;Integrated security=True;database=master";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = createDBcommand;
                command.ExecuteNonQuery();
                msg.Message("DataBase is Created Successfully");
                command.CommandText = createDBTables;
                command.ExecuteNonQuery();
                msg.Message("All Tables was Created Successfully");

            }
            catch (System.Exception ex)
            {
                msg.Message(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void EraseData()
        {
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = deleteDataCommand;
                command.ExecuteNonQuery();
                msg.Message("All Data from DB Deleted Successfully");
            }
            catch (System.Exception ex)
            {
                msg.Message(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void DropDB()
        {
            string connectionString = @"Data Source=.;Integrated security=True;database=master";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = dropDataBaseCommand;
                command.ExecuteNonQuery();
                msg.Message("DataBase is Deleted Successfully");

            }
            catch (System.Exception ex)
            {
                msg.Message(ex.ToString());
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public async Task<int> GetMaxCountDB(string table)
        {
            int count = 0;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT MAX(id) FROM {table}";
                count = Convert.ToInt32(command.ExecuteScalar());
                //while (reader.Read())
                //{
                //    count = Convert.ToInt32(reader["id"]);
                //}
                Console.WriteLine($"Max count from table {table} in row id = {count}");
                connection.Close();
            }
            return count;
        }

        public async Task<string> GetLinkByKey(int id, string sourceTable)
        {
            string link = "";
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM {sourceTable} WHERE id = '{id}'";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["id"].ToString() == id.ToString())
                    {
                        link = reader["link"].ToString();
                        Console.WriteLine($"We have found that id = {id}, is key to link = {link}");
                    }
                    else
                        Console.WriteLine($"ERROR!!! We didn't found value to key {id}");
                }
                connection.Close();
            }
            return link;
        }

        public async Task<int> GetIdByKey(int id)
        {
            int categoryId = 0;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM temp WHERE id = '{id}'";
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    // link = reader["link"].ToString();
                    if (reader["id"].ToString() == id.ToString())
                    {
                        categoryId = Convert.ToInt32(reader["categoryId"]);
                        Console.WriteLine($"We have found that id = {id}, is key to categoryID = {categoryId}");
                    }
                    else
                        Console.WriteLine($"ERROR!!! We didn't found value to key {id}");
                }
                connection.Close();
            }
            return categoryId;
        }




        //public static async Task PutToDataBase(string oneValue, string TSQLTextWrite, string TSQLTextRead)
        //{
        //    string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        //асинхронно открываем коннект
        //        await connection.OpenAsync();
        //        //Отправляем в БД пропарсенные значения
        //        SqlCommand command = new SqlCommand();
        //        command.Connection = connection;
        //        command.CommandText = $"INSERT INTO category (parentId, categoryName, link) VALUES ('0', '{oneValue}', '0')";
        //        command.ExecuteNonQuery();
        //        //Проверяем, успешно ли данные легли в БД
        //        command.CommandText = $"SELECT * FROM category WHERE categoryName = '{oneValue}'";
        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            if (reader[oneValue].ToString() == oneValue)
        //                Console.WriteLine($"INSERT INTO category (categoryName) VALUE {oneValue} was DONE...");
        //            else
        //                Console.WriteLine($"INSERT INTO category (categoryName) VALUE {oneValue} was NOT DONE! ERROR!!!");
        //        }
        //        connection.Close();
        //    }
        //}
    }
}
