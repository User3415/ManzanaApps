using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manzana.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<List<T>> GetByCount(int count);
        Task<T> Insert(T entity);
    }
}
