using YapartMarket.WebApi.ViewModel.Goods.Confirm;

namespace YapartMarket.WebApi.ViewModel.Goods.Reject
{
    public sealed class RejectResponse : SuccessResponse
    {
        public Goods.Data? data { get; set; }
        public List<Error> error { get; set; }
    }
}
