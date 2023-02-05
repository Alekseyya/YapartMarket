using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
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
        private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;

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
                GetOrderList = _configuration["AliExpress:GetOrderList"],
                Url = _configuration["AliExpress:Url"],
                AuthToken = _configuration["AliExpress:AuthToken"]
            });
            _mockLogger = new Mock<ILogger<AliExpressOrderService>>();
            _mockAzureAliExpressOrderRepository = new Mock<IAliExpressOrderRepository>();
            _mockAzureAliExpressOrderDetailRepository = new Mock<IAliExpressOrderDetailRepository>(); //todo можно заменить на базовый интерфейс!!!
            _mockMapper = new Mock<IMapper>(); //todo можно заменить на базовый интерфейс!!!

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(Deserializer<IReadOnlyList<AliExpressOrder>>)))
                .Returns(new OrderDeserializer());

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            _mockServiceScopeFactory = serviceScopeFactory;
            var mockFactory = new Mock<IHttpClientFactory>();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_aliExpressOption.Value.Url);
            client.DefaultRequestHeaders.Add("x-auth-token", _aliExpressOption.Value.AuthToken);
            mockFactory.Setup(_ => _.CreateClient("aliExpress")).Returns(client);

            _mockHttpClientFactory = mockFactory;
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
            var body = @"{
    ""data"": {
        ""total_count"": 1,
        ""orders"": [
            {
                ""id"": 2212094469888866,
                ""created_at"": ""2022-12-09T20:57:26.606005+00:00"",
                ""paid_at"": ""2022-12-09T20:57:37.450422+00:00"",
                ""updated_at"": ""2022-12-09T20:57:38.502102+00:00"",
                ""status"": ""Created"",
                ""payment_status"": ""Hold"",
                ""delivery_status"": ""Init"",
                ""delivery_address"": ""Россия, Нижегородская обл, Арзамас г, ПМС-73,3,24, 607220"",
                ""antifraud_status"": ""Passed"",
                ""buyer_country_code"": ""RU"",
                ""buyer_name"": ""Кукушкин Сергей Викторович "",
                ""order_display_status"": ""WaitSendGoods"",
                ""buyer_phone"": ""+79200358382"",
                ""order_lines"": [
                    {
                        ""id"": 2212095463331348,
                        ""item_id"": ""1005002891582458"",
                        ""sku_id"": ""12000029610583328"",
                        ""sku_code"": ""CHERY.S.6314003"",
                        ""name"": ""Подкрылок с шумоизоляцией CHERY Tiggo 5, 2014-2016,  (задний левый), CHERY.S.6314003"",
                        ""img_url"": ""https://ae04.alicdn.com/kf/S6ac980cb81ee417c9c20e9b312b4809e8.jpg_50x50.jpg"",
                        ""item_price"": 34000,
                        ""quantity"": 1.0,
                        ""total_amount"": 32980,
                        ""properties"": [
                            ""Autofamily"",
                            ""100000015"",
                            ""sell_by_piece"",
                            ""1"",
                            ""0.650"",
                            ""30"",
                            ""10"",
                            ""20""
                        ],
                        ""buyer_comment"": null,
                        ""height"": 10.0,
                        ""weight"": 650.0,
                        ""width"": 20.0,
                        ""length"": 30.0,
                        ""issue_status"": ""NoDispute"",
                        ""promotions"": [
                            {
                                ""ae_promotion_id"": 5000000071015191,
                                ""ae_activity_id"": 5000000071015191,
                                ""code"": null,
                                ""promotion_type"": ""FlexiCoin"",
                                ""discount"": 1020,
                                ""discount_currency"": ""RUB"",
                                ""original_discount"": 1020,
                                ""original_discount_currency"": ""RUB"",
                                ""promotion_target"": ""Sale"",
                                ""budget_sponsor"": ""Seller""
                            }
                        ],
                        ""order_line_fees"": null
                    }
                ],
                ""total_amount"": 64559,
                ""seller_comment"": null,
                ""fully_prepared"": false,
                ""finish_reason"": null,
                ""cut_off_date"": null,
                ""cut_off_date_histories"": null,
                ""shipping_deadline"": null,
                ""next_cut_off_date"": null,
                ""pre_split_postings"": null,
                ""logistic_orders"": null,
                ""commission"": null
            }
        ]
    },
    ""error"": null
}";
            var startDay = new DateTime(2022, 12, 08).StartOfDay();
            var endDay = new DateTime(2022, 12, 09).EndOfDay();
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption,
                _mockAzureAliExpressOrderRepository.Object, _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockHttpClientFactory.Object);
           
            //act
            var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(startDay, endDay);
            //assert
            Assert.NotNull(aliExpressOrderList);
            Assert.True(aliExpressOrderList.Result.Count == 3);
        }
        //[Fact]
        //public void QueryOrderDetail1_Deserialize()
        //{
        //    //arrange
        //    var startDay = new DateTime(2022, 12, 03).StartOfDay();
        //    var endDay = new DateTime(2022, 11, 09).EndOfDay();
        //    var orderStatusList = new List<OrderStatus>()
        //    {
        //        OrderStatus.SELLER_PART_SEND_GOODS,
        //        OrderStatus.FINISH,
        //        OrderStatus.WAIT_SELLER_SEND_GOODS
        //    };
        //    var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption,
        //        _mockAzureAliExpressOrderRepository.Object, _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockHttpClientFactory.Object);
        //    //act
        //    var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(startDay, endDay, orderStatusList);
        //    //assert
        //    Assert.NotNull(aliExpressOrderList);
        //    Assert.True(aliExpressOrderList.Result.Count == 3);
        //}

        [Fact]
        public void QueryOrderDetail_ReturnEmptyOrder()
        {
            //arrange
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption, _mockAzureAliExpressOrderRepository.Object,
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockHttpClientFactory.Object);
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
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockHttpClientFactory.Object);
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
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockHttpClientFactory.Object);
            var dateTimeNow = DateTime.UtcNow;
            //act
            var aliExpressOrderList = await aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-2).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay());
            //assert
            Assert.True(aliExpressOrderList.Any(x=>x.OrderId == orderId));
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
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockHttpClientFactory.Object);
            var dateTimeNow = DateTime.UtcNow;
            var orderFromAli =  (await aliExpressOrderService.QueryOrderDetail(dateTimeNow.AddDays(-20).StartOfDay(), dateTimeNow.AddDays(+1).EndOfDay())).Where(x=>x.OrderId == 5013832858011459).ToList();
            //act
            await aliExpressOrderService.AddOrders(orderFromAli.ToList());
            Func<Task> actionAsync = async () => await aliExpressOrderService.AddOrders(orderFromAli.ToList());
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
