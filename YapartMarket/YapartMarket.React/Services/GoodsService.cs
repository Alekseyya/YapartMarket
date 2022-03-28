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
using YapartMarket.Core.Models.Azure;
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
        public void GetOrders(OrderNewViewModel order, out List<OrderNewShipmentItem> confirmOrders, out List<OrderNewShipmentItem> rejectOrders)
        {
            confirmOrders = new List<OrderNewShipmentItem>();
            rejectOrders =  new List<OrderNewShipmentItem>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                connection.Open();
                var orderItems = order.OrderNewDataViewModel.Shipments[0].Items;
                var groupOrderItems = orderItems.GroupBy(x => x.OfferId).Select(x => new { OfferId = x.Key, Count = x.Count() });
                
                foreach (var orderItem in groupOrderItems)
                {
                    var product = connection.QueryFirstOrDefault<Product>("select * from products where sku = @sku", new { sku = orderItem.OfferId });
                    if (product.Count >= orderItem.Count)
                        confirmOrders.AddRange(orderItems.Where(x => x.OfferId == orderItem.OfferId));
                    else
                    {
                        var rejectItems = orderItem.Count - product.Count;
                        var confirmItems = orderItem.Count - rejectItems;
                        confirmOrders.AddRange(orderItems.Where(x => x.OfferId == orderItem.OfferId).Take(confirmItems));
                        rejectOrders.AddRange(orderItems.Where(x => x.OfferId == orderItem.OfferId).TakeLast(confirmItems));
                    }
                }
            }
        }
        public async Task<int> SaveOrder(string shipmentId, List<OrderNewShipmentItem> confirmOrderItems, List<OrderNewShipmentItem> rejectOrderItems)
        {
            int id = default;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var order = await connection.QueryFirstOrDefaultAsync<GoodsOrder>("select * from goods_order where shipmentId = @shipmentId", new { shipmentId = shipmentId });
                //todo добавить orderItem если есть order 
                if (order == null)
                {
                    id = await connection.QuerySingleAsync<int>("insert into goods_order(shipmentId) values(@shipmentId);SELECT CAST(SCOPE_IDENTITY() as int)", new { shipmentId = shipmentId });
                    foreach (var confirmOrder in confirmOrderItems)
                    {
                        connection.Execute("insert into goods_order_details(offerId, orderId, item_index, reason_type) values(@offerId, @orderId, @item_index, @reason_type)", new
                        {
                            offerId = confirmOrder.OfferId,
                            orderId = id,
                            item_index = confirmOrder.ItemIndex,
                            reason_type = (int)ReasonType.Empty
                        });
                    }
                    foreach (var rejectItem in rejectOrderItems)
                    {
                        connection.Execute("insert into goods_order_details(offerId, orderId, item_index, reason_type) values(@offerId, @orderId, @item_index, @reason_type)", new
                        {
                            offerId = rejectItem.OfferId,
                            orderId = id,
                            item_index = rejectItem.ItemIndex,
                            reason_type = (int)ReasonType.OUT_OF_STOCK
                        });
                    }
                }
            }
            return id;
        }

        public async Task<bool> Reject(string shipmentId, int orderId)
        {
            var shipments = await Shipments(shipmentId, orderId, ReasonType.OUT_OF_STOCK);
            var order = CreateOrderReject(shipments);
            var isSent = await SendRejectRequest(order);
            return isSent;
        }

        public async Task<bool> Confirm(string shipmentId, int orderId)
        {
            var shipments = await Shipments(shipmentId, orderId, ReasonType.Empty);
            var order = CreateOrderConfirm(shipments);
            var isSent = await SendConfirmRequest(order);
            return isSent;
        }
        private async Task<List<OrderShipmentViewModel>> Shipments(string shipmentId, int orderId, ReasonType reasonType)
        {
            var goodsOrderItems = new List<GoodsOrderItem>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                goodsOrderItems = (List<GoodsOrderItem>)await connection.QueryAsync<GoodsOrderItem>("select * from goods_order_details where orderId = @orderId and reason_type = @reason_type", new { orderId = orderId, reason_type = (int)reasonType });
            }
            var orderItems = new List<OrderItemViewModel>();
            foreach (var goodsItem in goodsOrderItems)
            {
                orderItems.Add(new()
                {
                    ItemIndex = goodsItem.ItemIndex,
                    OfferId = goodsItem.OfferId
                });
            }
            var orderShipments = new List<OrderShipmentViewModel>();
            orderShipments.Add(new()
            {
                ShipmentId = shipmentId,
                OrderCode = orderId.ToString(),
                Items = orderItems
            });
            return orderShipments;
        }

        private OrderViewModel CreateOrderConfirm(List<OrderShipmentViewModel> shipments)
        {
            var order = new OrderViewModel()
            {
                Data = new()
                {
                    Token = _configuration.GetSection("goodsTestToken").Value,
                    Shipments = shipments
                },
                Meta = new()
            };
            return order;
        }
        private OrderViewModel CreateOrderReject(List<OrderShipmentViewModel> shipments)
        {
            var order = new OrderViewModel()
            {
                Data = new()
                {
                    Token = _configuration.GetSection("goodsTestToken").Value,
                    Shipments = shipments
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
