using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Xunit;
using YapartMarket.BL.Implementation;
using YapartMarket.Core;
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
        private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory;

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
    ""aliexpress_solution_order_get_response"": {
        ""result"": {
            ""current_page"": 1,
            ""error_code"": ""0"",
            ""error_message"": ""操作成功"",
            ""page_size"": 20,
            ""success"": true,
            ""target_list"": {
                ""order_dto"": [
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru900785188"",
                        ""buyer_signer_fullname"": ""Artem Zhurkin"",
                        ""end_reason"": ""buyer_confirm_goods"",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""PAY_SUCCESS"",
                        ""gmt_create"": ""2022-11-09 08:36:07"",
                        ""gmt_pay_time"": ""2022-11-09 08:36:12"",
                        ""gmt_update"": ""2022-11-18 05:35:25"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5090889302950999,
                        ""order_status"": ""FINISH"",
                        ""payment_type"": ""MIXEDCARD"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5090889302960999,
                                    ""delivery_time"": ""6-6"",
                                    ""freight_commit_day"": ""22"",
                                    ""goods_prepare_time"": 3,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""0.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""Почта Росcии (AliExpress): Пункт выдачи в города"",
                                    ""logistics_type"": ""MYMALL_PUDO_CITY"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5090889302960999,
                                    ""product_count"": 1,
                                    ""product_id"": 1005002891808191,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/Aeae66d7a227c46a5922bd73a051fa198U.jpg"",
                                    ""product_name"": ""Брызговики задние SKODA KODIAQ, 2017-, 2 шт. (standart), NLFD.45.08.E13"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5090889302960999"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""1073.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""FINISH"",
                                    ""sku_code"": ""NLFD.45.08.E13"",
                                    ""son_order_status"": ""FINISH"",
                                    ""total_product_amount"": {
                                        ""amount"": ""1073.00"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Rogonskiy Store""
                    },
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru1092558138"",
                        ""buyer_signer_fullname"": ""Aleksey Chernigovskiy"",
                        ""end_reason"": ""pay_timeout"",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""NOT_PAY"",
                        ""gmt_create"": ""2022-11-08 19:01:15"",
                        ""gmt_update"": ""2022-11-08 19:32:06"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5090538061285340,
                        ""order_status"": ""FINISH"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5090538061295340,
                                    ""delivery_time"": ""11-13"",
                                    ""freight_commit_day"": ""31"",
                                    ""goods_prepare_time"": 3,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""0.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""Почта Росcии (AliExpress): Пункт выдачи в города"",
                                    ""logistics_type"": ""MYMALL_PUDO_CITY"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5090538061295340,
                                    ""product_count"": 2,
                                    ""product_id"": 1005003590247185,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/Af5a52fe361054ddaa57ce104742b36b5Y.jpg"",
                                    ""product_name"": ""Wing for trailer mound R13, nll.80.98.000rt NLL 80 98 000rt"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5090538061295340"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""803.70"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""FINISH"",
                                    ""sku_code"": ""NLL.80.98.000RT"",
                                    ""son_order_status"": ""FINISH"",
                                    ""total_product_amount"": {
                                        ""amount"": ""1607.40"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Rogonskiy Store""
                    },
                    {
                        ""biz_type"": ""AE_COMMON"",
                        ""buyer_login_id"": ""ru2743239039yayae"",
                        ""buyer_signer_fullname"": ""9501 user"",
                        ""end_reason"": ""buyer_confirm_goods"",
                        ""frozen_status"": ""NO_FROZEN"",
                        ""fund_status"": ""PAY_SUCCESS"",
                        ""gmt_create"": ""2022-11-08 02:10:48"",
                        ""gmt_pay_time"": ""2022-11-08 02:11:43"",
                        ""gmt_update"": ""2022-11-24 07:01:54"",
                        ""has_request_loan"": false,
                        ""issue_status"": ""NO_ISSUE"",
                        ""logisitcs_escrow_fee_rate"": """",
                        ""order_id"": 5090565331666012,
                        ""order_status"": ""FINISH"",
                        ""payment_type"": ""MIXEDCARD"",
                        ""phone"": false,
                        ""product_list"": {
                            ""order_product_dto"": [
                                {
                                    ""can_submit_issue"": false,
                                    ""child_id"": 5090565331676012,
                                    ""delivery_time"": ""5-7"",
                                    ""freight_commit_day"": ""23"",
                                    ""goods_prepare_time"": 3,
                                    ""issue_status"": ""NO_ISSUE"",
                                    ""logistics_amount"": {
                                        ""amount"": ""0.00"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""logistics_service_name"": ""Почта Роcсии (AliExpress): Курьером в города"",
                                    ""logistics_type"": ""MYMALL_DOOR_CITY"",
                                    ""money_back3x"": false,
                                    ""order_id"": 5090565331676012,
                                    ""product_count"": 1,
                                    ""product_id"": 1005004039048742,
                                    ""product_img_url"": ""http:\/\/ae01.alicdn.com\/kf\/A147c4ca814674d579bda41ea42ea9008j.jpg"",
                                    ""product_name"": ""Protection and PPC for Honda Fit 2007-2012, 1,3;1,5 gasoline, at FWD;4WD, right steering, nlz.18.25.030 NLZ 18 25 030"",
                                    ""product_snap_url"": ""\/\/www.aliexpress.com\/snapshot\/null.html?orderId=5090565331676012"",
                                    ""product_unit"": ""piece"",
                                    ""product_unit_price"": {
                                        ""amount"": ""4348.80"",
                                        ""currency_code"": ""RUB""
                                    },
                                    ""send_goods_operator"": ""SELLER_SEND_GOODS"",
                                    ""show_status"": ""FINISH"",
                                    ""sku_code"": ""NLZ.18.25.030"",
                                    ""son_order_status"": ""FINISH"",
                                    ""total_product_amount"": {
                                        ""amount"": ""4348.80"",
                                        ""currency_code"": ""RUB""
                                    }
                                }
                            ]
                        },
                        ""seller_login_id"": ""ru1404462327cets"",
                        ""seller_operator_login_id"": ""ru1404462327cets"",
                        ""seller_signer_fullname"": ""Rogonskiy Store""
                    }
                ]
            },
            ""total_count"": 3,
            ""total_page"": 1
        },
        ""request_id"": ""15sgqh35qaep4""
    }
}";
            var startDay = new DateTime(2022, 11, 08).StartOfDay();
            var endDay = new DateTime(2022, 11, 09).EndOfDay();
            var orderStatusList = new List<OrderStatus>()
            {
                OrderStatus.SELLER_PART_SEND_GOODS,
                OrderStatus.FINISH,
                OrderStatus.WAIT_SELLER_SEND_GOODS
            };
            var mockItopClient = new Mock<ITopClient>();
            mockItopClient.Setup(x => x.Execute(It.IsAny<AliexpressSolutionOrderGetRequest>()))
                .Returns(new Top.Api.Response.AliexpressSolutionOrderGetResponse() { Body = body });
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption,
                _mockAzureAliExpressOrderRepository.Object, _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockMapper.Object);
            //act
            var aliExpressOrderList = aliExpressOrderService.QueryOrderDetail(startDay, endDay, orderStatusList);
            //assert
            Assert.NotNull(aliExpressOrderList);
            Assert.True(aliExpressOrderList.Result.Count == 3);
        }

        [Fact]
        public void QueryOrderDetail_ReturnEmptyOrder()
        {
            //arrange
            var aliExpressOrderService = new AliExpressOrderService(_mockLogger.Object, _aliExpressOption, _mockAzureAliExpressOrderRepository.Object,
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockMapper.Object);
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
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockMapper.Object);
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
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockMapper.Object);
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
                _mockAzureAliExpressOrderDetailRepository.Object, _mockServiceScopeFactory.Object, _mockMapper.Object);
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
