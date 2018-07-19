using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser.Core
{
    class DbChecker
    {
        public async void markCompanyAsParsed(int id)
        {
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE temp SET isParsed = 1 WHERE id = '{id}'";
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }



        public async Task<(int links, int companyes, int phones, int faxes, int emails)> GetParsedData()
        {
            var answer = (links: 0, companyes: 0, phones: 0, faxes: 0, emails: 0);
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT COUNT(link) FROM temp";
                answer.links = Convert.ToInt32(command.ExecuteScalar());
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

        public async Task<int> GetMaxCountDB()
        {
            int linkId = 0;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT MAX(id) FROM temp WHERE isParsed = 1";
                var dbAnswer = command.ExecuteScalar();
                if (!(dbAnswer is DBNull))
                {
                    linkId = Convert.ToInt32(dbAnswer);
                    Console.WriteLine($"Parsing is resumed. Start id = {linkId}");
                }
                connection.Close();
            }
            return linkId;
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
                        catch { }
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

        public int TimeCheckerDb()
        {
            int answer = 0 ;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM work WHERE processName = 'internal-string-time-elapsed'";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    answer = Convert.ToInt32(reader["processValue"]);
                }
                connection.Close();
            }
            return answer;
        }

        public async void storeCounterToDb(int counter)
        {
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE work SET processValue = {counter} WHERE processName = 'internal-string-time-elapsed'";
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public void PutIntProcessToDb(string processName, int processValue, bool processFlag)
        {
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO work (processName, processValue, processFlag) VALUES ('{processName}', '{processValue}', '{processFlag}')";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool CheckIntProcessFromDb(string processName)
        {
            bool answer = false;
            string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = command.CommandText = $"SELECT * FROM work WHERE processName = '{processName}'";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["processFlag"] != DBNull.Value)
                        answer = Convert.ToBoolean(reader["processFlag"]);
                }
                connection.Close();
            }
            return answer;
        }


    }
}
