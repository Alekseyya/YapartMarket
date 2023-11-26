namespace YapartMarket.Core.Data
{
   public interface IRepositoryFactory
    {
        IRepository GetRepository<IRepository>();
    }
}
