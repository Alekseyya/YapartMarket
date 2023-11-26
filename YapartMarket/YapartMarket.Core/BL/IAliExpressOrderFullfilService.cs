namespace YapartMarket.Core.BL
{
    public interface IAliExpressOrderFullfilService
    {
        bool OrderFullfil(string service, long orderId, long logisticNumber);
    }
}
