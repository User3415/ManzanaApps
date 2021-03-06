using Manzana.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manzana.DAL.Interfaces
{
    public interface IChequeRepository : IRepository<Cheque>
    {
        Task<List<Cheque>> GetByCount(int count);
        Task<Cheque> Insert(Cheque entity);
    }
}
