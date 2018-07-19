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
                "SIZE = 2MB, MAXSIZE = 20MB, FILEGROWTH = 10%) " +
                "LOG ON (NAME = Database_Log, " +
                $"FILENAME = '{System.Windows.Forms.Application.StartupPath}\\Database.ldf', " + //C:\\Agroparser\\
                "SIZE = 1MB, " +
                "MAXSIZE = 20MB, " +
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
                "personPost NVARCHAR(200), " +
                "FOREIGN KEY(companyId) REFERENCES company(id) ON DELETE CASCADE); " +

                "CREATE TABLE phone " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "contactId INT REFERENCES contact (id), " +
                "phone NVARCHAR(60), " +
                "fax NVARCHAR(60), " +
                "email NVARCHAR(50), " +
                "FOREIGN KEY(contactId) REFERENCES contact(id) ON DELETE CASCADE); " +

                "CREATE TABLE work " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "processName NVARCHAR(60), " +
                "processValue INT, " +
                "processFlag BIT); " +

                "CREATE TABLE temp " +
                "(id INT PRIMARY KEY IDENTITY, " +
                "link NVARCHAR (150)," +
                "categoryId INT," +
                "isParsed BIT);";

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

        

        

        
    }
}
