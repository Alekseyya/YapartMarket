using YapartMarket.WebApi.ViewModel.Goods.Confirm;

namespace YapartMarket.WebApi.ViewModel.Goods.Packing
{
    public sealed class PackingResponse : SuccessResponse
    {
        public Goods.Data? data { get; set; }
        public Error error { get; set; }
    }
}
