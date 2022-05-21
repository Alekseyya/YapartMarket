using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using YapartMarket.Core.Data.Interfaces.Azure;
using YapartMarket.Core.Models.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public class AzureProductRepository : AzureGenericRepository<Product>,  IAzureProductRepository
    {
        private readonly string _tableName;
        private readonly string _connectionString;

        public AzureProductRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
            _tableName = tableName;
            _connectionString = connectionString;
        }
        public async Task BulkUpdateCountData(List<Product> list)
        {
            var dt = new DataTable(_tableName);
            dt = ConvertToDataTable(list);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(@"CREATE TABLE products_tmpCount (sku nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, count int NULL, updatedAt varchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL);", conn))
                {
                    try
                    {
                        await conn.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                        {
                            bulkcopy.BulkCopyTimeout = 6600;
                            bulkcopy.DestinationTableName = "products_tmpCount";
                            bulkcopy.ColumnMappings.Clear();
                            bulkcopy.ColumnMappings.Add("sku", "sku");
                            bulkcopy.ColumnMappings.Add("count", "count");
                            bulkcopy.ColumnMappings.Add("updatedAt", "updatedAt");
                            await bulkcopy.WriteToServerAsync(dt.CreateDataReader());
                            bulkcopy.Close();
                        }


                        command.CommandTimeout = 3000;
                        command.CommandText = "UPDATE P SET P.count = T.count FROM products AS P INNER JOIN products_tmpCount AS T ON P.sku = T.sku ;DROP TABLE products_tmpCount;";
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
