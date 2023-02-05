using YapartMarket.WebApi.ViewModel.Goods.Confirm;

namespace YapartMarket.WebApi.ViewModel.Goods.Packing
{
    public sealed class PackingResponse : SuccessResponse
    {
        public List<Error> error { get; set; }
    }
}
