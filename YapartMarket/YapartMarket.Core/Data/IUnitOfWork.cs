namespace YapartMarket.Core.Data
{
    public interface IUnitOfWork
    {
        void EnsureTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
