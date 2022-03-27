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

        public GoodsService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
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
        public async Task<int?> SaveOrder(string shipmentId, List<OrderNewShipmentItem> confirmOrderItems, List<OrderNewShipmentItem> rejectOrderItems)
        {
            int? id = null;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                var order = connection.QueryFirstOrDefaultAsync<GoodsOrder>("select * from goods_order where shipmentId = @shipmentId", new { shipmentId = shipmentId });
                if (order == null)
                {
                    id = connection.QuerySingle<int>("insert into goods_order(shipmentId) values(@shipmentId) goods_order;SELECT CAST(SCOPE_IDENTITY() as int)", new { shipmentId = shipmentId });
                    foreach (var confirmOrder in confirmOrderItems)
                    {
                        connection.Execute("insert into goods_order_details(offerId, orderId, item_index) values(@offerId, @orderId, @item_index)", new
                        {
                            offerId = confirmOrder.OfferId,
                            orderId = id,
                            item_index = confirmOrder.ItemIndex,
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
        public async Task<bool> Confirm(string shipmentId, int orderId)
        {
            var goodsOrderItems = new List<GoodsOrderItem>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                goodsOrderItems = (List<GoodsOrderItem>)await connection.QueryAsync<GoodsOrderItem>("select * from goods_order_details where orderId = @orderId and reason_type = null", new { orderId = orderId });
            }
            var orderConfirmItems = new List<OrderConfirmItem>();
            foreach (var goodsItem in goodsOrderItems)
            {
                orderConfirmItems.Add(new()
                {
                    ItemIndex = goodsItem.ItemIndex,
                    OfferId = goodsItem.OfferId
                });
            }
            var confirmItemsViewModel = new List<OrderConfirmShipment>();
            confirmItemsViewModel.Add(new()
            {
                ShipmentId = shipmentId,
                OrderCode = orderId.ToString(),
                Items = orderConfirmItems
            });
            
            var orderConfirm = new OrderConfirmViewModel()
            {
                Data = new()
                {
                    Token = _configuration.GetConnectionString("goodsTestToken"),
                    Shipments = confirmItemsViewModel
                },
                Meta = new()
            };//todo отправку запроса в отдельный метод!!
            var jsonResponse = JsonConvert.SerializeObject(orderConfirm);
            var content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            _httpClient.BaseAddress = new Uri("https://partner.goodsteam.tech");
            var result = await _httpClient.PostAsync("/api/market/v1/orderService/order/confirm", content);
            string resultContent = await result.Content.ReadAsStringAsync();
            var jsonResponseServer = JsonConvert.DeserializeObject<SuccessfulResponse>(resultContent);
            if (jsonResponseServer.Success == 1)
                return true;
            return false;
        }
    }
}
