using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
     public class AliExpressProduct
    {
        [Column("productId")]
        public long ProductId { get; set; }
        [Column("sku")]
        public string SKU { get; set; }
        [Column("inventory")]
        public int Inventory { get; set; }
        [Column("created")]
        public string Created { get; set; }
        [Column("updatedAt")]
        public string UpdatedAt { get; set; }
    }
}
