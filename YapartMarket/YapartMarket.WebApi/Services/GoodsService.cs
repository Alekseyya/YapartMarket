using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Text;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO.Goods;
using YapartMarket.Core.Extensions;
using YapartMarket.WebApi.Services.Interfaces;
using YapartMarket.WebApi.ViewModel.Goods;
using YapartMarket.WebApi.ViewModel.Goods.Cancel;

namespace YapartMarket.WebApi.Services
{
    public sealed class GoodsService : IGoodsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoodsService(IConfiguration configuration, IHttpClientFactory factory)
        {
            _configuration = configuration;
            _httpClient = factory.CreateClient("goodsClient");
        }
        public async Task<Order?> GetOrderAsync(OrderNewViewModel orderViewModel)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSqlConnectionString")))
            {
                await connection.OpenAsync();
                var shipmentId = orderViewModel.OrderNewDataViewModel.Shipments[0].ShipmentId;
                var sql = @"select * from public.order o inner join public.orderItem od on o.Id = od.orderId where o.shipmentId = @shipmentId";
                var orderDictionary = new Dictionary<Guid, Order>();
                var orderResult = await connection.QueryAsync<Order, OrderItem, Order>(sql, (order, orderDetail) =>
                {
                    if (!orderDictionary.TryGetValue(order.Id, out Order docEntry))
                    {
                        docEntry = order;
                        docEntry.OrderDetails = new List<OrderItem>();
                        orderDictionary.Add(docEntry.Id, docEntry);
                    }

                    if (orderDetail != null) docEntry.OrderDetails.Add(orderDetail);
                    docEntry.OrderDetails = docEntry.OrderDetails.Distinct().ToList();

                    return docEntry;
                }, splitOn: "orderid", param: new { shipmentId });

                return orderResult.FirstOrDefault();
            }
        }
        public async Task PackageAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
        {
            var items = new List<ViewModel.Goods.Packing.Item>();
            var boxIndex = 1;
            foreach (var item in orderItems)
            {
                items.Add(new()
                {
                    itemIndex = item.IntemIndex,
                    boxes = new List<ViewModel.Goods.Packing.Box>()
                    {
                       new ViewModel.Goods.Packing.Box()
                       {
                           boxIndex = boxIndex,
                           boxCode = "3897" + shipmentId + boxIndex
                       }
                    }
                });
                boxIndex++;
            }
            var package  = new ViewModel.Goods.Packing.Root()
            {
                data = new ViewModel.Goods.Packing.Data()
                {
                    token = _configuration.GetSection("goodsTestToken").Value,
                    shipments = new List<ViewModel.Goods.Packing.Shipment>()
                    {
                        new()
                        {
                            shipmentId = shipmentId,
                            orderCode = shipmentId,
                            items = items
                        }
                    }
                }
            };
            var json = System.Text.Json.JsonSerializer.Serialize(package);
            await SendPackageRequestAsync(json);
        }
        public async Task RejectAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
        {
            var items = new List<ViewModel.Goods.Reject.Item>();
            foreach (var item in orderItems)
            {
                items.Add(new()
                {
                    offerId = shipmentId,
                    itemIndex = item.IntemIndex,
                });
            }
            var reject = new ViewModel.Goods.Reject.Root()
            {
                data = new()
                {
                   shipments = new List<ViewModel.Goods.Reject.Shipment>()
                   {
                       new()
                       {
                           shipmentId = shipmentId,
                           items = items
                       }
                   },
                   reason = new()
                   {
                       type = Enum.GetName(typeof(ReasonType), ReasonType.OUT_OF_STOCK)
                   },
                   token = _configuration.GetSection("goodsTestToken").Value
                },
                meta = new()
            };
            var json = System.Text.Json.JsonSerializer.Serialize(reject);
            await SendRejectRequestAsync(json);
        }
        public async Task CancelAsync(Cancel cancelOrder)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSqlConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    var canceledOrder = cancelOrder.data.shipments.First();
                    var order = await connection.QueryFirstAsync<Order>(@"select * from ""order"" where ""shipmentId"" = @shipmentid", new { shipmentId = canceledOrder.shipmentId });
                    if (order != null)
                    {
                        var cancelDateTime = DateTime.Now;
                        var orderId = order.Id;
                        var updateSql = @"update ""orderItem"" set ""cancel"" = true where ""orderId"" = @orderId and ""itemIndex"" = @itemIndex and ""goodsId"" = @goodsId
and ""cancelDateTime"" = @cancelDateTime;";
                        var canceledOrderTmp = canceledOrder.items.Select(x=> new { itemIndex = x.itemIndex, goodsId = x.goodsId, cancelDateTime = cancelDateTime });
                        foreach (var cancelOrderTmp in canceledOrderTmp)
                        {
                            var orderItem = await connection.QueryFirstAsync<OrderItem>(@"select * from ""orderItem"" where ""itemIndex"" = @itemIndex and ""goodsId"" = @goodsId", cancelOrderTmp);
                            if (orderItem != null)
                                await connection.ExecuteAsync(updateSql, new { orderId = orderId, itemIndex = cancelOrderTmp.itemIndex, goodsId = cancelOrderTmp.goodsId });
                        }
                        transaction.Commit();
                    }
                }
            }
        }
        public async Task SaveOrderAsync(OrderNewViewModel orderViewModel)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSqlConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {

                    var shipment = orderViewModel.OrderNewDataViewModel.Shipments.First();
                    var order = await connection.QueryAsync<Order>(@"select * from ""order"" where ""shipmentId"" = @shipmentid", new { shipmentId = shipment.ShipmentId });
                    if (!order.IsAny())
                    {
                        var shipmentDate = DateTime.Parse(shipment.ShipmentDate);
                        var orderId = Guid.NewGuid();
                        var addOrderSql = @$"insert into ""order""(""id"", ""shipmentId"", ""shipmentDate"") values(@orderId, @shipmentid, @shipmentdate)";
                        await connection.ExecuteAsync(addOrderSql, new
                        {
                            orderId = orderId,
                            shipmentId = shipment.ShipmentId,
                            shipmentDate = shipmentDate
                        });
                        var orderDetails = shipment.Items.Select(x => new
                        {
                            id = Guid.NewGuid(),
                            orderId = orderId,
                            itemIndex = int.Parse(x.ItemIndex),
                            goodsId = x.GoodsId,
                            offerId = x.OfferId,
                            itemName = x.ItemName,
                            price = x.Price,
                            finalPrice = x.FinalPrice,
                            quantity = x.Quantity
                        });
                        var addOrderDetailSql = $@"insert into ""orderItem""(""id"", ""orderId"", ""itemIndex"", ""goodsId"", ""offerId"", ""itemName"", ""price"", ""finalPrice"", ""quantity"") 
values(@id, @orderId, @itemIndex, @goodsId, @offerId, @itemName, @price, @finalPrice, @quantity)";
                        await connection.ExecuteAsync(addOrderDetailSql, orderDetails);
                        transaction.Commit();
                    }
                }
            }
        }

        public async Task ProcessConfirmOrRejectAsync(string? shipmentId)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSqlConnectionString")))
            {
                await connection.OpenAsync();
                var order = await connection.QueryFirstAsync<Order>(@"select * from ""order"" where ""shipmentId"" = @shipmentid", new { shipmentId = shipmentId });
                if (order != null)
                {
                    var orderId = order.Id;
                    var orderItems = await connection.QueryAsync<OrderItem>(@"select * from ""orderItem"" where ""orderId"" = @orderId", new { orderId });
                    var items = orderItems.Select(x => x.OfferId).ToList();
                    var confirmProducts = await connection.QueryAsync<Product>(@"select * from ""products"" where ""sku"" IN @sku", new { sku = items });
                    var confirmItems = orderItems.Where(x => confirmProducts.Any(t => t.Sku.ToLower() == x.OfferId.ToLower()));
                    var rejectItems = orderItems.Where(x => !confirmProducts.Any(t => t.Sku.ToLower() == x.OfferId.ToLower()));
                    if(confirmItems.Any() && rejectItems.Any())
                    {
                        await ConfirmAsync(shipmentId, confirmItems.ToList());
                        await RejectAsync(shipmentId, rejectItems.ToList());
                        await PackageAsync(shipmentId, confirmItems.ToList());
                        await ShipmentAsync(shipmentId, confirmItems.ToList());
                    }
                    if (confirmItems.Any() && !rejectItems.Any())
                    {
                        await ConfirmAsync(shipmentId, confirmItems.ToList());
                        await PackageAsync(shipmentId, confirmItems.ToList());
                        await ShipmentAsync(shipmentId, confirmItems.ToList());
                    }
                    if(!confirmItems.Any() && rejectItems.Any())
                    {
                        await RejectAsync(shipmentId, rejectItems.ToList());
                    }
                }
            }
        }
        public async Task ShipmentAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
        {
            var date = DateTime.UtcNow.AddDays(1);
            var boxes = new List<ViewModel.Goods.Shipping.Box>();
            var boxIndex = 1;
            foreach (var item in orderItems)
            {
                boxes.Add(
                    new ViewModel.Goods.Shipping.Box()
                    {
                        boxIndex = boxIndex,
                        boxCode = "3897" + shipmentId + boxIndex
                    });
                boxIndex++;
            }
            var shipping = new ViewModel.Goods.Shipping.Root()
            {
                data = new()
                {
                    token = _configuration.GetSection("goodsTestToken").Value,
                    shipments = new List<ViewModel.Goods.Shipping.Shipment>()
                    {
                        new()
                        {
                            shipmentId = shipmentId,
                            boxes = boxes,
                            shipping = new()
                            {
                                shippingDate = date
                            }
                        }
                    }
                },
                meta = new()
            };
            var json = System.Text.Json.JsonSerializer.Serialize(shipping);
            await SendShippingRequestAsync(json);
        }
        public async Task ConfirmAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
        {
            var items = new List<ViewModel.Goods.Confirm.Item>();
            foreach (var orderItem in orderItems)
            {
                items.Add(new()
                {
                    itemIndex = orderItem.IntemIndex,
                    offerId = orderItem.OfferId
                });
            }
            var confirm = new ViewModel.Goods.Confirm.Confirm()
            {
                data = new()
                {
                    token = _configuration.GetSection("goodsTestToken").Value,
                    shipments = new List<ViewModel.Goods.Confirm.Shipment>()
                    {
                        new()
                        {
                            shipmentId = shipmentId,
                            orderCode = shipmentId,
                            items = items
                        }
                    }
                },
                meta = new()
            };
            var json = System.Text.Json.JsonSerializer.Serialize(confirm);
            await SendConfirmRequestAsync(json);
        }
        public Task<bool> Confirm(string shipmentId, int orderId)
        {
            throw new NotImplementedException();
        }
        private async Task<SuccessfulResponse> SendRequestAsync(string url, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(url, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SuccessfulResponse>(resultContent);
        }
        private async Task SendConfirmRequestAsync(string json)
        {
            var url = "/api/market/v1/orderService/order/confirm";
            var response = await SendRequestAsync(url, json);
        }
        private async Task SendShippingRequestAsync(string json)
        {
            var url = "/api/market/v1/orderService/order/shipping";
            var response = await SendRequestAsync(url, json);
        }
        private async Task SendPackageRequestAsync(string json)
        {
            var url = "/api/market/v1/orderService/order/packing";
            var response = await SendRequestAsync(url, json);
        }
        private async Task SendRejectRequestAsync(string json)
        {
            var url = "/api/market/v1/orderService/order/reject";
            var response = await SendRequestAsync(url, json);
        }
    }
}
