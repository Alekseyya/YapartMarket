using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Newtonsoft.Json;
using Xunit;
using YapartMarket.Parser;
using YapartMarket.Parser.Data.Models;

namespace YapartMarket.UnitTests.YapartMarket.Parser
{
    public class AccessContextTest
    {
        private readonly AppSettings _appSettings;
        public AccessContextTest()
        {
            using (var r = new StreamReader("C:\\YapartStore\\YapartMarket\\YapartMarket.Parser\\appsettings.json"))
            {
                var json = r.ReadToEnd();
                _appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            }
        }


        [Fact]
        public void AccessContextTest_ReadJson()
        {
            var json = "{\"connectionAccess\": \"Provider=Microsoft.ACE.OLEDB.16.0; Data Source=\\\\192.168.0.96\\\\Parcer\\\\Parcer.accdb;\"}";
            var jsonO = JsonConvert.DeserializeObject<AppSettings>(json);
            Assert.NotNull(jsonO);
        }

        [Fact]
        public void AccessContext_ConnectionAccess2016File()
        {
            using (var r = new StreamReader("C:\\YapartStore\\YapartMarket\\YapartMarket.Parser\\appsettings.json"))
            {
                var json = r.ReadToEnd();
                var jsonObject = JsonConvert.DeserializeObject<AppSettings>(json);
                Assert.NotNull(jsonObject);
            }
        }

        [Fact]
        public void AccessContext_TestConnectionOleDB()
        {
            using (OleDbConnection connection = new OleDbConnection(_appSettings.connectionAccess))
            {
                string sqlExpression = "INSERT INTO Parcer_Output_Data (Brand, Article, 1Price, 1Count, 1Days, 2Price, 2Count, 2Days, 3Price, 3Count, 3Days, YourPrice, YourCount, YourDays)" +
                      "VALUES (@Brand, @Article, @1Price, @1Count, @1Days, @2Price, @2Count, @2Days, @3Price, @3Count, @3Days, @YourPrice, @YourCount, @YourDays)";

                connection.Open();
            };
        }

        [Fact]
        public void AccessContext_GetAll()
        {
            string sqlQuery = "Select * from Parcer_Sheriff";
            List<Product> listProducts = new List<Product>();
            using (OleDbConnection connection = new OleDbConnection(_appSettings.connectionAccess))
            {
                connection.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                int id = 0;
                foreach (var row in ds.Tables[0].AsEnumerable())
                {
                    id++;
                    var brand = row.ItemArray[0].ToString();
                    var article = row.ItemArray[1].ToString();
                    listProducts.Add(new Product() { Id = id, Brand = brand, Article = article });
                }
            }
            Assert.NotNull(listProducts);
        }
    }
}
