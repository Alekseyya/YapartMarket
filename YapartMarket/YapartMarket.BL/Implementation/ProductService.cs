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
        public async Task UpdateGoodsIdFromProducts()
        {
            var dic = new Dictionary<string, string>();
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(@"C:\TMP\Готовые связки-2023-03-06.xlsx")))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                var sb = new StringBuilder(); //this is your data
                var cell = myWorksheet.Cells[9130, 2].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                if (string.IsNullOrEmpty(cell.FirstOrDefault()))
                    Console.WriteLine(true);
                Console.WriteLine(cell);
                for (int rowNum = 2; rowNum <= totalRows; rowNum++) //select ;starting row here
                {
                    var cellGoodsId = myWorksheet.Cells[rowNum, 3].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();
                    var cellId = myWorksheet.Cells[rowNum, 12].Select(c => c.Value == null ? string.Empty : c.Value.ToString()).FirstOrDefault();
                    if(!dic.ContainsKey(cellGoodsId))
                        dic.Add(cellGoodsId, cellId);
                }
            }
            using (var connection = new SqlConnection(configuration.GetConnectionString("SQLServerConnectionString")))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var updateSql = @"update products set goodsId = @goodsId where sku = @sku;";
                    foreach (var item in dic)
                    {
                        await connection.ExecuteAsync(updateSql, new { goodsId = item.Key, sku = item.Value }, transaction).ConfigureAwait(false);
                    }
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
