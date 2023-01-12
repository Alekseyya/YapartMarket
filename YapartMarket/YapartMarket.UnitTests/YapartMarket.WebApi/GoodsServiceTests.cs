using Dapper;
using Microsoft.Extensions.Configuration;
using Moq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using YapartMarket.WebApi.Services;
using YapartMarket.WebApi.ViewModel.Goods;

namespace YapartMarket.UnitTests.YapartMarket.WebApi
{
    public sealed class GoodsServiceTests
    {
        private IConfiguration configuration;
        private string sqLightConnection;

        public GoodsServiceTests()
        {
            configuration = (IConfiguration)new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
            sqLightConnection = configuration["ConnectionStrings:SQLightConnectionString"];
        }
        [Fact]
        public async Task Execute_SaveOrder_ReturnNewOrder()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var clientHandlerStub = new DelegatingHandlerStub();
            var client = new HttpClient(clientHandlerStub);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var goodsService = new GoodsService(configuration, mockFactory.Object);
            var orderModel = new OrderNewViewModel()
            {
                OrderNewDataViewModel = new OrderNewDataViewModel()
                {
                    MerchantId = 5046,
                    Shipments = new List<OrderNewShipment>()
                        {
                            new OrderNewShipment()
                            {
                                ShipmentId = "946032218",
                                ShipmentDate = "2021-02-17T09:34:03+03:00",
                                Items = new List<OrderNewShipmentItem>
                                {
                                    new OrderNewShipmentItem()
                                    {
                                        GoodsId = "100023763738",
                                        OfferId = "3951",
                                        ItemIndex = "1",
                                        Price = 11900,
                                        Quantity = 1
                                    },
                                    new OrderNewShipmentItem()
                                    {
                                        GoodsId = "100023763500",
                                        OfferId = "3953",
                                        ItemIndex = "2",
                                        Price = 600,
                                        Quantity = 2
                                    }
                                }
                            }
                        }
                }
            };
            await goodsService.SaveOrderAsync(orderModel);
            using (var connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSqlConnectionString")))
            {
                await connection.OpenAsync();
                var order = await connection.QueryAsync<Core.DTO.Goods.Order>(@"select * from ""order""");
                var orderItems = await connection.QueryAsync<Core.DTO.Goods.OrderItem>(@"select * from ""orderItem""");
                Assert.True(order != null);
                Assert.True(orderItems != null);
                await connection.ExecuteAsync(@"truncate table ""order""; truncate table ""orderItem""");
            }
        }
        [Fact]
        public async Task Execute_FindOrder_ReturnNewOrder()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var clientHandlerStub = new DelegatingHandlerStub();
            var client = new HttpClient(clientHandlerStub);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var goodsService = new GoodsService(configuration, mockFactory.Object);
            var orderModel = new OrderNewViewModel()
            {
                OrderNewDataViewModel = new OrderNewDataViewModel()
                {
                    MerchantId = 5046,
                    Shipments = new List<OrderNewShipment>()
                        {
                            new OrderNewShipment()
                            {
                                ShipmentId = "946032218",
                                Items = new List<OrderNewShipmentItem>
                                {
                                    new OrderNewShipmentItem()
                                    {
                                        GoodsId = "100023763738",
                                        OfferId = "3951",
                                        ItemIndex = "Вертикальный пылесос Kitfort  KT-535-2 White",
                                        Price = 11900,
                                        Quantity = 1
                                    },
                                    new OrderNewShipmentItem()
                                    {
                                        GoodsId = "100023763500",
                                        OfferId = "3953",
                                        ItemIndex = "Ковер в машину",
                                        Price = 600,
                                        Quantity = 2
                                    }
                                }
                            }
                        }
                }
            };
            var order = await goodsService.GetOrderAsync(orderModel);
        }

        public class DelegatingHandlerStub : DelegatingHandler
        {
            private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

            public DelegatingHandlerStub()
            {
                _handlerFunc = (request, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }

            public DelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
            {
                _handlerFunc = handlerFunc;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return _handlerFunc(request, cancellationToken);
            }
        }
    }
}
