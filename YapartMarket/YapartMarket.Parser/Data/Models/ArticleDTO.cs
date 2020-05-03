using System.Collections.Generic;

namespace YapartMarket.Parser.Data.Models
{
    public class ArticleDTO
    {
        public string Brand { get; set; }
        public string Article { get; set; }
        public string Descriptions { get; set; }
        public IList<ShortInfo> ListSellers { get; set; }
        public IList<ShortInfo> Yapart { get; set; }
    }

}
