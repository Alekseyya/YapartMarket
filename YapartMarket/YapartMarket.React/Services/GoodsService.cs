using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YapartMarket.Core.DTO.Goods;
using YapartMarket.React.Services.Interfaces;
using YapartMarket.React.ViewModels.Goods;

namespace YapartMarket.React.Services
{
    public class GoodsService : IGoodsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoodsService(IConfiguration configuration, IHttpClientFactory factory)
        {
            _configuration = configuration;
            _httpClient = factory.CreateClient("goodsClient");
        }

        public Task<bool> Confirm(string shipmentId, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItem>> GetOrders(OrderNewViewModel order)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Package(string shipmentId, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Reject(string shipmentId, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveOrder(string shipmentId, List<OrderItem> orderDetails)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Shipment(string shipmentId)
        {
            throw new NotImplementedException();
        }
        //public async Task<List<OrderDetail>> GetOrders(OrderNewViewModel order)
        //{
        //    var orderDetails = new List<OrderDetail>();
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
        //    {
        //        await connection.OpenAsync();
        //        var orderItems = order.OrderNewDataViewModel.Shipments[0].Items;
        //        var groupOrderItems = orderItems.GroupBy(x => x.OfferId).Select(x => new { OfferId = x.Key, Count = x.Count() });

        //        foreach (var orderItem in groupOrderItems)
        //        {
        //            var product = await connection.QueryFirstOrDefaultAsync<Product>("select * from products where sku = @sku", new { sku = orderItem.OfferId });
        //            if (product.Count >= orderItem.Count)
        //                orderDetails.AddRange(Convert(orderItems.Where(x => x.OfferId == orderItem.OfferId).ToList(), ReasonType.Empty));
        //            else
        //            {
        //                var rejectItems = orderItem.Count - product.Count;
        //                var confirmItems = orderItem.Count - rejectItems;
        //                var confirmOrderDetails = orderItems.Where(x => x.OfferId == orderItem.OfferId).Take(confirmItems).ToList();
        //                var rejectOrderDetails = orderItems.Where(x => x.OfferId == orderItem.OfferId).TakeLast(rejectItems).ToList();
        //                orderDetails.AddRange(Convert(confirmOrderDetails.AsReadOnly(), ReasonType.Empty));
        //                orderDetails.AddRange(Convert(rejectOrderDetails.AsReadOnly(), ReasonType.OUT_OF_STOCK));
        //            }
        //        }
        //    }
        //    return orderDetails;
        //}

        //private IReadOnlyList<OrderDetail> Convert(IReadOnlyList<OrderNewShipmentItem> orderNewShipmentItems, ReasonType reasonType)
        //{
        //    var orderDetails = new List<OrderDetail>();
        //    foreach (var orderNewShipmentItem in orderNewShipmentItems)
        //    {
        //        orderDetails.Add(new()
        //        {
        //            OfferId = orderNewShipmentItem.OfferId,
        //            GoodsId = orderNewShipmentItem.GoodsId,
        //            ItemIndex = orderNewShipmentItem.ItemIndex,
        //            ReasonType = reasonType
        //        });
        //    }
        //    return orderDetails.AsReadOnly();
        //}
        //public async Task<int> SaveOrder(string shipmentId, List<OrderDetail> orderDetails)
        //{
        //    int id = default;
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
        //    {
        //        await connection.OpenAsync();
        //        var order = await connection.QueryFirstOrDefaultAsync<GoodsOrder>("select * from goods_order where shipmentId = @shipmentId", new { shipmentid = shipmentId });
        //        if (order != null)
        //        {
        //            var confirmOrders = await connection.QueryAsync<GoodsOrderItem>(
        //                "select * from goods_order_details where orderId = @orderId and reasonType = @reasonType", new
        //                {
        //                    orderId = order.Id,
        //                    reasonType = (int)ReasonType.Empty
        //                });
        //            var rejectOrders = await connection.QueryAsync<GoodsOrderItem>(
        //                "select * from goods_order_details where orderId = @orderId and reasonType = @reasonType", new
        //                {
        //                    orderId = order.Id,
        //                    reasonType = (int)ReasonType.OUT_OF_STOCK
        //                });
        //            var newOrderDetails = new List<OrderDetail>();
        //            if (confirmOrders.IsAny())
        //            {
        //                var newConfirmOrders = orderDetails.Where(x=>x.ReasonType == ReasonType.Empty).Where(x => confirmOrders.All(t =>  t.OfferId != x.OfferId || (t.OfferId == x.OfferId && t.ItemIndex != x.ItemIndex))).ToList();
        //                if (newConfirmOrders.IsAny())
        //                    newOrderDetails.AddRange(newConfirmOrders);
        //            }
        //            else
        //                newOrderDetails.AddRange(orderDetails.Where(x=>x.ReasonType == ReasonType.Empty));
        //            if (rejectOrders.IsAny())
        //            {
        //                var newRejectOrders = orderDetails.Where(x => x.ReasonType == ReasonType.OUT_OF_STOCK).Where(x => rejectOrders.All(t => t.OfferId != x.OfferId || (t.OfferId == x.OfferId && t.ItemIndex != x.ItemIndex))).ToList();
        //                if (newRejectOrders.IsAny())
        //                    newOrderDetails.AddRange(newRejectOrders);
        //            }
        //            else
        //                newOrderDetails.AddRange(orderDetails.Where(x => x.ReasonType == ReasonType.OUT_OF_STOCK));

        //            if (newOrderDetails.IsAny())
        //                await CreateDetails(order.Id, connection, newOrderDetails);
        //            return order.Id;

        //        }
        //        id = await connection.QuerySingleAsync<int>("insert into goods_order(shipmentId) values(@shipmentId);SELECT CAST(SCOPE_IDENTITY() as int)", new
        //        {
        //            shipmentId = shipmentId
        //        });
        //        await CreateDetails(id, connection, orderDetails);
        //    }
        //    return id;
        //}
        //private async Task CreateDetails(int orderId, SqlConnection connection, List<OrderDetail> orderDetails)
        //{
        //    foreach (var orderDetail in orderDetails)
        //    {
        //        await connection.ExecuteAsync("insert into goods_order_details(offerId, orderId, itemIndex, reasonType) values(@offerId, @orderId, @itemIndex, @reasonType)", new
        //        {
        //            offerId = orderDetail.OfferId,
        //            orderId = orderId,
        //            itemIndex = orderDetail.ItemIndex,
        //            reasonType = (int)orderDetail.ReasonType
        //        });
        //    }
        //}
        //public async Task<bool> Reject(string shipmentId, int orderId)
        //{
        //    var orderItems = await OrderItems(orderId, ReasonType.OUT_OF_STOCK);
        //    var order = CreateOrderReject(shipmentId, orderItems);
        //    var isSent = await SendRejectRequest(order);
        //    return isSent;
        //}
        //public async Task<bool> Confirm(string shipmentId, int orderId)
        //{
        //    var orderItems = await OrderItems(orderId, ReasonType.Empty);
        //    var order = CreateOrderConfirm(shipmentId, orderItems);
        //    var isSent = await SendConfirmRequest(order);
        //    return isSent;
        //}
        //public async Task<bool> Package(string shipmentId, int orderId)
        //{
        //    var orderItems = await OrderItems(orderId, ReasonType.Empty);
        //    if (orderItems.IsAny())
        //    {
        //        var package = CreatePackage(shipmentId, orderItems);
        //        var packageDto = new PackageDto();
        //        var orderShipmentsViewModel = package.Data.Shipments.FirstOrDefault();
        //        packageDto.ShipmentId = orderShipmentsViewModel.ShipmentId;
        //        packageDto.OrderId = int.Parse(orderShipmentsViewModel.OrderCode);
        //        packageDto.Items = new();
        //        foreach (var orderItemViewModel in orderShipmentsViewModel.Items)
        //        {
        //            packageDto.Items.Add(new ()
        //            {
        //                ItemIndex = int.Parse(orderItemViewModel.ItemIndex),
        //                BoxIndex = orderItemViewModel.OrderBox.FirstOrDefault().BoxIndex,
        //                BoxCode = orderItemViewModel.OrderBox.FirstOrDefault().BoxCode
        //            });
        //        }
        //        await SavePackage(shipmentId, packageDto);
        //        var isSent = await SendPackageRequest(package);
        //        return isSent;
        //    }
        //    return false;
        //}
        //public async Task<bool> Shipment(string shipmentId)
        //{
        //    var orderShippingRoot = new OrderShippingRoot();
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
        //    {
        //        await connection.OpenAsync();
        //        var packingItems = await connection.QueryAsync<PackageItem>("select * from goods_packing_items where shipmentId = @shipmentId",
        //            new { shipmentId = shipmentId });
        //        orderShippingRoot.Meta = new();

        //        var orderShipping = new OrderShipment();
        //        orderShipping.ShipmentId = shipmentId;
        //        orderShipping.Shipping = new()
        //        {
        //            ShippingDate = DateTime.Now.ToString("s")
        //        };
        //        var boxes = new List<Box>();
        //        foreach (var box in packingItems.Select(x => new { x.BoxIndex, x.BoxCode }))
        //        {
        //            boxes.Add(new()
        //            {
        //                BoxIndex = box.BoxIndex,
        //                BoxCode = box.BoxCode,
        //            });
        //        }
        //        orderShipping.Boxes = boxes;
        //        var shipments = new List<OrderShipment>();
        //        shipments.Add(orderShipping);
        //        orderShippingRoot.Shipping = new()
        //        {
        //            Token = _configuration.GetSection("goodsTestToken").Value,
        //            Shipments = shipments
        //        };
        //    }

        //    var isResponse = false;
        //    if (orderShippingRoot.Shipping.Shipments.IsAny())
        //    {
        //        isResponse = await SendShippingRequest(orderShippingRoot);
        //    }
        //    return isResponse;
        //}
        //private async Task SavePackage(string shipmentId, PackageDto packageDto)
        //{
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
        //    {
        //        await connection.OpenAsync();
        //        var order = await connection.QueryFirstOrDefaultAsync<GoodsOrder>(
        //            "select * from goods_order where shipmentId = @shipmentId", new { shipmentId = shipmentId });
        //        if (order != null)
        //        {
        //            var package = await connection.QueryFirstOrDefaultAsync<Package>(
        //                "select * from goods_packing where shipmentId = @shipmentId",
        //                new
        //                {
        //                    shipmentId = shipmentId,
        //                });
        //            if (package == null)
        //            {
        //                int packingId = default;
        //                packingId = await connection.ExecuteAsync("insert into goods_packing(orderId, shipmentId) values(@orderId, @shipmentId);SELECT CAST(SCOPE_IDENTITY() as int)", new
        //                {
        //                    orderId = order.Id,
        //                    shipmentId = shipmentId
        //                });
        //                foreach (var packageDtoItem in packageDto.Items)
        //                {
        //                    await connection.ExecuteAsync("insert into goods_packing_items(itemIndex, boxIndex, boxCode, digitalMarks, shipmentId, packingId) " +
        //                                                  "values(@itemIndex, @boxIndex, @boxCode, @digitalMarks, @shipmentId, @packingId)", new
        //                    {
        //                        itemIndex = packageDtoItem.ItemIndex,
        //                        boxIndex = packageDtoItem.BoxIndex,
        //                        boxCode = packageDtoItem.BoxCode,
        //                        shipmentId = shipmentId,
        //                        digitalMarks = packageDtoItem.DigitalMarks,
        //                        packingId = packingId
        //                    });
        //                }

        //            }
        //            if (package != null)
        //            {
        //                var packages = await connection.QueryAsync<PackageItem>(
        //                    "select * from goods_packing_items where shipmentId = @shipmentId",
        //                    new
        //                    {
        //                        shipmentId = shipmentId,
        //                    });
        //                var newPackageItems = packageDto.Items.Where(x => packages.Any(t => t.ShipmentId != shipmentId));
        //                var updatePackageItems = packageDto.Items.Where(x =>
        //                    packages.Any(t => t.ShipmentId == shipmentId && t.ItemIndex != x.ItemIndex || t.BoxCode != x.BoxCode || t.BoxIndex != x.BoxIndex));
        //                if (newPackageItems.IsAny())
        //                {
        //                    foreach (var newPackageItem in newPackageItems)
        //                    {
        //                        await connection.ExecuteAsync("insert into goods_packing_items(itemIndex, boxIndex, boxCode, digitalMarks, shipmentId, packingId) " +
        //                                                      "values(@itemIndex, @boxIndex, @boxCode, @digitalMarks, @shipmentId, @packingId)", new
        //                        {
        //                            item_index = newPackageItem.ItemIndex,
        //                            box_index = newPackageItem.BoxIndex,
        //                            box_code = newPackageItem.BoxCode,
        //                            shipment_id = shipmentId,
        //                            digital_marks = newPackageItem.DigitalMarks
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //private async Task<List<GoodsOrderItem>> OrderItems(int orderId, ReasonType reasonType)
        //{
        //    var goodsOrderItems = new List<GoodsOrderItem>();
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
        //    {
        //        await connection.OpenAsync();
        //        goodsOrderItems = (List<GoodsOrderItem>)await connection.QueryAsync<GoodsOrderItem>("select * from goods_order_details where orderId = @orderId and reasonType = @reasonType", new { orderId = orderId, reasonType = (int)reasonType });
        //    }
        //    return goodsOrderItems;
        //}
        //private OrderViewModel CreatePackage(string shipmentId, List<GoodsOrderItem> orderItems) // 
        //{
        //    var orderId = orderItems.First().OrderId;
        //    var orderItemsViewModels = new List<OrderItemViewModel>();
        //    var boxIndex = 1;
        //    foreach (var items in orderItems)
        //    {
        //        orderItemsViewModels.Add(new()
        //        {
        //            ItemIndex = items.ItemIndex,
        //            OrderBox = new()
        //            {
        //                new OrderBox()
        //                {
        //                    BoxIndex = boxIndex,
        //                    BoxCode = $"3897*{orderId}*{boxIndex}"
        //                }
        //            },
        //        });
        //        boxIndex++;
        //    }
        //    var orderShipments = new List<OrderShipmentViewModel>();
        //    orderShipments.Add(new()
        //    {
        //        ShipmentId = shipmentId,
        //        OrderCode = orderId.ToString(),
        //        Items = orderItemsViewModels
        //    });

        //    var package = new OrderViewModel()
        //    {
        //        Data = new()
        //        {
        //            Token = _configuration.GetSection("goodsTestToken").Value,
        //            Shipments = orderShipments
        //        },
        //        Meta = new()
        //    };
        //    return package;
        //}
        //private OrderViewModel CreateOrderConfirm(string shipmentId, List<GoodsOrderItem> orderItems)
        //{
        //    var orderId = orderItems.First().OrderId;
        //    var orderItemViewModels = new List<OrderItemViewModel>();
        //    foreach (var item in orderItems)
        //    {
        //        orderItemViewModels.Add(new()
        //        {
        //            ItemIndex = item.ItemIndex,
        //            OfferId = item.OfferId
        //        });
        //    }
        //    var orderShipments = new List<OrderShipmentViewModel>();

        //    orderShipments.Add(new()
        //    {
        //        ShipmentId = shipmentId,
        //        OrderCode = orderId.ToString(),
        //        Items = orderItemViewModels
        //    });
        //    var order = new OrderViewModel()
        //    {
        //        Data = new()
        //        {
        //            Token = _configuration.GetSection("goodsTestToken").Value,
        //            Shipments = orderShipments
        //        },
        //        Meta = new()
        //    };
        //    return order;
        //}
        //private OrderViewModel CreateOrderReject(string shipmentId, List<GoodsOrderItem> orderItems)
        //{
        //    var orderItemViewModels = new List<OrderItemViewModel>();
        //    foreach (var item in orderItems)
        //    {
        //        orderItemViewModels.Add(new()
        //        {
        //            ItemIndex = item.ItemIndex,
        //            OfferId = item.OfferId
        //        });
        //    }
        //    var orderShipments = new List<OrderShipmentViewModel>();
        //    orderShipments.Add(new()
        //    {
        //        ShipmentId = shipmentId,
        //        Items = orderItemViewModels
        //    });
        //    var order = new OrderViewModel()
        //    {
        //        Data = new()
        //        {
        //            Token = _configuration.GetSection("goodsTestToken").Value,
        //            Shipments = orderShipments
        //        },
        //        Reason = new()
        //        {
        //            Type = Enum.GetName(typeof(ReasonType), ReasonType.OUT_OF_STOCK)
        //        },
        //        Meta = new()
        //    };
        //    return order;
        //}
        //private async Task<SuccessfulResponse> SendRequest(string url, string body)
        //{
        //    var content = new StringContent(body, Encoding.UTF8, "application/json");
        //    var result = await _httpClient.PostAsync(url, content);
        //    string resultContent = await result.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<SuccessfulResponse>(resultContent);
        //}
        //private async Task<bool> SendConfirmRequest(OrderViewModel order)
        //{
        //    var body = JsonConvert.SerializeObject(order);
        //    var url = "/api/market/v1/orderService/order/confirm";
        //    var response = await SendRequest(url, body);
        //    if (response.Success == 1)
        //        return true;
        //    return false;
        //}
        //private async Task<bool> SendShippingRequest(OrderShippingRoot shipping)
        //{
        //    var body = JsonConvert.SerializeObject(shipping);
        //    var url = "/api/market/v1/orderService/order/shipping";
        //    var response = await SendRequest(url, body);
        //    if (response.Success == 1)
        //        return true;
        //    return false;
        //}
        //private async Task<bool> SendPackageRequest(OrderViewModel order)
        //{
        //    var body = JsonConvert.SerializeObject(order);
        //    var url = "/api/market/v1/orderService/order/packing";
        //    var response = await SendRequest(url, body);
        //    if (response.Success == 1)
        //        return true;
        //    return false;
        //}
        //private async Task<bool> SendRejectRequest(OrderViewModel order)
        //{
        //    var body = JsonConvert.SerializeObject(order);
        //    var url = "/api/market/v1/orderService/order/reject";
        //    var response = await SendRequest(url, body);
        //    if (response.Success == 1)
        //        return true;
        //    return false;
        //}
    }
}
