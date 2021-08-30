using System;
using System.IO;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.UnitTests.YapartMarket.Data
{
    public class TestAzureGenericRepository
    {
        private readonly IConfiguration _configuration;
        public TestAzureGenericRepository()
        {
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }
        [Fact]
        public void TestAzureGenericRepository_Update()
        {
            //arrange
            //act
            //assert
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                connection.Open();
                connection.Execute("update aliExpressProducts set sku = @sku, inventory = @inventory, updatedAt = @updatedAt where productId = @productId",
                    new
                    {
                        sku = "123",
                        inventory = 5,
                        updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                        productId = 1005003033814656
                    });
            }
        }
    }
}
