using Dapper;
using Manzana.DAL.Interfaces;
using Manzana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Manzana.DAL.Repositories
{
    public class ChequeRepository : IChequeRepository
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public async Task<List<Cheque>> GetByCount(int count)
        {
            List<Cheque> cheques = new List<Cheque>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //db.QueryAsync<Cheque>("") add select top by count ordered by chequeId
            }
            return cheques;
        }

        public async Task<Cheque> Insert(Cheque entity)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var articles = string.Join(";", entity.Articles);

                //db.QueryAsync<Cheque>("") add insert sql command here
            }

            return entity;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
