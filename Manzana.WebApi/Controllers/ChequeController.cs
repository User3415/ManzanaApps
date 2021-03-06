using log4net;
using Manzana.DAL.Interfaces;
using Manzana.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Manzana.WebApi.Controllers
{
    /// <summary>
    /// Cheque base controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChequeController : ControllerBase
    {
        private readonly IChequeRepository _chequeRepository;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Cheque constructor
        /// </summary>
        /// <param name="chequeRepository"></param>
        public ChequeController(IChequeRepository chequeRepository)
        {
            _chequeRepository = chequeRepository;
        }
        /// <summary>
        /// Get cheques
        /// </summary>
        /// <param name="count">Cheque counts</param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult> GetChequesByCount(int count)
        {
            Log.Info("Received a request to receive checks");

            return Ok(await _chequeRepository.GetByCount(count));
        }

        /// <summary>
        /// Save cheque
        /// </summary>
        /// <returns></returns>
        [HttpPost("saveCheque")]
        public async Task<ActionResult> GetChequesByCount(Cheque cheque)
        {
            Log.Info("Received a request to save the receipt");
            
            return Ok(await _chequeRepository.Insert(cheque));
        }
    }
}