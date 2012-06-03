using System.Collections.Generic;

namespace PizzaModel.Repos
{
    public interface IRepo<T>
    {
        void Save(T entity);
        T Get(int id);
        IList<T> GetAll();
        void Delete(int id);
        void Delete(T entity);
    }
}