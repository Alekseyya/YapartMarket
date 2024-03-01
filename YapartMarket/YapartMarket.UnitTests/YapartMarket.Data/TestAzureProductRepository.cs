using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;
using YapartMarket.Data.Implementation.Azure;

namespace YapartMarket.UnitTests.YapartMarket.Data
{
    public class TestAzureProductRepository
    {
        private readonly IConfiguration _configuration;
        public TestAzureProductRepository()
        {
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }

        [Fact]
        private void TestAzureProductRepository_GetById_ProductIdNotZero()
        {
            //arrange
            var azureProductRepository = new AzureProductRepository("dbo.products", _configuration.GetConnectionString("SQLServerConnectionString"));
            var id = 34737;
            //act
            var product = azureProductRepository.GetByIdAsync(id);
            //assert
            //Assert.Equal();
        }
    }
}
