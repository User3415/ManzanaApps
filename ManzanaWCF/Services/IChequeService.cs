using Manzana.Domain.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace ManzanaWCF.Services
{
    /// <summary>
    /// Service for works with cheques
    /// </summary>
    [ServiceContract]
    public interface IChequeService
    {
        /// <summary>
        /// Save cheque to db
        /// </summary>
        /// <param name="cheque">Cheque model</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/SaveCheque", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        Task<Cheque> SaveCheque(Cheque cheque);

        /// <summary>
        /// Get all cheques by count
        /// </summary>
        /// <param name="count">Cheque counts</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetCheques?count={count}", ResponseFormat  = WebMessageFormat.Json, Method = "GET")]
        Task<IEnumerable<Cheque>> GetCheques(int count);
    }
}
