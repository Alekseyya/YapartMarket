using Dapper;
using Npgsql;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO.Goods;
using YapartMarket.Core.Extensions;
using YapartMarket.WebApi.Services.Interfaces;
using YapartMarket.WebApi.ViewModel.Goods;
using YapartMarket.WebApi.ViewModel.Goods.Cancel;
using YapartMarket.WebApi.ViewModel.Goods.Confirm;
using YapartMarket.WebApi.ViewModel.Goods.Packing;
using YapartMarket.WebApi.ViewModel.Goods.Reject;
using YapartMarket.WebApi.ViewModel.Goods.Shipping;

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
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var shipmentId = orderViewModel.OrderNewDataViewModel.Shipments[0].ShipmentId;
                var sql = @"select * from goods_order o inner join goods_orderItem od on o.Id = od.orderId where o.shipmentId = @shipmentId";
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
        public async Task<SuccessResult> PackingAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
        {
            var items = new List<ViewModel.Goods.Packing.Item>();
            var boxIndex = 1;
            foreach (var item in orderItems)
            {
                items.Add(new()
                {
                    itemIndex = item.ItemIndex,
                    boxes = new List<ViewModel.Goods.Packing.Box>()
                    {
                       new()
                       {
                           boxIndex = boxIndex,
                           boxCode = "3897" + shipmentId + boxIndex
                       }
                    }
                });
                boxIndex++;
            }
            var package = new ViewModel.Goods.Packing.Root()
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
                },
                meta = new()
            };
            var json = System.Text.Json.JsonSerializer.Serialize(package);
            return await SendPackageRequestAsync(json);
        }
        public async Task<SuccessResult> RejectAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
        {
            var items = new List<ViewModel.Goods.Reject.Item>();
            foreach (var item in orderItems)
            {
                items.Add(new()
                {
                    offerId = shipmentId,
                    itemIndex = item.ItemIndex,
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
            return await SendRejectRequestAsync(json);
        }
        public async Task<SuccessResult> CancelAsync(Cancel cancelOrder)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var canceledOrder = cancelOrder.data.shipments.First();
                    var order = await connection.QueryFirstAsync<Order>(@"select * from goods_order where shipmentId = @shipmentid", new { shipmentId = canceledOrder.shipmentId });
                    if (order != null)
                    {
                        var cancelDateTime = DateTime.Now;
                        var orderId = order.Id;
                        var updateSql = @"update goods_orderItem set cancel = true where orderId = @orderId and itemIndex = @itemIndex and goodsId = @goodsId
and cancelDateTime = @cancelDateTime;";
                        var canceledOrderTmp = canceledOrder.items.Select(x => new { itemIndex = x.itemIndex, goodsId = x.goodsId, cancelDateTime = cancelDateTime });
                        foreach (var cancelOrderTmp in canceledOrderTmp)
                        {
                            var orderItem = await connection.QueryFirstAsync<OrderItem>(@"select * from goods_orderItem where itemIndex = @itemIndex and goodsId = @goodsId", cancelOrderTmp).ConfigureAwait(false);
                            if (orderItem != null)
                                await connection.ExecuteAsync(updateSql, new { orderId = orderId, itemIndex = cancelOrderTmp.itemIndex, goodsId = cancelOrderTmp.goodsId }).ConfigureAwait(false);
                        }
                        await transaction.CommitAsync().ConfigureAwait(false);
                    }
                }
            }
            return SuccessResult.Success;
        }
        /// <inheritdoc/>
        public async Task SaveOrderAsync(OrderNewViewModel orderViewModel)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {

                    var shipment = orderViewModel.OrderNewDataViewModel.Shipments.First();
                    var order = await connection.QueryAsync<Order>(@"select * from goods_order where shipmentId = @shipmentid",
                        new
                        {
                            shipmentId = shipment.ShipmentId
                        }, transaction);
                    if (!order.IsAny())
                    {
                        var shipmentDate = DateTime.Parse(shipment.ShipmentDate);
                        var orderId = Guid.NewGuid();
                        var addOrderSql = @$"insert into goods_order(id, shipmentId, shipmentDate) values(@orderId, @shipmentid, @shipmentdate)";
                        await connection.ExecuteAsync(addOrderSql, new
                        {
                            orderId = orderId,
                            shipmentId = shipment.ShipmentId,
                            shipmentDate = shipmentDate
                        }, transaction);
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
                        var addOrderDetailSql = $@"insert into goods_orderItem(id, orderId, itemIndex, goodsId, offerId, itemName, price, finalPrice, quantity) 
values(@id, @orderId, @itemIndex, @goodsId, @offerId, @itemName, @price, @finalPrice, @quantity)";
                        await connection.ExecuteAsync(addOrderDetailSql, orderDetails, transaction);
                        transaction.Commit();
                    }
                }

            }
        }

        /// <inheritdoc />
        public async Task<SuccessResult> ProcessConfirmOrRejectAsync(string? shipmentId)
    {
        var errors = new List<string>();
        using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
        {
            await connection.OpenAsync();
            var order = await connection.QueryFirstAsync<Order>(@"select * from goods_order where shipmentId = @shipmentid", new { shipmentId = shipmentId });
            if (order != null)
            {
                var orderId = order.Id;
                var orderItems = await connection.QueryAsync<OrderItem>(@"select * from goods_orderItem where orderId = @orderId", new { orderId });
                var goodsId = orderItems!.Select(x => x.GoodsId).ToList();
                var confirmProducts = await connection.QueryAsync<Core.Models.Azure.Product>(@"select * from products where goodsId IN @goodsId", new { goodsId = goodsId });
                var confirmItems = orderItems.Where(x => confirmProducts.Any(t => t.GoodsId!.Value == x.GoodsId));
                var rejectItems = orderItems.Where(x => !confirmProducts.Any(t => t.GoodsId!.Value == x.GoodsId));
                if (confirmItems.Any() && rejectItems.Any())
                {
                    var confirmResult = await ConfirmAsync(shipmentId, confirmItems.ToList());
                    var rejectResult = await RejectAsync(shipmentId, rejectItems.ToList());
                    var packingResult = await PackingAsync(shipmentId, confirmItems.ToList());
                    var shipmentResult = await ShippingAsync(shipmentId, confirmItems.ToList());
                    var result = SuccessResult.Combine(confirmResult, rejectResult, packingResult, shipmentResult);
                    if (!result.Succeeded)
                        return result;
                }
                if (confirmItems.Any() && !rejectItems.Any())
                {
                    var confirmResult = await ConfirmAsync(shipmentId, confirmItems.ToList());
                    var packingResult = await PackingAsync(shipmentId, confirmItems.ToList());
                    var shipmentResult = await ShippingAsync(shipmentId, confirmItems.ToList());
                    var result = SuccessResult.Combine(confirmResult, packingResult, shipmentResult);
                    if (!result.Succeeded)
                        return result;
                }
                if (!confirmItems.Any() && rejectItems.Any())
                {
                    var rejectResult = await RejectAsync(shipmentId, rejectItems.ToList());
                    var result = SuccessResult.Combine(rejectResult);
                    if (!result.Succeeded)
                        return result;
                }
            }
        }
        return SuccessResult.Success;
    }
    /// <inheritdoc />
    public async Task<SuccessResult> ShippingAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
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
        return await SendShippingRequestAsync(json);
    }
    public async Task<SuccessResult> ConfirmAsync(string shipmentId, IReadOnlyList<OrderItem> orderItems)
    {
        var items = new List<ViewModel.Goods.Confirm.Item>();
        foreach (var orderItem in orderItems)
        {
            items.Add(new()
            {
                itemIndex = orderItem.ItemIndex,
                offerId = orderItem.OfferId
            });
        }
        var confirm = new Confirm()
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
        var json = JsonSerializer.Serialize(confirm);
        var result = await SendConfirmRequestAsync(json);
        return result;
    }
    private async Task<string> SendRequestAsync(string url, string body)
    {
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsync(url, content);
        string resultContent = await result.Content.ReadAsStringAsync();
        return resultContent;
    }
    private async Task<SuccessResult> SendConfirmRequestAsync(string json)
    {
        var url = "/api/market/v1/orderService/order/confirm";
        var response = await SendRequestAsync(url, json);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        var confirmResponse = JsonSerializer.Deserialize<ConfirmResponse>(response, jsonSerializerOptions);
        if (confirmResponse.success == 0)
            return new SuccessResult(confirmResponse.error.message);
        return SuccessResult.Success;
    }
    private async Task<SuccessResult> SendShippingRequestAsync(string json)
    {
        var url = "/api/market/v1/orderService/order/shipping";
        var response = await SendRequestAsync(url, json);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        var packingResponse = JsonSerializer.Deserialize<ShippingResponse>(response, jsonSerializerOptions);
        if (packingResponse.success == 0)
            return new SuccessResult(packingResponse.error.Select(x => x.message).ToList());
        return SuccessResult.Success;
    }
    private async Task<SuccessResult> SendPackageRequestAsync(string json)
    {
        var url = "/api/market/v1/orderService/order/packing";
        var response = await SendRequestAsync(url, json);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        var packagedResponse = JsonSerializer.Deserialize<PackingResponse>(response, jsonSerializerOptions);
        if (packagedResponse.success == 0)
            return new SuccessResult(packagedResponse.error.Select(x => x.message).ToList());
        return SuccessResult.Success;
    }
    private async Task<SuccessResult> SendRejectRequestAsync(string json)
    {
        var url = "/api/market/v1/orderService/order/reject";
        var response = await SendRequestAsync(url, json);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        var rejectResponse = JsonSerializer.Deserialize<RejectResponse>(response, jsonSerializerOptions);
        if (rejectResponse.success == 0)
            return new SuccessResult(rejectResponse.error.Select(x => x.message).ToList());
        return SuccessResult.Success;
    }
}
}
