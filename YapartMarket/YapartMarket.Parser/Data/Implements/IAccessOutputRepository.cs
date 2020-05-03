using System;
using System.Collections.Generic;
using System.Text;
using YapartMarket.Parser.Data.Models;

namespace YapartMarket.Parser.Data.Implements
{
    public interface IAccessOutputRepository : IAccessRepository<OutputInfo>
    {
        void AddListProducts(IList<OutputInfo> listProducts);
        void DeleteTable();
        void TruncateTable();
    }
}
