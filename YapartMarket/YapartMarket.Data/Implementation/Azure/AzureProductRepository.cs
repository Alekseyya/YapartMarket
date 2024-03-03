using System;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using YapartMarket.Core.Models.Azure;
using YapartMarket.Core.Data.Interfaces.Azure;

namespace YapartMarket.Data.Implementation.Azure
{
    public sealed class AzureProductRepository : AzureGenericRepository<Product>, IAzureProductRepository
    {
        readonly string tableName;
        readonly string connectionString;

        public AzureProductRepository(string tableName, string connectionString) : base(tableName, connectionString)
        {
            this.tableName = tableName;
            this.connectionString = connectionString;
        }
        public async Task BulkUpdateProductIdAsync(IReadOnlyList<Product> products, CancellationToken cancellationToken)
        {
            var dt = new DataTable(tableName);
            dt = ConvertToDataTable(products);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
                {
                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE products_productId (
sku nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
aliExpressProductId bigint NULL,
updatedAt varchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL);", connection))
                    {
                        try
                        {
                            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

                            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                            {
                                bulkcopy.BulkCopyTimeout = 6600;
                                bulkcopy.DestinationTableName = "products_productId";
                                bulkcopy.ColumnMappings.Clear();
                                bulkcopy.ColumnMappings.Add("sku", "sku");
                                bulkcopy.ColumnMappings.Add("aliExpressProductId", "aliExpressProductId");
                                bulkcopy.ColumnMappings.Add("updatedAt", "updatedAt");
                                await bulkcopy.WriteToServerAsync(dt.CreateDataReader(), cancellationToken).ConfigureAwait(false);
                                bulkcopy.Close();
                            }


                            command.CommandTimeout = 3000;
                            command.CommandText = @"UPDATE P SET P.aliExpressProductId = T.aliExpressProductId, P.updatedAt = T.updatedAt
FROM products AS P INNER JOIN products_productId AS T ON P.sku = T.sku;";
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                            command.CommandText = "DROP TABLE products_productId;";
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                        }
                        finally
                        {
                            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                            await connection.CloseAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }
        public async Task<string> BulkUpdateCountDataAsync(List<Product> list, CancellationToken cancellationToken)
        {
            var dt = new DataTable(tableName);
            dt = ConvertToDataTable(list);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
                {
                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE products_tmpCount (sku nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, count int NULL, updatedAt varchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL);", connection))
                    {
                        try
                        {
                            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

                            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                            {
                                bulkcopy.BulkCopyTimeout = 6600;
                                bulkcopy.DestinationTableName = "products_tmpCount";
                                bulkcopy.ColumnMappings.Clear();
                                bulkcopy.ColumnMappings.Add("sku", "sku");
                                bulkcopy.ColumnMappings.Add("count", "count");
                                bulkcopy.ColumnMappings.Add("updatedAt", "updatedAt");
                                await bulkcopy.WriteToServerAsync(dt.CreateDataReader(), cancellationToken).ConfigureAwait(false);
                                bulkcopy.Close();
                            }


                            command.CommandTimeout = 3000;
                            command.CommandText = "UPDATE P SET P.count = T.count FROM products AS P INNER JOIN products_tmpCount AS T ON P.sku = T.sku;";
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                            command.CommandText = "DROP TABLE products_tmpCount;";
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                            return ex.Message;
                        }
                        finally
                        {
                            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                            await connection.CloseAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
            return string.Empty;
        }
        //todo добавить везде транзакции!!
        public async Task<string> BulkUpdateCountExpressDataAsync(List<Product> productList, CancellationToken cancellationToken)
        {
            var dt = new DataTable(tableName);
            dt = ConvertToDataTable(productList);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
                {

                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE products_express_count (sku nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, countExpress int NULL, updateExpress datetime2(6) NULL);", connection))
                    {
                        try
                        {
                            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

                            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                            {
                                bulkcopy.BulkCopyTimeout = 6600;
                                bulkcopy.DestinationTableName = "products_express_count";
                                bulkcopy.ColumnMappings.Clear();
                                bulkcopy.ColumnMappings.Add("sku", "sku");
                                bulkcopy.ColumnMappings.Add("countExpress", "countExpress");
                                bulkcopy.ColumnMappings.Add("updateExpress", "updateExpress");
                                await bulkcopy.WriteToServerAsync(dt.CreateDataReader(), cancellationToken).ConfigureAwait(false);
                                bulkcopy.Close();
                            }


                            command.CommandTimeout = 3000;
                            command.CommandText = "UPDATE P SET P.countExpress = T.countExpress, P.updateExpress = T.updateExpress  FROM products AS P INNER JOIN products_express_count AS T ON P.sku = T.sku;";
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                            command.CommandText = "DROP TABLE products_express_count;";
                            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                        finally
                        {
                            await connection.CloseAsync().ConfigureAwait(false);
                        }

                    }

                    return string.Empty;
                }
            }
        }

        public async Task BulkUpdateTakeTimeAsync(List<Product> list, CancellationToken cancellationToken)
        {
            var dt = new DataTable(tableName);
            dt = ConvertToDataTable(list);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
                {
                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE products_tmpTakeTime (sku nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, takeTime datetime2(0) NULL);", connection))
                    {
                        try
                        {
                            await connection.OpenAsync();
                            await command.ExecuteNonQueryAsync();

                            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                            {
                                bulkcopy.BulkCopyTimeout = 6600;
                                bulkcopy.DestinationTableName = "products_tmpTakeTime";
                                bulkcopy.ColumnMappings.Clear();
                                bulkcopy.ColumnMappings.Add("sku", "sku");
                                bulkcopy.ColumnMappings.Add("takeTime", "takeTime");
                                await bulkcopy.WriteToServerAsync(dt.CreateDataReader());
                                bulkcopy.Close();
                            }


                            command.CommandTimeout = 3000;
                            command.CommandText = "UPDATE P SET P.takeTime = T.takeTime FROM products AS P INNER JOIN products_tmpTakeTime AS T ON P.sku = T.sku;";
                            await command.ExecuteNonQueryAsync();
                            command.CommandText = "DROP TABLE products_tmpTakeTime;";
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                        }
                        finally
                        {
                            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                            await connection.CloseAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        public async Task BulkUpdateExpressTakeTimeAsync(List<Product> list, CancellationToken cancellationToken)
        {
            var dt = new DataTable(tableName);
            dt = ConvertToDataTable(list);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
                {
                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE products_express_take (sku nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, takeTimeExpress datetime2(6) NULL);", connection))
                    {
                        try
                        {
                            await connection.OpenAsync();
                            await command.ExecuteNonQueryAsync();

                            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                            {
                                bulkcopy.BulkCopyTimeout = 6600;
                                bulkcopy.DestinationTableName = "products_express_take";
                                bulkcopy.ColumnMappings.Clear();
                                bulkcopy.ColumnMappings.Add("sku", "sku");
                                bulkcopy.ColumnMappings.Add("takeTimeExpress", "takeTimeExpress");
                                await bulkcopy.WriteToServerAsync(dt.CreateDataReader());
                                bulkcopy.Close();
                            }


                            command.CommandTimeout = 3000;
                            command.CommandText = "UPDATE P SET P.takeTimeExpress = T.takeTimeExpress FROM products AS P INNER JOIN products_express_take AS T ON P.sku = T.sku;";
                            await command.ExecuteNonQueryAsync();
                            command.CommandText = "DROP TABLE products_express_take;";
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                        }
                        finally
                        {
                            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                            await connection.CloseAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}