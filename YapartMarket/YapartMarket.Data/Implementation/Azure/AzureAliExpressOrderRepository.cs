using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressOrderRepository : AzureGenericRepository<AliExpressOrder>, IAzureAliExpressOrderRepository
    {
        private readonly string _tableName;
        private readonly string _connectionString;

        public AzureAliExpressOrderRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
            _tableName = tableName;
            _connectionString = connectionString;
        }

        public async Task Update(IEnumerable<AliExpressOrder> aliExpressOrders)
        {
            var updateOrder = new AliExpressOrder().UpdateString(_tableName);
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var orders = aliExpressOrders.Select(aliExpressOrder => new
                    {
                        seller_signer_fullname = aliExpressOrder.SellerSignerFullName,
                        seller_login_id = aliExpressOrder.SellerLoginId,
                        order_id = aliExpressOrder.OrderId,
                        logistics_status = aliExpressOrder.LogisticsStatus,
                        biz_type = aliExpressOrder.BizType,
                        gmt_pay_time = aliExpressOrder.GmtPayTime,
                        end_reason = aliExpressOrder.EndReason,
                        updated = DateTime.UtcNow,
                        total_product_count = aliExpressOrder.TotalProductCount, //сумма всех продуктов
                        total_pay_amount = aliExpressOrder.TotalPayAmount, //цена всех продуктов
                        order_status = aliExpressOrder.OrderStatus,
                        gmt_create = aliExpressOrder.GmtCreate,
                        gmt_update = aliExpressOrder.GmtUpdate,
                        fund_status = aliExpressOrder.FundStatus,
                        frozen_status = aliExpressOrder.FrozenStatus
                    });
                    await connection.ExecuteAsync(updateOrder, orders);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<AliExpressOrder>> GetOrdersByWaitSellerSendGoods(DateTime start, DateTime end)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var orderDictionary = new Dictionary<int, AliExpressOrder>();
                var orderDetailDictionary = new Dictionary<long, AliExpressOrderDetail>();
                var orderInDb = await connection.QueryAsync<AliExpressOrder, AliExpressOrderDetail, Product, AliExpressOrder>(
                    @"select * FROM dbo.orders o 
inner join dbo.order_details od on o.id = od.order_id 
inner join dbo.products p on p.aliExpressProductId = od.product_id 
where gmt_update >= @gmt_update_start and gmt_update <= @gmt_update_end and order_status = @order_status",
                    (order, orderDetail, product) =>
                    {
                        AliExpressOrder orderEntry;
                        if (!orderDictionary.TryGetValue(order.Id, out orderEntry))
                        {
                            orderEntry = order;
                            orderEntry.AliExpressOrderDetails = new List<AliExpressOrderDetail>();
                            orderDictionary.Add(orderEntry.Id, orderEntry);
                        }
                        AliExpressOrderDetail orderDetailEntry;
                        if (!orderDetailDictionary.TryGetValue(orderDetail.ProductId, out orderDetailEntry))
                        {
                            orderDetailEntry = orderDetail;
                            orderDetailEntry.Product = product;
                            orderDetailDictionary.Add(orderDetail.OrderId, orderDetailEntry);
                        }
                        orderEntry.AliExpressOrderDetails.Add(orderDetailEntry);
                        return orderEntry;
                    }, 
                    new { gmt_update_start = start, gmt_update_end = end, order_status = (int)OrderStatus.WAIT_SELLER_SEND_GOODS },
                    splitOn: "order_id, product_id");
                return orderInDb;
            }
        }
        
        public async Task AddOrdersWitchOrderDetails(IEnumerable<AliExpressOrder> aliExpressOrders)
        {
            var insertOrder = new AliExpressOrder().InsertString(_tableName);
            var insertOrderDetail = new AliExpressOrderDetail().InsertString("dbo.order_details");
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    foreach (var aliExpressOrder in aliExpressOrders)
                    {
                        var newOrderId = await connection.QuerySingleAsync<int>(insertOrder, new
                        {
                            seller_signer_fullname = aliExpressOrder.SellerSignerFullName,
                            seller_login_id = aliExpressOrder.SellerLoginId,
                            order_id = aliExpressOrder.OrderId,
                            logistics_status = aliExpressOrder.LogisticsStatus,
                            biz_type = aliExpressOrder.BizType,
                            gmt_pay_time = aliExpressOrder.GmtPayTime,
                            end_reason = aliExpressOrder.EndReason,
                            created = DateTime.UtcNow,
                            updated = (DateTime?)null,
                            total_product_count = aliExpressOrder.TotalProductCount, //сумма всех продуктов
                            total_pay_amount = aliExpressOrder.TotalPayAmount, //цена всех продуктов
                            order_status = aliExpressOrder.OrderStatus,
                            gmt_create = aliExpressOrder.GmtCreate,
                            gmt_update = aliExpressOrder.GmtUpdate,
                            fund_status = aliExpressOrder.FundStatus,
                            frozen_status = aliExpressOrder.FrozenStatus
                        });

                        foreach (var aliExpressOrderProductDto in aliExpressOrder.AliExpressOrderDetails)
                            await connection.ExecuteAsync(insertOrderDetail, new
                            {
                                logistics_service_name = aliExpressOrderProductDto.LogisticsServiceName,
                                ali_order_id = aliExpressOrderProductDto.OrderId,
                                order_id = newOrderId,
                                product_count = aliExpressOrderProductDto.ProductCount,
                                product_id = aliExpressOrderProductDto.ProductId,
                                product_name = aliExpressOrderProductDto.ProductName,
                                product_unit_price = aliExpressOrderProductDto.ProductUnitPrice,
                                send_goods_operator = aliExpressOrderProductDto.SendGoodsOperator,
                                show_status = aliExpressOrderProductDto.ShowStatus,
                                goods_prepare_time = aliExpressOrderProductDto.GoodsPrepareTime,
                                total_count_product_amount = aliExpressOrderProductDto.TotalProductAmount,
                                created = DateTime.UtcNow,
                                updated = (DateTime?)null,
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
