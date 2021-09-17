using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.Data.Interfaces.Azure;
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
                                created = DateTime.UtcNow
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateOrder(IEnumerable<AliExpressOrder> aliExpressOrders)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

            }
        }


    }
}
