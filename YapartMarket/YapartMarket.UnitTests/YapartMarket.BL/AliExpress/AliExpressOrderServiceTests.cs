using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO.AliExpress.OrderGetResponse;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Mapper;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.UnitTests.YapartMarket.BL
{
    public class AliExpressOrderServiceTests
    {
        private readonly IOptions<AliExpressOptions> _aliExpressOption;
        private readonly IConfiguration _configuration;
        private Mock<ILogger<AliExpressOrderService>> _mockLogger;
        private Mock<IAliExpressOrderRepository> _mockAzureAliExpressOrderRepository;
        private Mock<IAliExpressOrderDetailRepository> _mockAzureAliExpressOrderDetailRepository;
        private Mock<IMapper> _mockMapper;
        private IMapper _mapper;
        private readonly Mock<IServiceScopeFactory> _mockServiceScope;

        public AliExpressOrderServiceTests()
        {
            _configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            _aliExpressOption = Options.Create(new AliExpressOptions()
            {
                AppKey = _configuration["AliExpress:AppKey"],
                AppSecret = _configuration["AliExpress:AppSecret"],
                AccessToken = _configuration["AliExpress:AccessToken"],
                HttpsEndPoint = _configuration["AliExpress:HttpsEndPoint"],
            });
            _mockLogger = new Mock<ILogger<AliExpressOrderService>>();
            _mockAzureAliExpressOrderRepository = new Mock<IAliExpressOrderRepository>();
            _mockAzureAliExpressOrderDetailRepository = new Mock<IAliExpressOrderDetailRepository>(); //todo можно заменить на базовый интерфейс!!!
            _mockMapper = new Mock<IMapper>(); //todo можно заменить на базовый интерфейс!!!

            _mockServiceScope = new Mock<IServiceScopeFactory>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AliExpressOrderProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public void QueryOrderDetail_Deserialize()
        {
            //arrange
            var startDay = new DateTime(2022, 11, 01).StartOfDay();
            var endDay = new DateTime(2022, 11, 04).EndOfDay();
            var orderStatusList = new List<OrderStatus>()
            {
                OrderStatus.SELLER_PART_SEND_GOODS,
                OrderStatus.FINISH,
                OrderStatus.WAIT_SELLER_SEND_GOODS
            };
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption,
                _mockAzureAliExpressOrderRepository.Object, _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScope.Object, _mockMapper.Object);
            //act
            var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(startDay, endDay, orderStatusList);
            //assert
            Assert.NotNull(aliExpressOrderList);
        }

        [Fact]
        public void QueryOrderDetail_ReturnEmptyOrder()
        {
            //arrange
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption, _mockAzureAliExpressOrderRepository.Object,
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScope.Object, _mockMapper.Object);
            //act
            var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(new DateTime(2021, 01, 01).StartOfDay(), new DateTime(2021, 01, 01).EndOfDay());
            //assert
            Assert.Null(aliExpressOrderList);
        }

        [Fact]
        public async Task QueryOrderDetail_ReturnNotNull()
        {
            //arrange
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption, _mockAzureAliExpressOrderRepository.Object,
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScope.Object, _mockMapper.Object);
            var dateTimeNow = DateTime.UtcNow;
            //act
            var aliExpressOrderList = await aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-20).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay());
            //assert
            Assert.True(aliExpressOrderList.Count > 0);
        }
        [Fact]
        public async Task QueryOrderDetail_GetDetailsByOrder_ReturnNotNull()
        {
            //arrange
            var orderId = 5029342366925571;
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption, _mockAzureAliExpressOrderRepository.Object, 
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScope.Object, _mockMapper.Object);
            var dateTimeNow = DateTime.UtcNow;
            //act
            var aliExpressOrderList = await aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-2).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay());
            //assert
            Assert.True(aliExpressOrderList.Any(x=>x.order_id == orderId));
        }

        [Fact]
        public void Test()
        {
            var orderUpdates = new List<AliExpressOrderDetail>()
            {
                new()
                {
                    Id = 89,
                    OrderId = 1
                },
                new()
                {
                Id = 79,
                OrderId = 1
                },
                new()
                {
                    Id = 69,
                    OrderId = 1
                },
                new()
                {
                    Id = 55,
                    OrderId = 2
                },
                new()
                {
                    Id = 79,
                    OrderId = 2
                }
            };

            var orderDetailUpdates = new List<AliExpressOrderDetail>()
            {
                new()
                {
                    Id = 79,
                    OrderId = 1
                },
                new()
                {
                    Id = 69,
                    OrderId = 1
                },
                new()
                {
                    Id = 79,
                    OrderId = 2
                }
            };

            
            var expected = orderUpdates.Where(x=> !orderDetailUpdates.Any(t=>t.OrderId == x.OrderId && t.Id == x.Id)).ToList();

            Assert.True(true);
        }

        [Fact]
        public async Task TestAliExpressOrderService_AddCurrentOrderWithError()
        {
            //arrange
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption, _mockAzureAliExpressOrderRepository.Object,
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScope.Object, _mockMapper.Object);
            var dateTimeNow = DateTime.UtcNow;
            var orderFromAli =  (await aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-20).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay())).Where(x=>x.order_id == 5013832858011459).ToList();
            var aliExpressOrders = _mapper.Map<List<OrderDto>, List<AliExpressOrder>>(orderFromAli);
            //act
            await aliExpressOrderService.AddOrders(aliExpressOrders);
            Func<Task> actionAsync = async () => await aliExpressOrderService.AddOrders(aliExpressOrders);
            //assert
            await Assert.ThrowsAsync<Exception>(actionAsync);
        }

        //[Fact]
        //public async Task TestAliExpressOrderService_QueryOrderDetail_CreateNewOrders()
        //{
        //    //arrange
        //    var aliExpressOrderService = (IAliExpressOrderService)new AliExpressOrderService(_mockLogger.Object, _aliExpressOption, _mockAzureAliExpressOrderRepository.Object, _mockAzureAliExpressOrderDetailRepository.Object, _mockMapper.Object);
        //    //act
        //    var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(new DateTime(2021, 09, 01).StartOfDay(), DateTime.Today.EndOfDay());
        //    Assert.NotNull(aliExpressOrderList);
        //    await aliExpressOrderService.AddOrders(aliExpressOrderList);
        //    //assert

        //    Action action = () => aliExpressOrderService.AddOrders(aliExpressOrderList);
        //    //assert
        //    var jsonReaderException = Assert.Throws<Exception>(action);
        //    Assert.Empty(jsonReaderException.Message);

        //}
    }
}
