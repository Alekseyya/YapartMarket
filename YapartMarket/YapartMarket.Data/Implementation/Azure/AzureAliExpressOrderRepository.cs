using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.DTO;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressOrderRepository : AzureGenericRepository<AliExpressOrder>, IAzureAliExpressOrderRepository
    {
        private readonly string _connectionString;

        public AzureAliExpressOrderRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
            _connectionString = connectionString;
        }
        //todo переписать метод!!!
        public async Task AddOrders(IEnumerable<AliExpressOrderListDTO> aliExpressOrderListDtos)
        {

            var insertOrder = @"insert into dbo.orders (seller_signer_fullname, seller_login_id, order_id, time_stamp, total_product_count, total_pay_amount, order_status, gmt_create, gmt_update, fund_status, frozen_status)
                       OUTPUT INSERTED.id 
                       values (@seller_signer_fullname, @seller_login_id, @order_id, @time_stamp, @total_product_count, @total_pay_amount, @order_status, @gmt_create, @gmt_update, @fund_status, @frozen_status) ";
            var insertOrderDetail = @"insert into dbo.order_details (logistics_service_name, ali_order_id, order_id, product_count, product_id, product_name, product_unit_price, send_goods_operator, show_status, goods_prepare_time, total_product_amount) 
values(@logistics_service_name, @ali_order_id, @order_id, @product_count, @product_id, @product_name, @product_unit_price, @send_goods_operator, @show_status, @goods_prepare_time, @total_product_amount)";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    foreach (var aliExpressOrder in aliExpressOrderListDtos)
                    {
                        var newOrderId = await connection.QuerySingleAsync<int>(insertOrder, new
                        {
                            seller_signer_fullname = aliExpressOrder.SellerSignerFullName,
                            seller_login_id = aliExpressOrder.SellerLoginId,
                            order_id = aliExpressOrder.OrderId,
                            time_stamp = DateTime.UtcNow,
                            total_product_count = aliExpressOrder.AliExpressOrderProducts.Select(x => x.ProductCount).Aggregate((a, b) => a + b), //сумма всех продуктов
                            total_pay_amount = aliExpressOrder.AliExpressOrderProducts.Select(x => x.TotalProductAmount).Aggregate((a, b) => a + b), //цена всех продуктов
                            order_status = aliExpressOrder.OrderStatus,
                            gmt_create = aliExpressOrder.GmtCreate,
                            gmt_update = aliExpressOrder.GmtUpdate,
                            fund_status = aliExpressOrder.FundStatus,
                            frozen_status = aliExpressOrder.FrozenStatus
                        });

                        foreach (var aliExpressOrderProductDto in aliExpressOrder.AliExpressOrderProducts)
                            await connection.ExecuteAsync(insertOrderDetail, new
                            {
                                logistics_service_name = aliExpressOrderProductDto.LogisticsServiceName,
                                ali_order_id = newOrderId,
                                order_id = aliExpressOrderProductDto.OrderId,
                                product_count = aliExpressOrderProductDto.ProductCount,
                                product_id = aliExpressOrderProductDto.ProductId,
                                product_name = aliExpressOrderProductDto.ProductName,
                                product_unit_price = aliExpressOrderProductDto.ProductUnitPrice,
                                send_goods_operator = aliExpressOrderProductDto.SendGoodsOperator,
                                show_status = aliExpressOrderProductDto.ShowStatus,
                                goods_prepare_time = aliExpressOrderProductDto.GoodsPrepareTime,
                                total_product_amount = aliExpressOrderProductDto.TotalProductAmount
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
