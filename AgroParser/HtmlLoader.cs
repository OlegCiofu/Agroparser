using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AgroParser
{
    class HtmlLoader
    {
        readonly HttpClient client;

        public HtmlLoader()
        {
            client = new HttpClient();
            //url = $"{settings.BaseUrl}/{settings.Prefix}.html";
        }

        public async Task<string> GetSource(int id, string startLink)
        {
          
            var currentUrl = startLink.Replace(".html", $"_p{id.ToString()}.html");
            var response = await client.GetAsync(currentUrl);
            byte[] bytes = null;
            string recodedResponse = null;
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                bytes = await response.Content.ReadAsByteArrayAsync();
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
            }
            if (recodedResponse.Contains("comp-tit") == true)
                return recodedResponse;
            else
                return "404";
        }

        public async Task<string> GetSource(string link)
        {
            var response = await client.GetAsync(link);
            byte[] bytes = null;
            string recodedResponse = null;
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                bytes = await response.Content.ReadAsByteArrayAsync();
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
            }
            return recodedResponse;

        }

        //public async Task<string> GetSourcePage(int mainLink, int page, string whatTable)
        //{
        //    string getmainLink = await GetLinkByKey(mainLink, whatTable);
        //    var currentUrl = getmainLink.Replace(".html", $"_p{page.ToString()}.html");
        //    Console.WriteLine($"Replacing link from |{getmainLink}| to {currentUrl}");
        //    Console.WriteLine($"Parsing page |{currentUrl}|");
        //    var response = await client.GetAsync(currentUrl);
        //    byte[] bytes = null;
        //    string recodedResponse = null;
        //    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        bytes = await response.Content.ReadAsByteArrayAsync();
        //        Encoding encoding = Encoding.GetEncoding("windows-1251");
        //        recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
        //    }
        //    if (recodedResponse.Contains("comp-tit") == true)
        //    {
        //        await PutIdToDb(mainLink);
        //        return recodedResponse;
        //    }

        //    else
        //        return "404";
        //}

        //public async Task<string> GetSourceEmpty(int idKey, string whatTable)
        //{

        //    string finalLink = await GetLinkByKey(idKey, whatTable);
        //    if (finalLink == "0")
        //        return "0";

        //    Console.WriteLine($"We found from DBase that IdKey = {idKey}, has link = {finalLink}");
        //    Console.WriteLine($"Replacing link from |{url}| to {finalLink}");
        //    Console.WriteLine($"Parsing page |{finalLink}|");
        //    var response = await client.GetAsync(finalLink);
        //    byte[] bytes = null;
        //    string recodedResponse = null;
        //    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        bytes = await response.Content.ReadAsByteArrayAsync();
        //        Encoding encoding = Encoding.GetEncoding("windows-1251");
        //        recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
        //    }
        //    await PutIdToDb(idKey);
        //    return recodedResponse;

        //}

        //public async Task<string> GetSourceEmpty2(int idKey, string source)
        //{

        //    string finalLink = await GetLinkByKey(idKey, source);
        //    Console.WriteLine($"We found from DBase that IdKey = {idKey}, has link = {finalLink}");
        //    var currentUrl = url.Replace("{part1}/{part2}.html", finalLink);
        //    var response = await client.GetAsync(currentUrl);
        //    byte[] bytes = null;
        //    string recodedResponse = null;
        //    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        bytes = await response.Content.ReadAsByteArrayAsync();
        //        Encoding encoding = Encoding.GetEncoding("windows-1251");
        //        recodedResponse = encoding.GetString(bytes, 0, bytes.Length);
        //    }

        //    return recodedResponse;

        //}

        //private static async Task<string> GetLinkByKey(int id, string sourceTable)
        //{
        //    string link = "";
        //    string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();
        //        SqlCommand command = new SqlCommand();
        //        command.Connection = connection;
        //        command.CommandText = $"SELECT * FROM {sourceTable} WHERE id = '{id}'";
        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            link = reader["link"].ToString();
        //            Console.WriteLine($"THIS {link}");
        //            if (reader["id"].ToString() == id.ToString())
        //            {
        //                link = reader["link"].ToString();
        //                Console.WriteLine($"We have found that id = {id}, is key to link = {link}");
        //            }
        //            else
        //                Console.WriteLine($"ERROR!!! We didn't found value to key {id}");
        //        }
        //        connection.Close();
        //    }
        //    return link;
        //}

        //private static async Task PutIdToDb(int id)
        //{
        //    string connectionString = @"Data Source=.;Initial Catalog=parserDB;Integrated Security=True";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        // Открываем асинхронно коннект с БД
        //        await connection.OpenAsync();
        //        //Отправляем в БД необходимые значения
        //        SqlCommand command = new SqlCommand();
        //        command.Connection = connection;
        //        command.CommandText = $"INSERT INTO temp (link, categoryId) VALUES ('0', '{id}')";
        //        command.ExecuteNonQuery();

        //        //Закрываем коннект с БД
        //        connection.Close();
        //    }


        //}
    }
}
