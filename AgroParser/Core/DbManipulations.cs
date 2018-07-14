using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgroParser.Core
{
    class DbManipulations
    {
        Visual msg = new Visual();

        #region DatabaseCreateDelete

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
                "personName NVARCHAR(100), " +
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
            string command = createDBcommand;
            string message = "All Data from DB Deleted Successfully";
            DoWithDatabase(command, message);
            command = createDBTables;
            message = "All Tables was Created Successfully";
            DoWithDatabase(command, message);
        }

        public void EraseData()
        {
            string command = deleteDataCommand;
            string message = "All Data from DB Deleted Successfully";
            DoWithDatabase(command, message);
        }

        public void DropDB()
        {
            string command = dropDataBaseCommand;
            string message = "DataBase and Log is Deleted Successfully";
            DoWithDatabase(command, message);
        }

        private void DoWithDatabase(string comnd, string mesg)
        {
            string connectionString = @"Data Source=.;Integrated security=True;database=master";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = comnd;
                command.ExecuteNonQuery();
                msg.Message(mesg);
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

        #endregion

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
                Console.WriteLine($"Max count from table {table} in row id = {count}");
                connection.Close();
            }
            return count;
        }

        public async Task<(string link, int categoryId)> GetLinkAndIdByKey(int id, string sourceTable)
        {
            var answer = (link: "0", categoryId: 0);
            
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
                        answer.link = reader["link"].ToString();
                        try
                        {
                            answer.categoryId = Convert.ToInt32(reader["categoryId"]);
                        }
                        catch {}
                    }
                }
                connection.Close();
                if (answer.link == "" || answer.link == null)
                    Console.WriteLine($"Error!!! We didn't found value to key {id}");
                else if (answer.link == "0")
                    Console.WriteLine($"id: {id} is a key to main category with no link. All is Ok, doing next step");
                else
                    Console.WriteLine($"Success! We have found that id: {id}, is key to link: {answer.link} and key to categoryID = {answer.categoryId}");
            }
            return answer;
        }

        public async Task<(int companyes, int phones, int faxes, int emails)> GetParsedData()
        {
            var answer = (companyes: 0, phones: 0, faxes: 0, emails: 0);

            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(id) FROM company";
                answer.companyes = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = $"SELECT COUNT(phone) FROM phone WHERE phone <> '0'";
                answer.phones = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = $"SELECT COUNT(fax) FROM phone WHERE fax <> '0'";
                answer.faxes = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = $"SELECT COUNT(email) FROM phone WHERE email <> '0'";
                answer.emails = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            return answer;
        }
    }
}
