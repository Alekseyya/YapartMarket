using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO.Goods;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Core.Models.Azure.Goods;
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
        public async Task<List<OrderDetailDto>> GetOrders(OrderNewViewModel order)
        {
            var orderDetails = new List<OrderDetailDto>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var orderItems = order.OrderNewDataViewModel.Shipments[0].Items;
                var groupOrderItems = orderItems.GroupBy(x => x.OfferId).Select(x => new { OfferId = x.Key, Count = x.Count() });
                
                foreach (var orderItem in groupOrderItems)
                {
                    var product = await connection.QueryFirstOrDefaultAsync<Product>("select * from products where sku = @sku", new { sku = orderItem.OfferId });
                    if (product.Count >= orderItem.Count)
                        orderDetails.AddRange(Convert(orderItems.Where(x => x.OfferId == orderItem.OfferId).ToList(), ReasonType.Empty));
                    else
                    {
                        var rejectItems = orderItem.Count - product.Count;
                        var confirmItems = orderItem.Count - rejectItems;
                        var confirmOrderDetails = orderItems.Where(x => x.OfferId == orderItem.OfferId).Take(confirmItems).ToList();
                        var rejectOrderDetails = orderItems.Where(x => x.OfferId == orderItem.OfferId).TakeLast(confirmItems).ToList();
                        orderDetails.AddRange(Convert(confirmOrderDetails.AsReadOnly(), ReasonType.Empty));
                        orderDetails.AddRange(Convert(rejectOrderDetails.AsReadOnly(), ReasonType.OUT_OF_STOCK));
                    }
                }
            }

            return orderDetails;
        }

        private IReadOnlyList<OrderDetailDto> Convert(IReadOnlyList<OrderNewShipmentItem> orderNewShipmentItems, ReasonType reasonType)
        {
            var orderDetails = new List<OrderDetailDto>();
            foreach (var orderNewShipmentItem in orderNewShipmentItems)
            {
                orderDetails.Add(new()
                {
                    OfferId = orderNewShipmentItem.OfferId,
                    GoodsId = orderNewShipmentItem.GoodsId,
                    ItemIndex = orderNewShipmentItem.ItemIndex,
                    ReasonType = reasonType
                });
            }
            return orderDetails.AsReadOnly();
        }
        public async Task<int> SaveOrder(string shipmentId, List<OrderDetailDto> orderDetails)
        {
            int id = default;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var order = await connection.QueryFirstOrDefaultAsync<GoodsOrder>("select * from goods_order where shipmentId = @shipmentId", new { shipmentId = shipmentId });
                if (order != null)
                {
                    var confirmOrders = await connection.QueryAsync<GoodsOrderItem>(
                        "select * from goods_order_details where order_id = @order_id and reason_type = @reason_type", new
                        {
                            order_id = order.Id,
                            reason_type = (int)ReasonType.Empty
                        });
                    var rejectOrders = await connection.QueryAsync<GoodsOrderItem>(
                        "select * from goods_order_details where order_id = @order_id and reason_type = @reason_type", new
                        {
                            order_id = order.Id,
                            reason_type = (int)ReasonType.OUT_OF_STOCK
                        });
                    var newConfirmOrders = orderDetails.Where(x => confirmOrders.All(t => t.OfferId != x.OfferId || (t.OfferId == x.OfferId && t.ItemIndex != x.ItemIndex))).ToList();
                    //var updateConfirmOrders = confirmOrders.Where(x=> confirmOrders.All(t=>t.OfferId.Equals(x.OfferId) && t.ItemIndex == x.ItemIndex));
                    var newRejectOrders = orderDetails.Where(x => rejectOrders.All(t => t.OfferId != x.OfferId || (t.OfferId == x.OfferId && t.ItemIndex != x.ItemIndex)));
                    var newOrderDetails = new List<OrderDetailDto>();
                    newOrderDetails.AddRange(newConfirmOrders);
                    newOrderDetails.AddRange(newRejectOrders);
                    if (newOrderDetails.IsAny())
                        await CreateDetails(id, connection, newOrderDetails);
                   
                }
                if (order == null)
                {
                    id = await connection.QuerySingleAsync<int>("insert into goods_order(shipmentId,create) values(@shipmentId);SELECT CAST(SCOPE_IDENTITY() as int)", new
                    {
                        shipmentId = shipmentId, 
                        create = DateTime.UtcNow
                    });
                    await CreateDetails(id, connection, orderDetails);
                }
            }
            return id;
        }
        private async Task CreateDetails(int orderId, SqlConnection connection, List<OrderDetailDto> orderDetails)
        {
            foreach (var orderDetail in orderDetails)
            {
                await connection.ExecuteAsync("insert into goods_order_details(offerId, orderId, item_index, reason_type) values(@offerId, @orderId, @item_index, @reason_type)", new
                {
                    offerId = orderDetail.OfferId,
                    orderId = orderId,
                    item_index = orderDetail.ItemIndex,
                    reason_type = (int)orderDetail.ReasonType,
                    create = DateTime.UtcNow
                });
            }
        }
        public async Task<bool> Reject(string shipmentId, int orderId)
        {
            var orderItems = await OrderItems(orderId, ReasonType.OUT_OF_STOCK);
            var order = CreateOrderReject(shipmentId, orderItems);
            var isSent = await SendRejectRequest(order);
            return isSent;
        }
        public async Task<bool> Confirm(string shipmentId, int orderId)
        {
            var orderItems = await OrderItems(orderId, ReasonType.Empty);
            var order = CreateOrderConfirm(shipmentId, orderItems);
            var isSent = await SendConfirmRequest(order);
            return isSent;
        }
        public async Task<bool> Package(string shipmentId, int orderId)
        {
            var orderItems = await OrderItems(orderId, ReasonType.Empty);
            if (orderItems.IsAny())
            {
                var package = CreatePackage(shipmentId, orderItems);
                var isSent = await SendPackageRequest(package);
                return isSent;
            }
            return false;
        }

        //private async Task SavePackage(string shipmentId, )
        //{
        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
        //    {
        //        await connection.OpenAsync();
        //        var order = await connection.QueryFirstOrDefaultAsync<GoodsOrder>(
        //            "select * from goods_packing where shipmentId = @shipmentId", new { shipmentId = shipmentId });
        //        if (order != null)
        //        {
        //            var packages = await connection.QueryAsync<Package>(
        //                "select * from goods_packing where order_id = @order_id",
        //                new
        //                {
        //                    order_id = order.Id,
        //                });
        //            //todo переделать
        //            var newPackages = packages.Where(x => rejectOrders.All(t =>
        //                t.OfferId != x.OfferId || (t.OfferId == x.OfferId && t.ItemIndex != x.ItemIndex)));
        //            var newOrderDetails = new List<OrderDetailDto>();
        //            newOrderDetails.AddRange(newConfirmOrders);
        //            newOrderDetails.AddRange(newPackages);
        //            if (newOrderDetails.IsAny())
        //                await CreateDetails(id, connection, newOrderDetails);

        //        }
        //    }
        //}

        private async Task<List<GoodsOrderItem>> OrderItems(int orderId, ReasonType reasonType)
        {
            var goodsOrderItems = new List<GoodsOrderItem>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                goodsOrderItems = (List<GoodsOrderItem>)await connection.QueryAsync<GoodsOrderItem>("select * from goods_order_details where orderId = @orderId and reason_type = @reason_type", new { orderId = orderId, reason_type = (int)reasonType });
            }
            return goodsOrderItems;
        }
        private OrderViewModel CreatePackage(string shipmentId, List<GoodsOrderItem> orderItems) // 
        {
            var orderId = orderItems.First().OrderId;
            var orderItemsViewModels = new List<OrderItemViewModel>();
            var boxIndex = 1;
            foreach (var items in orderItems)
            {
                orderItemsViewModels.Add(new()
                {
                    ItemIndex = items.ItemIndex,
                    OrderBox = new()
                    {
                        new OrderBox()
                        {
                            BoxIndex = boxIndex,
                            BoxCode = $"3897*{orderId}*{boxIndex}"
                        }
                    },
                });
                boxIndex++;
            }
            var orderShipments = new List<OrderShipmentViewModel>();
            orderShipments.Add(new()
            {
                ShipmentId = shipmentId,
                OrderCode = orderId.ToString(),
                Items = orderItemsViewModels
            });

            var package = new OrderViewModel()
            {
                Data = new()
                {
                    Token = _configuration.GetSection("goodsTestToken").Value,
                    Shipments = orderShipments
                },
                Meta = new()
            };
            return package;
        }
        private OrderViewModel CreateOrderConfirm(string shipmentId, List<GoodsOrderItem> orderItems)
        {
            var orderId = orderItems.First().OrderId;
            var orderItemViewModels = new List<OrderItemViewModel>();
            foreach (var item in orderItems)
            {
                orderItemViewModels.Add(new()
                {
                    ItemIndex = item.ItemIndex,
                    OfferId = item.OfferId
                });
            }
            var orderShipments = new List<OrderShipmentViewModel>();

            orderShipments.Add(new()
            {
                ShipmentId = shipmentId,
                OrderCode = orderId.ToString(),
                Items = orderItemViewModels
            });
            var order = new OrderViewModel()
            {
                Data = new()
                {
                    Token = _configuration.GetSection("goodsTestToken").Value,
                    Shipments = orderShipments
                },
                Meta = new()
            };
            return order;
        }
        private OrderViewModel CreateOrderReject(string shipmentId, List<GoodsOrderItem> orderItems)
        {
            var orderItemViewModels = new List<OrderItemViewModel>();
            foreach (var item in orderItems)
            {
                orderItemViewModels.Add(new()
                {
                    ItemIndex = item.ItemIndex,
                    OfferId = item.OfferId
                });
            }
            var orderShipments = new List<OrderShipmentViewModel>();
            orderShipments.Add(new()
            {
                ShipmentId = shipmentId,
                Items = orderItemViewModels
            });
            var order = new OrderViewModel()
            {
                Data = new()
                {
                    Token = _configuration.GetSection("goodsTestToken").Value,
                    Shipments = orderShipments
                },
                Reason = new()
                {
                    Type = Enum.GetName(typeof(ReasonType), ReasonType.OUT_OF_STOCK)
                },
                Meta = new()
            };
            return order;
        }
        private async Task<SuccessfulResponse> SendRequest(string url, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync(url, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SuccessfulResponse>(resultContent);
        }
        private async Task<bool> SendConfirmRequest(OrderViewModel order)
        {
            var body = JsonConvert.SerializeObject(order);
            var url = "/api/market/v1/orderService/order/confirm";
            var response = await SendRequest(url, body);
            if (response.Success == 1)
                return true;
            return false;
        }
        private async Task<bool> SendPackageRequest(OrderViewModel order)
        {
            var body = JsonConvert.SerializeObject(order);
            var url = "/api/market/v1/orderService/order/packing";
            var response = await SendRequest(url, body);
            if (response.Success == 1)
                return true;
            return false;
        }
        private async Task<bool> SendRejectRequest(OrderViewModel order)
        {
            var body = JsonConvert.SerializeObject(order);
            var url = "/api/market/v1/orderService/order/reject";
            var response = await SendRequest(url, body);
            if (response.Success == 1)
                return true;
            return false;
        }
    }
}
