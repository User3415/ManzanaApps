using Manzana.DAL.Interfaces;
using Manzana.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Manzana.DAL.Repositories
{
    public class ChequeRepositoryFakeDb: IChequeRepository
    {
        private static Random _rnd;
        public ChequeRepositoryFakeDb()
        {
            _rnd = new Random();

        }

        public async Task<List<Cheque>> GetByCount(int count)
        {
            List<Cheque> cheques = new List<Cheque>();
            for (int i = 0; i < count; i++)
            {
                cheques.Add(new Cheque() { ChequeId = _rnd.Next(1, count), ChequeNumber = _rnd.Next(1, count) });
            }
            return cheques;
        }

        public async Task<Cheque> Insert(Cheque entity)
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, "cheque.txt");
            var json = JsonConvert.SerializeObject(entity);
            entity.ChequeId = _rnd.Next(1, 100);
            File.WriteAllText(path, json);

            return entity;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
