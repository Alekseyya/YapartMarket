using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using YapartMarket.Parser.Data.Implements;
using YapartMarket.Parser.Data.Models;

namespace YapartMarket.Parser.Data
{
    public class AccessOutputRepository : IAccessOutputRepository
    {
        private readonly AccessContext _context;
        public AccessOutputRepository()
        {
            _context = new AccessContext();
        }

        public void AddListProducts(IList<OutputInfo> listProducts)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(_context._connectionString))
                {
                    string sqlExpression = "INSERT INTO Parcer_Output_Data (Brand, Article, 1Price, 1Count, 1Days, 2Price, 2Count, 2Days, 3Price, 3Count, 3Days, YourPrice, YourCount, YourDays)" +
                          "VALUES (@Brand, @Article, @1Price, @1Count, @1Days, @2Price, @2Count, @2Days, @3Price, @3Count, @3Days, @YourPrice, @YourCount, @YourDays)";
                    connection.Open();

                    foreach (var info in listProducts)
                    {
                        using (OleDbCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = sqlExpression;
                            cmd.Parameters.AddRange(new OleDbParameter[]{
                            new OleDbParameter("@Brand", info.Brand),
                            new OleDbParameter("@Article", info.Article),
                            new OleDbParameter("@1Price", info.FirstPrice),
                            new OleDbParameter("@1Count", info.FirstCount),
                            new OleDbParameter("@1Days", info.FirstDays),
                            new OleDbParameter("@2Price", info.SecondPrice),
                            new OleDbParameter("@2Count", info.SecondCount),
                            new OleDbParameter("@2Days", info.SecondDays),
                            new OleDbParameter("@3Price", info.ThirdPrice),
                            new OleDbParameter("@3Count", info.ThirdCount),
                            new OleDbParameter("@3Days", info.ThirdDays),
                            new OleDbParameter("@YourPrice", info.YourPrice),
                            new OleDbParameter("@YourCount", info.YourCount),
                            new OleDbParameter("@YourDays", info.YourDays) });
                            cmd.ExecuteNonQuery();
                        }
                    }
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Create(OutputInfo item)
        {
            throw new NotImplementedException();
        }

        public void Delete(OutputInfo item)
        {

        }

        public void DeleteTable()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(_context._connectionString))
                {
                    connection.Open();
                    using (OleDbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "DELETE from OutputData";
                        cmd.ExecuteNonQuery();
                    };

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<OutputInfo> GetAll()
        {
            string sqlQuery = "Select * from Parcer_Output_Data";
            List<OutputInfo> listProducts = new List<OutputInfo>();
            using (OleDbConnection connection = new OleDbConnection(_context._connectionString))
            {
                connection.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                foreach (var row in ds.Tables[0].AsEnumerable())
                {

                    var brand = row.ItemArray[1].ToString();
                    var article = row.ItemArray[2].ToString();
                    var firstPrice = (int)row.ItemArray[3];
                    var firstCount = (int)row.ItemArray[4];
                    var firstDays = (int)row.ItemArray[5];
                    var secondPrice = (int)row.ItemArray[6];
                    var secondCount = (int)row.ItemArray[7];
                    var seconsDays = (int)row.ItemArray[8];
                    var thirdPrice = (int)row.ItemArray[9];
                    var thirdCount = (int)row.ItemArray[10];
                    var thirdDays = (int)row.ItemArray[11];
                    listProducts.Add(new OutputInfo()
                    {
                        Brand = brand,
                        Article = article,
                        FirstPrice = firstPrice,
                        FirstCount = firstCount,
                        FirstDays = firstDays,
                        SecondPrice = secondPrice,
                        SecondCount = secondCount,
                        SecondDays = seconsDays,
                        ThirdPrice = thirdPrice,
                        ThirdCount = thirdCount,
                        ThirdDays = thirdDays
                    });
                }
            }
            return listProducts;
        }

        public OutputInfo GetItemById(int id)
        {
            throw new NotImplementedException();
        }

        public void TruncateTable()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(_context._connectionString))
                {
                    connection.Open();
                    using (OleDbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "TRUNCATE TABLE OutputData";
                        cmd.ExecuteNonQuery();
                    };

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Update(OutputInfo item)
        {
            throw new NotImplementedException();
        }
    }
}
