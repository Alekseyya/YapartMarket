using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using YapartMarket.Core.Config;
using YapartMarket.Data.Implementation.Azure;

namespace YapartMarket.UnitTests.YapartMarket.Data
{
    public class TestAliExpressOrderDetailRepository
    {
        private readonly IConfiguration _configuration;
        public TestAliExpressOrderDetailRepository()
        {
            TypeMapper.Initialize("YapartMarket.Core.Models.Azure");
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }

        [Fact]
        private async Task TestAliExpressOrderDetailRepository_GetById_ProductIdNotZero()
        {
            //arrange
            var aliExpressOrderDetailRepository = new AzureAliExpressOrderDetailRepository("dbo.order_details", _configuration.GetConnectionString("SQLServerConnectionString"));
            var id = 0;
            //act
            var orderDetail = await aliExpressOrderDetailRepository.GetById(id);
            //assert
            Assert.Equal(1005002891644727, orderDetail.ProductId);
        }

        [Fact]
        private async Task TestAliExpressOrderDetailRepository_GetById_ProductCountNotZero()
        {
            //arrange
            var aliExpressOrderDetailRepository = new AzureAliExpressOrderDetailRepository("dbo.order_details", _configuration.GetConnectionString("SQLServerConnectionString"));
            var id = 0;
            //act
            var orderDetail = await aliExpressOrderDetailRepository.GetById(id);
            //assert
            Assert.Equal(1, orderDetail.ProductCount);
        }
    }
}
