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
        public async Task Confirm(string shipmentId, List<OrderNewShipmentItem> confirmItems)
        {
            var orderConfirmItems = new List<OrderConfirmItem>();
            foreach (var item in confirmItems)
            {
                orderConfirmItems.Add(new()
                {
                    ItemIndex = Convert.ToInt16(item.ItemIndex),
                    OfferId = item.OfferId
                });
            }

            var confirmItemsViewModel = new List<OrderConfirmShipment>();
            confirmItemsViewModel.Add(new()
            {
                ShipmentId = shipmentId,
                //OrderCode = Random. //todo обязательно понадобится - записить в БД,
                Items = orderConfirmItems
            });
            
            var orderConfirm = new OrderConfirmViewModel()
            {
                Data = new()
                {
                    Token = _configuration.GetConnectionString("goodsTestToken"),
                    Shipments = confirmItemsViewModel
                }
            };//todo отправку запроса в отдельный метод!!
            var jsonResponse = JsonConvert.SerializeObject(orderConfirm);
            var content = new StringContent(jsonResponse, Encoding.UTF8, "application/json");
            _httpClient.BaseAddress = new Uri("https://partner.goodsteam.tech");
            var result = await _httpClient.PostAsync("/api/market/v1/orderService/order/confirm", content);
            string resultContent = await result.Content.ReadAsStringAsync();
        }
    }
}
