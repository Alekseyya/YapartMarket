using System.Collections.Generic;

namespace YapartMarket.Parser.Data.Implements
{
    public interface IAccessRepository<T> where T : class
    {
        IList<T> GetAll();
        T GetItemById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);

    }
}
