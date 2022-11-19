using System;
using System.Collections.Generic;

namespace YapartMarket.Core.DTO.Yandex
{
    public class StockDto
    {
        public Int64 WarehouseId { get; set; }
        public List<string> Skus { get; set; }
    }
}