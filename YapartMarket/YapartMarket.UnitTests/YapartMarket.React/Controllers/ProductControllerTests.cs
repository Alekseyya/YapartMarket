using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO.Yandex;
using YapartMarket.Data.Implementation.Azure;
using YapartMarket.React.Controllers;

namespace YapartMarket.UnitTests.YapartMarket.React.Controllers
{
    public class ProductControllerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IConfiguration _configuration;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<IAzureProductRepository> _mockProductRepository;

        public ProductControllerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            _mockMapper = new Mock<IMapper>();
            _mockProductRepository = new Mock<IAzureProductRepository>();
        }
        [Fact]
        private async Task SetProducts_OneQuery_Success()
        {
            //arrange
            var azureProductRepository = new AzureProductRepository("dbo.products_tmp", _configuration.GetConnectionString("SQLServerConnectionString"));
            var products = (await azureProductRepository.GetAsync("select TOP 1000 * from dbo.products_tmp")).ToList();
            var listItem = products.Select(x=> new ItemDto()
            {
                Count = 60,
                Sku = x.Sku
            }).ToList();
            var updateProducts = new ItemsDto() {Products = listItem};
            var productController = new ProductController(_mockMapper.Object,_configuration, _mockProductRepository.Object);
            //act
            var timer = new Stopwatch();
            timer.Start();
            var result = await productController.SetProducts(updateProducts);
            timer.Stop();
            //assert
            var okObjectResult = result as OkResult;
            Assert.NotNull(okObjectResult);
            TimeSpan timeTaken = timer.Elapsed;
            _testOutputHelper.WriteLine("Time taken: " + timeTaken.ToString(@"m\:ss\.fff"));
        }
        [Fact]
        private async Task SetProducts_CreateQuery_Success()
        {
            //arrange
            var azureProductRepository = new AzureProductRepository("dbo.products_tmp", _configuration.GetConnectionString("SQLServerConnectionString"));
            var products = (await azureProductRepository.GetAsync("select TOP 1000 * from dbo.products_tmp")).ToList();
            var listItem = products.Select(x => new ItemDto()
            {
                Count = 60,
                Sku = x.Sku
            }).ToList();
            var str = new StringBuilder();
            var json = JsonConvert.SerializeObject(listItem);
            _testOutputHelper.WriteLine(json);
        }
    }
}
