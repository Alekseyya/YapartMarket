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
    public class AliExpressOrderDetailRepository: AzureGenericRepository<AliExpressOrderDetail>, IAliExpressOrderDetailRepository 
    {
        private readonly string _tableName;
        private readonly string _connectionString;

        public AliExpressOrderDetailRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
            _tableName = tableName;
            _connectionString = connectionString;
        }

        public async Task UpdateAsync(IEnumerable<AliExpressOrderDetail> orderDetails)
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
                        order_id = orderDetail.OrderId,
                        logistics_service_name = orderDetail.LogisticsServiceName,
                        product_count = orderDetail.ProductCount,
                        product_id = orderDetail.ProductId,
                        product_name = orderDetail.ProductName,
                        product_unit_price = orderDetail.ItemPrice,
                        show_status = orderDetail.ShowStatus,
                        goods_prepare_time = orderDetail.GoodsPrepareDays,
                        total_count_product_amount = orderDetail.TotalProductAmount,
                        updated = DateTime.Now,
                        created = DateTime.Now
                    });
                    await connection.ExecuteAsync(updateOrderDetailString, orderDetailsAnom);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public async Task AddAsync(IEnumerable<AliExpressOrderDetail> orderDetails)
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
                        product_count = orderDetail.ProductCount,
                        product_id = orderDetail.ProductId,
                        product_name = orderDetail.ProductName,
                        product_unit_price = orderDetail.ItemPrice,
                        show_status = orderDetail.ShowStatus,
                        goods_prepare_time = orderDetail.GoodsPrepareDays,
                        total_count_product_amount = orderDetail.TotalProductAmount,
                        created = DateTime.Now,
                        updated = DateTime.Now
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
