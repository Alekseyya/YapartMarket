using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YapartMarket.Core;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Core.Models.Raw;

[assembly: InternalsVisibleTo("UnitTests")]
namespace YapartMarket.BL.Implementation
{
    public class AliExpressOrderService : SendRequest<GetOrderList>, IAliExpressOrderService
    {
        private readonly ILogger<AliExpressOrderService> _logger;
        private readonly IAliExpressOrderRepository _orderRepository;
        private readonly IAliExpressOrderDetailRepository _orderDetailRepository;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly AliExpressOptions _aliExpressOptions;
        private readonly HttpClient _httpClient;

        public AliExpressOrderService(ILogger<AliExpressOrderService> logger, IOptions<AliExpressOptions> options, IAliExpressOrderRepository orderRepository,
            IAliExpressOrderDetailRepository orderDetailRepository, IServiceScopeFactory scopeFactory, IHttpClientFactory factory)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _scopeFactory = scopeFactory;
            _aliExpressOptions = options.Value;
            _httpClient = factory.CreateClient("aliExpress");
            _httpClient.DefaultRequestHeaders.Add("x-auth-token", options.Value.AuthToken);
        }
        //todo написать метод для отправки заказов. Похожий на нижний. Создать еще один десериалйзер. Отправить заказы.
        //todo напечатать этикетки отправления - отдавать отцу метод
        public async Task<IReadOnlyList<AliExpressOrder>> QueryOrderDetailAsync(DateTime? startDateTime = null, DateTime? endDateTime = null, List<OrderStatus> orderStatusList = null)
        {

            var getOrderRequest = new GetOrderList()
            {
                date_start = startDateTime?.ToString("yyy-MM-dd HH:mm:ss"),
                date_end = endDateTime?.ToString("yyy-MM-dd HH:mm:ss"),
                page = 1,
                page_size = 99
            };
            var result = await Request(getOrderRequest, _aliExpressOptions.GetOrderList, _httpClient);
            var aliExpressOrderList = new List<AliExpressOrder>();
            using (var scope = _scopeFactory.CreateScope())
            {
                var deserialize = scope.ServiceProvider.GetRequiredService<Deserializer<IReadOnlyList<AliExpressOrder>>>();
                var order = deserialize.Deserialize(result);
                if (order.IsAny())
                    aliExpressOrderList.AddRange(order.ToList());
            }
            return aliExpressOrderList;
        }
        public async Task CreateLogisticOrderAsync()
        {
            var startOfDay = DateTime.Now.StartOfDay();
            var endOfDay = DateTime.Now.EndOfDay();
            var getOrderRequest = new GetOrderList()
            {
                date_start = startOfDay.ToString("yyy-MM-dd HH:mm:ss"),
                date_end = endOfDay.ToString("yyy-MM-dd HH:mm:ss"),
                page = 1,
                page_size = 99
            };
            var getOrderResult = await HttpExtension.Request(getOrderRequest, _aliExpressOptions.GetOrderList, _httpClient);
            var aliExpressOrderList = new List<AliExpressOrder>();
            using (var scope = _scopeFactory.CreateScope())
            {
                var deserialize = scope.ServiceProvider.GetRequiredService<Deserializer<IReadOnlyList<AliExpressOrder>>>();
                var order = deserialize.Deserialize(getOrderResult);
                if (order.IsAny())
                    aliExpressOrderList.AddRange(order.ToList());
            }
            var orderWaitSendGoods = aliExpressOrderList.Where(x => x.OrderStatus == OrderStatus.WaitSendGoods);
            if (orderWaitSendGoods.Any())
            {
                foreach (var goods in orderWaitSendGoods)
                {
                    var orderId = goods.OrderId;
                    var orderDetails = goods.AliExpressOrderDetails;
                    var maxLength = orderDetails.Max(x => x.Length);
                    var summHeight = orderDetails.Sum(x => x.Height);
                    var summWeight = (double)orderDetails.Sum(x => x.Weight) / 1000;
                    var summWidth = orderDetails.Sum(x => x.Width);
                    var items = goods.AliExpressOrderDetails.Select(x => new LogisticOrderItemInfo()
                    {
                        quantity = x.ProductCount,
                        sku_id = x.SkuId
                    });
                    var logisticOrderItems = orderDetails.Select(x => new LogisticOrderItem()
                    {
                        trade_order_id = orderId,
                        total_length = maxLength,
                        total_height = summHeight,
                        total_weight = summWeight,
                        total_width = summWidth,
                        items = items.ToList(),
                    });
                    var logisticOrder = new LogisticOrder()
                    {
                        orders = logisticOrderItems.ToList()
                    };
                    await HttpExtension.Request(logisticOrder, _aliExpressOptions.CreateLogisticOrder, _httpClient);
                }
            }
        } 
        /// <summary>
        /// Получение заказа по статусу "Ожидает отправки товара"
        /// </summary>
        /// <param name="start">Дата начала обновления заказа</param>
        /// <param name="end">Дата окончания обновления заказа</param>
        public async Task<IEnumerable<AliExpressOrder>> GetOrdersAsync(DateTime start, DateTime end)
        {
            var orders = await _orderRepository.GetOrdersByWaitSellerSendGoodsAsync(start, end);
            return orders;
        }

        public async Task AddOrdersAsync(List<AliExpressOrder> aliExpressOrders)
        {
            foreach (var aliExpressOrder in aliExpressOrders)
            {
                foreach (var aliExpressOrderDetail in aliExpressOrder.AliExpressOrderDetails)
                {
                    aliExpressOrderDetail.OrderId = aliExpressOrder.OrderId;
                }
            }
            var newAliExpressOrders = await ExceptOrders(aliExpressOrders);
            if (newAliExpressOrders.Any())
                await _orderRepository.AddOrdersAsync(newAliExpressOrders);
            var updatedOrders = await IntersectOrder(aliExpressOrders);
            if (updatedOrders.IsAny())
                await _orderRepository.UpdateAsync(updatedOrders);
            await AddOrUpdateOrderDetails(aliExpressOrders);
        }

        public async Task AddOrUpdateOrderDetails(List<AliExpressOrder> aliExpressOrders)
        {
            var orderDetails = aliExpressOrders.SelectMany(x => x.AliExpressOrderDetails).ToList();
            var orderDetailHasInDb = await _orderDetailRepository.GetInAsync("order_id", new { order_id = orderDetails.Select(x => x.OrderId) });
            var orderDetailNotHasInDb = orderDetails.Where(x => orderDetailHasInDb.All(t => t.OrderId != x.OrderId)).ToList();
            if (orderDetailHasInDb.IsAny())
            {
                var modifyOrderDetails = orderDetailHasInDb.Where(x => orderDetails.Any(t =>
                    t.OrderId == x.OrderId
                    && t.ProductCount != x.ProductCount && t.ItemPrice != x.ItemPrice 
                    && t.ShowStatus != x.ShowStatus && t.TotalProductAmount != x.TotalProductAmount));
                if (modifyOrderDetails.IsAny())
                    await _orderDetailRepository.Update(modifyOrderDetails);
            }
            if (orderDetailNotHasInDb.IsAny())
                await _orderDetailRepository.Add(orderDetailNotHasInDb);
        }
        /// <summary>
        /// Получить только новые заказы
        /// </summary>
        /// <param name="aliExpressOrderList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AliExpressOrder>> ExceptOrders(List<AliExpressOrder> aliExpressOrderList)
        {
            var orderInDb = await _orderRepository.GetInAsync("order_id", new { order_id = aliExpressOrderList.Select(x => x.OrderId) });
            return aliExpressOrderList.Where(aliOrder => orderInDb.All(orderDb => orderDb.OrderId != aliOrder.OrderId));
        }
        
        /// <summary>
        /// Получить заказы которые надо обновить
        /// </summary>
        /// <param name="aliExpressOrderList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AliExpressOrder>> IntersectOrder(List<AliExpressOrder> aliExpressOrderList)
        {
            var ordersInDb = await _orderRepository.GetInAsync("order_id", new { order_id = aliExpressOrderList.Select(x => x.OrderId) });
            //значит что-то поменялось в заказе, количество товара, цена мб
            var orderUpdates = aliExpressOrderList.Where(x => ordersInDb.Any(orderDb =>
                    orderDb.OrderId == x.OrderId &&
                    (orderDb.OrderStatus != x.OrderStatus ||
                    orderDb.TotalProductCount != x.TotalProductCount ||
                    orderDb.TotalPayAmount != x.TotalPayAmount ||
                    orderDb.PaymentStatus != x.PaymentStatus)
            )).ToList();

            foreach (var orderInDb in ordersInDb)
            {
                if (orderUpdates.Any(x => x.OrderId == orderInDb.OrderId))
                {
                    var orderUpdate = orderUpdates.FirstOrDefault(x=>x.OrderId == orderInDb.OrderId);
                    orderUpdate.Id = orderInDb.Id;
                }
            }
            return orderUpdates;
        }
    }
}
