using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using YapartMarket.Parser.Data.Implements;
using YapartMarket.Parser.Data.Models;

namespace YapartMarket.Parser.Data
{
    public class AccessInputRepository : IAccessRepository<Product>
    {
        private readonly AccessContext _context;
        public AccessInputRepository()
        {
            _context = new AccessContext();
        }
        public void Create(Product item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product item)
        {
            throw new NotImplementedException();
        }

        public IList<Product> GetAll()
        {
            string sqlQuery = "Select * from Parcer_Sheriff";
            List<Product> listProducts = new List<Product>();
            using (OleDbConnection connection = new OleDbConnection(_context._connectionString))
            {
                connection.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                int id = 0;
                foreach (var row in ds.Tables[0].AsEnumerable())
                {
                    id++;
                    var brand = row.ItemArray[0].ToString();
                    var article = row.ItemArray[1].ToString();
                    listProducts.Add(new Product() { Id = id, Brand = brand, Article = article });
                }
            }
            return listProducts;
        }

        public Product GetItemById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
