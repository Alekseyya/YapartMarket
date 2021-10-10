using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YapartMarket.Core.Models.Azure
{
    public class AliExpressBaseEntity
    {
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("updated")]
        public DateTime Updated { get; set; }
    }
}
