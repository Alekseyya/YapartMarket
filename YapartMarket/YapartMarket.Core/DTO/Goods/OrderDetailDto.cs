using YapartMarket.Core.DateStructures;

namespace YapartMarket.Core.DTO.Goods
{
    public  sealed class OrderDetailDto
    {
        public string GoodsId { get; set; }
        public string OfferId { get; set; }
        public string ItemIndex { get; set; }
        public ReasonType ReasonType { get; set; }
    }
}
