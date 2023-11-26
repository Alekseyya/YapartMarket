using YapartMarket.WebApi.ViewModel.Goods.Confirm;

namespace YapartMarket.WebApi.ViewModel.Goods.Shipping
{
    public sealed class ShippingResponse : SuccessResponse
    {
        public Error error { get; set; }
    }
}
