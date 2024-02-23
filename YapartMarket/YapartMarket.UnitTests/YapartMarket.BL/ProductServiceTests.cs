using Microsoft.Extensions.Configuration;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Data;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class ProductServiceTests
    {
        private readonly IConfiguration configuration;
        private Mock<IRepositoryFactory> repositoryFactory;
        public ProductServiceTests()
        {
            configuration = (IConfiguration)new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            repositoryFactory = new Mock<IRepositoryFactory>();
        }
        [Fact]
        public async Task UpdateGoodsIdFromProducts_UpdateProducts_Success()
        {
            //Act
            var productService = new ProductService(repositoryFactory.Object, configuration);
            //Arrange
            await productService.UpdateGoodsIdFromProductsAsync();
        }
        [Fact]
        public async Task UpdateAliProductIdFromProducts_UpdateProducts_Success()
        {
            //Act
            var productService = new ProductService(repositoryFactory.Object, configuration);
            //Arrange
            await productService.UpdateAliProductIdAsync();
        }
    }
}
