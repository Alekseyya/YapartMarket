﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using YapartMarket.Core.BL;
using YapartMarket.Core.Config;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

[assembly: InternalsVisibleTo("UnitTests")]
namespace YapartMarket.BL.Implementation
{

    public class AliExpressOrderService : IAliExpressOrderService
    {
        private readonly ILogger<AliExpressOrderService> _logger;
        private readonly IAzureAliExpressOrderRepository _orderRepository;
        private readonly IAzureAliExpressOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        private readonly AliExpressOptions _aliExpressOptions;

        public AliExpressOrderService(ILogger<AliExpressOrderService> logger, IOptions<AliExpressOptions> options, IAzureAliExpressOrderRepository orderRepository,
            IAzureAliExpressOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _aliExpressOptions = options.Value;
        }
        
        public List<AliExpressOrderDTO> QueryOrderDetail(DateTime? startDateTime = null, DateTime? endDateTime = null, List<OrderStatus> orderStatusList = null)
        {
            var currentPage = 1;
            ITopClient client = new DefaultTopClient(_aliExpressOptions.HttpsEndPoint, _aliExpressOptions.AppKey, _aliExpressOptions.AppSecret, "Json");
            var aliExpressOrderList = new List<AliExpressOrderDTO>();
            do
            {
                try
                {
                    AliexpressSolutionOrderGetRequest req = new AliexpressSolutionOrderGetRequest();
                    AliexpressSolutionOrderGetRequest.OrderQueryDomain obj1 = new AliexpressSolutionOrderGetRequest.OrderQueryDomain();
                    obj1.CreateDateEnd = endDateTime?.ToString("yyy-MM-dd hh:mm:ss");
                    obj1.CreateDateStart = startDateTime?.ToString("yyy-MM-dd hh:mm:ss");
                    if(orderStatusList != null)
                        obj1.OrderStatusList = new List<string>{ OrderStatus.SELLER_PART_SEND_GOODS.ToString() };
                    obj1.PageSize = 20;
                    obj1.CurrentPage = currentPage;
                    req.Param0_ = obj1;
                    var rsp = client.Execute(req, _aliExpressOptions.AccessToken);
                    var deserializeAliExpressOrderList = DeserializeAliExpressOrderList(rsp.Body);
                    if(deserializeAliExpressOrderList.Any())
                        aliExpressOrderList.AddRange(deserializeAliExpressOrderList);
                    else
                        break;
                } 
                catch (Exception e)
                {
                    break;
                }
                currentPage++;
            } while (true);
            return aliExpressOrderList;
        }
        /// <summary>
        /// Получение заказа по статусу "Ожидает отправки товара"
        /// </summary>
        /// <param name="start">Дата начала обновления заказа</param>
        /// <param name="end">Дата окончания обновления заказа</param>
        public async Task<IEnumerable<AliExpressOrder>> GetOrders(DateTime start, DateTime end)
        {
            //var select = $"select * from dbo.orders where gmt_update >= '{start.ToString("yyyy-MM-dd HH:mm:ss")}' and gmt_update <= '{end.ToString("yyyy-MM-dd HH:mm:ss")}' and order_status = {OrderStatus.WAIT_BUYER_ACCEPT_GOODS}";
            //var orders = await _orderRepository.GetAsync(select);
            
            //var parameters = new DynamicParameters();
            //parameters.Add("@gmt_update_start", start, DbType.DateTime);
            //parameters.Add("@gmt_update_end", end, DbType.DateTime);
            //parameters.Add("@order_status", (int)OrderStatus.WAIT_BUYER_ACCEPT_GOODS, DbType.Int16);
            //SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
            //var orders = await _orderRepository.GetAsync("select * from dbo.orders where gmt_update >= '@gmt_update_start' and gmt_update <= '@gmt_update_end' and order_status = @order_status", parameters);
            var orders = await _orderRepository.GetAsync("select * from dbo.orders where gmt_update >= '2021-09-16 00:00:00' and gmt_update <= '2021-09-16 23:59:59' and order_status = 2");
            return orders;

        }

        public async Task AddOrders(List<AliExpressOrder> aliExpressOrders)
        {
            var newAliExpressOrders = await ExceptOrders(aliExpressOrders);
            if (newAliExpressOrders.Any())
            {
                await _orderRepository.AddOrdersWitchOrderDetails(newAliExpressOrders);
            }
            var updatedOrders = await IntersectOrder(aliExpressOrders);
            if (updatedOrders.IsAny())
            {
                await _orderRepository.Update(updatedOrders);
                //список товаров в заказах из бд, новые в списке orderUpdates не учитываются
                var orderDetailUpdatesDb = await _orderDetailRepository.GetInAsync("order_id", new { order_id = updatedOrders.Select(x => x.OrderId) });

                var orderDetailUpdates = updatedOrders.SelectMany(x => x.AliExpressOrderDetails);
                //поучить те записи которые изменились
                var modifyOrderDetails = orderDetailUpdatesDb.Where(x => orderDetailUpdates.Any(t =>
                    t.OrderId == x.OrderId
                    && t.ProductCount != x.ProductCount && t.ProductUnitPrice != x.ProductUnitPrice && t.SendGoodsOperator != x.SendGoodsOperator
                    && t.ShowStatus != x.ShowStatus && t.TotalProductAmount != x.TotalProductAmount));
                await _orderDetailRepository.Update(modifyOrderDetails);
                //новые записи
                var newOrderDetails = orderDetailUpdates.Where(x => orderDetailUpdatesDb.Any(t => t.OrderId == x.OrderId && t.ProductId != x.ProductCount));
                await _orderDetailRepository.Add(newOrderDetails);

            }
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
            var orderInDb = await _orderRepository.GetInAsync("order_id", new { order_id = aliExpressOrderList.Select(x => x.OrderId) });
            //значит что-то поменялось в заказе, количество товара, цена мб
            var orderUpdates = aliExpressOrderList.Where(x => orderInDb.Any(orderDb =>
                orderDb.LogisticsStatus != x.LogisticsStatus &&
                orderDb.BizType != x.BizType &&
                orderDb.TotalProductCount != x.TotalProductCount &&
                orderDb.TotalPayAmount != x.TotalPayAmount &&
                orderDb.OrderStatus != x.OrderStatus && 
                orderDb.FundStatus != x.FundStatus &&
                orderDb.FrozenStatus != x.FrozenStatus));
            return orderUpdates;
        }

        public List<AliExpressOrderDTO> DeserializeAliExpressOrderList(string json)
        {
            var aliExpressResponseResult = JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json)?.AliExpressSolutionOrderGetResponseDTO.AliExpressSolutionOrderGetResponseResultDto;
            if (!aliExpressResponseResult.AliExpressOrderListDTOs.Any() && aliExpressResponseResult.TotalCount == 0)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<AliExpressGetOrderRoot>(json)?.AliExpressSolutionOrderGetResponseDTO.AliExpressSolutionOrderGetResponseResultDto.AliExpressOrderListDTOs;
        }
    }
}
