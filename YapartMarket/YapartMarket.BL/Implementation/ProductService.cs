using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YapartMarket.Core.BL;
using YapartMarket.Core.Data;
using YapartMarket.Core.Data.Interfaces;
using YapartMarket.Core.Models;

namespace YapartMarket.BL.Implementation
{
    public class ProductService : GenericService<Product, int, IProductRepository>, IProductService
    {
        private readonly IConfiguration configuration;

        public ProductService(IRepositoryFactory repositoryFactory, IConfiguration configuration) : base(repositoryFactory)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this.configuration = configuration;
        }
        public IList<Product> GetAll()
        {
            return GetAll(null);
        }

        public IList<Product> GetAll(Expression<Func<Product, bool>> conditionFunc)
        {
            var productRepository = RepositoryFactory.GetRepository<IProductRepository>();
            IList<Product> products;
            if (conditionFunc != null)
            {
                products = productRepository.GetAll().AsQueryable().Where(conditionFunc).ToList();
            }
            return productRepository.GetAll();
        }

        sealed class UpdateGoodsProduct
        {
            public UpdateGoodsProduct(string sku, string goodsId, string offerId)
            {
                Sku = sku;
                GoodsId = goodsId;
                OfferId = offerId;
            }
            public string Sku { get; }
            public string GoodsId { get; }
            public string OfferId { get; }
        }

        public async Task UpdateGoodsIdFromProducts()
        {
            var products = new List<UpdateGoodsProduct>();
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(@"C:\TMP\Готовые связки-2023-05-05.xlsx")))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                var sb = new StringBuilder(); //this is your data
                for (int rowNum = 2; rowNum <= totalRows; rowNum++) //select ;starting row here
                {
                    var cellGoodsId = myWorksheet.Cells[rowNum, 3].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();
                    var cellOfferId = myWorksheet.Cells[rowNum, 2].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();
                    var cellSku = myWorksheet.Cells[rowNum, 12].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();
                    if (!products.Any(x => x.OfferId == cellOfferId))
                        products.Add(new(cellSku, cellGoodsId, cellOfferId));
                }
            }
            using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var updateSql = @"update products set goodsId = @goodsId, offerId = @offerId where sku = @sku;";
                    foreach (var product in products)
                    {
                        await connection.ExecuteAsync(updateSql, new { goodsId = product.GoodsId, offerId = product.OfferId, sku = product.Sku }, transaction).ConfigureAwait(false);
                    }
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }
        }
        public async Task UpdateAliProdutId()
        {
            var products = new Dictionary<long, string>();
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(@"C:\MyOwn\актуальные артикулы али.xlsx")))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                var sb = new StringBuilder(); //this is your data
                for (int rowNum = 4; rowNum <= totalRows; rowNum++) //select ;starting row here
                {
                    var cellProductId = long.Parse(myWorksheet.Cells[rowNum, 1].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault());
                    var cellSku = myWorksheet.Cells[rowNum, 2].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();
                    if (!products.ContainsKey(cellProductId))
                        products.Add(cellProductId, cellSku);
                }
            }
            using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var updateSql = @"update products set aliExpressProductId = @aliExpressProductId where sku = @sku;";
                    foreach (var product in products)
                    {
                        await connection.ExecuteAsync(updateSql, new { aliExpressProductId = product.Key, sku = product.Value }, transaction).ConfigureAwait(false);
                    }
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
