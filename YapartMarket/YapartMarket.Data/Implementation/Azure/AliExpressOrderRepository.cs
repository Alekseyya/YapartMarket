using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DateStructures;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AliExpressOrderRepository : AzureGenericRepository<AliExpressOrder>, IAliExpressOrderRepository
    {
        private readonly ILogger<AliExpressOrderRepository> _azureAliExpressOrderLogger;
        private readonly string _tableName;
        private readonly string _connectionString;

        public AliExpressOrderRepository(ILogger<AliExpressOrderRepository> azureAliExpressOrderLogger, string tableName, string connectionString) : base(tableName, connectionString)
        {
            _azureAliExpressOrderLogger = azureAliExpressOrderLogger;
            _tableName = tableName;
            _connectionString = connectionString;
        }

        public async Task Update(IEnumerable<AliExpressOrder> aliExpressOrders)
        {
            //var dateTimeNow = new DateTimeWithZone(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
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
                        updated = DateTime.Now,
                        created = (string)null,
                        total_product_count = aliExpressOrder.TotalProductCount, //сумма всех продуктов
                        total_pay_amount = aliExpressOrder.TotalPayAmount, //цена всех продуктов
                        order_status = aliExpressOrder.OrderStatus,
                        gmt_create = aliExpressOrder.GmtCreate,
                        gmt_update = aliExpressOrder.GmtUpdate,
                        fund_status = aliExpressOrder.FundStatus,
                        frozen_status = aliExpressOrder.FrozenStatus,
                        id = aliExpressOrder.Id
                    });
                    await connection.ExecuteAsync(updateOrder, orders);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<AliExpressOrder>> GetOrdersByWaitSellerSendGoodsAsync(DateTime start, DateTime end)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var orderDictionary = new Dictionary<long, AliExpressOrder>();
                    var orderInDb = await connection.QueryAsync<AliExpressOrder, AliExpressOrderDetail, AliExpressOrder>(
                        @"select * FROM dbo.orders o 
inner join dbo.order_details od on o.order_id = od.order_id 
where gmt_create >= @gmt_create_start and gmt_create <= @gmt_create_end and order_status = @order_status",
                        (order, orderDetail) =>
                        {
                            AliExpressOrder orderEntry;
                            if (!orderDictionary.TryGetValue(order.OrderId, out orderEntry))
                            {
                                orderEntry = order;
                                orderEntry.AliExpressOrderDetails = new List<AliExpressOrderDetail>();
                                orderDictionary.Add(orderEntry.OrderId, orderEntry);
                            }
                            orderEntry.AliExpressOrderDetails.Add(orderDetail);
                            return orderEntry;
                        },
                        new { gmt_create_start = start, gmt_create_end = end, order_status = (int)OrderStatus.WAIT_SELLER_SEND_GOODS },
                        splitOn: "order_id"); //, product_id

                    foreach (var order in orderInDb)
                    {
                        foreach (var aliExpressOrder in order.AliExpressOrderDetails)
                        {
                            aliExpressOrder.Product = await connection.QueryFirstAsync<Product>("select * from dbo.products where aliExpressProductId = @aliExpressProductId", new
                            {
                                aliExpressProductId = aliExpressOrder.ProductId
                            });
                        }
                    }
                    return orderInDb;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        public async Task AddOrders(IEnumerable<AliExpressOrder> aliExpressOrders)
        {
            var insertOrder = new AliExpressOrder().InsertString(_tableName);
            var insertOrderDetail = new AliExpressOrderDetail().InsertString("dbo.order_details");
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                foreach (var aliExpressOrder in aliExpressOrders)
                {
                    try
                    {
                        await connection.QuerySingleAsync<int>(insertOrder, new
                        {
                            seller_signer_fullname = aliExpressOrder.SellerSignerFullName,
                            seller_login_id = aliExpressOrder.SellerLoginId,
                            order_id = aliExpressOrder.OrderId,
                            logistics_status = aliExpressOrder.LogisticsStatus,
                            biz_type = aliExpressOrder.BizType,
                            gmt_pay_time = aliExpressOrder.GmtPayTime,
                            end_reason = aliExpressOrder.EndReason,
                            created = DateTime.Now,
                            updated = (DateTime?)null,
                            total_product_count = aliExpressOrder.TotalProductCount, //сумма всех продуктов
                            total_pay_amount = aliExpressOrder.TotalPayAmount, //цена всех продуктов
                            order_status = aliExpressOrder.OrderStatus,
                            gmt_create = aliExpressOrder.GmtCreate,
                            gmt_update = aliExpressOrder.GmtUpdate,
                            fund_status = aliExpressOrder.FundStatus,
                            frozen_status = aliExpressOrder.FrozenStatus
                        });
                    }
                    catch (Exception ex)
                    {
                        _azureAliExpressOrderLogger.LogInformation($"OrderId : {aliExpressOrder.OrderId} OrderDetails: {string.Join(',', aliExpressOrder.AliExpressOrderDetails.Select(x => $" OrderId :{x.OrderId}"))} \n");
                        _azureAliExpressOrderLogger.LogWarning(ex.Message);
                        _azureAliExpressOrderLogger.LogWarning(ex.StackTrace);
                        throw;
                    }
                }
            }

        }
    }
}
