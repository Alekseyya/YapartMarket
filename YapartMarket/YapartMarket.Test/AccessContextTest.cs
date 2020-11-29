using System;
using System.Data.OleDb;
using System.IO;
using Dapper;
using Newtonsoft.Json;
using Xunit;

namespace YapartMarket.Test
{
    public class AccessContextTest
    {
        private readonly AppSettings _appSettings;
        public AccessContextTest()
        {
            using (var r = new StreamReader("C:\\MyOwn\\YapartStore\\YapartMarket\\YapartMarket.Parser\\appsettings.json"))
            {
                var json = r.ReadToEnd();
                _appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            }
        }

        [Fact]
        public void Test1()
        {
            using (var connection = new OleDbConnection(_appSettings.ConnectionStringAccess))
            {
                connection.Open();
                var result = connection.Query<string>("select * from Tovari");
                var a = "a";
            }
        }
    }
}
