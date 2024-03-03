using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.Core.DTO.Yandex;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Data.Implementation.Azure;

namespace YapartMarket.UnitTests.YapartMarket.Data
{
    public class TestAzureGenericRepository
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IConfiguration _configuration;
        public TestAzureGenericRepository(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
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

        [Fact]
        private void ConvertToDataTable_Read_Success()
        {
            //arrange
            var product = new Product()
            {
                Id = 1,
                AliExpressProductId = 12312312313,
                Sku = "qweqwe",
                Count = 2,
                Type = ProductType.FIT.ToString()
            };
            var list = new List<Product>();
            list.Add(product);
            var azureProductRepository = new AzureProductRepository("dbo.products", _configuration.GetConnectionString("SQLServerConnectionString"));
            //act
            var dataTable = azureProductRepository.ConvertToDataTable(list);
            //assert
            Assert.Equal(product.Sku, dataTable.Rows[0]["sku"]);
            Assert.Equal(product.Count, dataTable.Rows[0]["count"]);
            Assert.Equal(product.Type, dataTable.Rows[0]["type"]);
        }
        [Fact]
        private async Task Update_UpdateDapper_Success()
        {
            //arrange
            var timer = new Stopwatch();

            var azureProductRepository = new AzureProductRepository("dbo.products_tmp", _configuration.GetConnectionString("SQLServerConnectionString"));
            var products = (await azureProductRepository.GetAsync("select TOP 200* from dbo.products_tmp")).ToList();
            var actions = products.Select(x => new
            {
                count = x.Count,
                updatedAt = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK"),
                sku = x.Sku
            });
            var sql = "update products set count = @count, updatedAt = @updatedAt where sku = @sku";
            //act
            timer.Start();
            await azureProductRepository.UpdateAsync(sql,actions);
            timer.Stop();
            //assert
            TimeSpan timeTaken = timer.Elapsed;
            _testOutputHelper.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
        }
    }
}
