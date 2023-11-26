using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using YapartMarket.Data.Implementation.Azure;

namespace YapartMarket.UnitTests.YapartMarket.Data
{
    public class TestAzureAliExpressProductRepository
    {
        private readonly IConfiguration _configuration;
        public TestAzureAliExpressProductRepository()
        {
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }
        [Fact]
        private async Task TestAzureAliExpressProductRepository_GetById_ProductIdNotZero()
        {
            //arrange
            var azureProductRepository = new AzureAliExpressProductRepository("dbo.aliExpressProducts", _configuration.GetConnectionString("SQLServerConnectionString"));
            var id = 1005003033814656;
            //act
            var product = (await azureProductRepository.GetAsync($"select * from dbo.aliExpressProducts where productId = {id}")).FirstOrDefault();
            //assert
            Assert.NotNull(product);
            Assert.Equal(1005003033814656, product.ProductId);
        }
    }
}
