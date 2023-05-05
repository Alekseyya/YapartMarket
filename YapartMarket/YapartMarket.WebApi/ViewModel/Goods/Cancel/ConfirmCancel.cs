namespace YapartMarket.WebApi.ViewModel.Goods.Cancel
{
    public sealed class ConfirmCancel
    {
        public CancelData? data { get; set; }
        public int success { get; set; }
        public CancelMeta meta { get; set; }
    }
}
