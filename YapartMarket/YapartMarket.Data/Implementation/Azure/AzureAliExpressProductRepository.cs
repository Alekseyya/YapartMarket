using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureAliExpressProductRepository : AzureGenericRepository<AliExpressProduct>, IAzureAliExpressProductRepository
    {
        private readonly string _tableName;
        private readonly string _connectionString;
        public AzureAliExpressProductRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
            _tableName = tableName;
            _connectionString = connectionString;
        }
        public async Task BulkUpdateData(IReadOnlyList<AliExpressProduct> products)
        {
            var dt = new DataTable(_tableName);
            dt = ConvertToDataTable(products);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(@"CREATE TABLE aliProducts_tmp (
sku nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
productId bigint NULL,
created varchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
updatedAt varchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
category_id bigint NULL,
currency_code nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
group_id bigint NULL,
package_height int NULL,
package_length int NULL,
product_price decimal NULL,
product_status_type nvarchar(100) NULL,
product_unit bigint NULL,
gross_weight varchar(10) NULL);", conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                        {
                            bulkcopy.BulkCopyTimeout = 6600;
                            bulkcopy.DestinationTableName = "aliProducts_tmp";
                            bulkcopy.ColumnMappings.Clear();
                            bulkcopy.ColumnMappings.Add("sku", "sku");
                            bulkcopy.ColumnMappings.Add("productId", "productId");
                            bulkcopy.ColumnMappings.Add("created", "created");
                            bulkcopy.ColumnMappings.Add("updatedAt", "updatedAt");
                            bulkcopy.ColumnMappings.Add("category_id", "category_id");
                            bulkcopy.ColumnMappings.Add("currency_code", "currency_code");
                            bulkcopy.ColumnMappings.Add("group_id", "group_id");
                            bulkcopy.ColumnMappings.Add("package_height", "package_height");
                            bulkcopy.ColumnMappings.Add("package_length", "package_length");
                            bulkcopy.ColumnMappings.Add("product_price", "product_price");
                            bulkcopy.ColumnMappings.Add("product_status_type", "product_status_type");
                            bulkcopy.ColumnMappings.Add("product_unit", "product_unit");
                            bulkcopy.ColumnMappings.Add("gross_weight", "gross_weight");
                            await bulkcopy.WriteToServerAsync(dt.CreateDataReader());
                            bulkcopy.Close();
                        }


                        command.CommandTimeout = 3000;
                        command.CommandText = @"UPDATE P SET 
P.sku = T.sku, 
P.created = T.created, 
P.updatedAt = T.updatedAt, 
P.category_id = T.category_id, 
P.currency_code = T.currency_code, 
P.group_id = T.group_id, 
P.package_height = T.package_height, 
P.package_length = T.package_length, 
P.product_price = T.product_price, 
P.product_status_type = T.product_status_type, 
P.product_unit = T.product_unit, 
P.gross_weight = T.gross_weight 
FROM aliExpressProducts AS P INNER JOIN aliProducts_tmp AS T ON P.productId = T.productId; DROP TABLE aliProducts_tmp;";
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception properly
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
