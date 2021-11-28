using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Extensions;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressOrderDetailRepository: AzureGenericRepository<AliExpressOrderDetail>, IAzureAliExpressOrderDetailRepository 
    {
        private readonly string _tableName;
        private readonly string _connectionString;

        public AzureAliExpressOrderDetailRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
            _tableName = tableName;
            _connectionString = connectionString;
        }

        public async Task Update(IEnumerable<AliExpressOrderDetail> orderDetails)
        {
            try
            {
                //var dateTimeNow = new DateTimeWithZone(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
                var updateOrderDetailString = new AliExpressOrderDetail().UpdateString(_tableName);
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var orderDetailsAnom = orderDetails.Select(orderDetail => new
                    {
                        id = orderDetail.Id,
                        logistics_service_name = orderDetail.LogisticsServiceName,
                        product_count = orderDetail.ProductCount,
                        product_name = orderDetail.ProductName,
                        product_unit_price = orderDetail.ProductUnitPrice,
                        send_goods_operator = orderDetail.SendGoodsOperator,
                        show_status = orderDetail.ShowStatus,
                        total_count_product_amount = orderDetail.TotalProductAmount,
                        updated = DateTime.Now
                    });
                    await connection.ExecuteAsync(updateOrderDetailString, orderDetailsAnom);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public async Task Add(IEnumerable<AliExpressOrderDetail> orderDetails)
        {
            try
            {
                //var dateTimeNow = new DateTimeWithZone(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
                var insertOrderDetailString = new AliExpressOrderDetail().InsertString(_tableName);
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var orderDetailsAnom = orderDetails.Select(orderDetail => new
                    {
                        logistics_service_name = orderDetail.LogisticsServiceName,
                        order_id = orderDetail.OrderId,
                        ali_order_id = orderDetail.AliOrderId,
                        product_count = orderDetail.ProductCount,
                        product_id = orderDetail.ProductId,
                        product_name = orderDetail.ProductName,
                        product_unit_price = orderDetail.ProductUnitPrice,
                        send_goods_operator = orderDetail.SendGoodsOperator,
                        show_status = orderDetail.ShowStatus,
                        goods_prepare_time = orderDetail.GoodsPrepareTime,
                        total_count_product_amount = orderDetail.TotalProductAmount,
                        created = DateTime.Now
                    });
                    await connection.ExecuteAsync(insertOrderDetailString, orderDetailsAnom);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
