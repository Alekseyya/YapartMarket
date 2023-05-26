namespace YapartMarket.WebApi.Model.Goods
{
    public sealed class OrdersViewModel
    {
        public OrdersViewModel(List<OrderViewModel> orders)
        {
            Orders = orders;
        }
        public List<OrderViewModel> Orders { get; }
    }
}
