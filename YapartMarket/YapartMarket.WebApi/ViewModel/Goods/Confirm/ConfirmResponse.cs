
namespace YapartMarket.WebApi.ViewModel.Goods.Confirm
{
    /// <inheritdoc />
    public sealed class ConfirmResponse : SuccessResponse
    {
        public Goods.Data? data { get; set; }
        public Error error { get; set; }
    }
}
