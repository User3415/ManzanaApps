using DevTrends.WCFDataAnnotations;
using log4net;
using Manzana.DAL.Interfaces;
using Manzana.Domain.Entities;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ManzanaWCF.Services
{
    ///<inheritdoc cref="IChequeService"/>
    [ValidateDataAnnotationsBehavior]
    public class ChequeService : IChequeService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IChequeRepository Repository { get; private set; }

        public ChequeService(IChequeRepository repository)
        {
            Repository = repository;
        }

        ///<inheritdoc cref="IChequeService"/>
        public async Task<IEnumerable<Cheque>> GetCheques(int count)
        {
            Log.Info("Received a request to receive checks");
            return await Repository.GetByCount(count);
        }

        ///<inheritdoc cref="IChequeService"/>
        public async Task<Cheque> SaveCheque(Cheque cheque)
        {
            Log.Info("Received a request to save the receipt");
            return await Repository.Insert(cheque);
        }
    }
}
