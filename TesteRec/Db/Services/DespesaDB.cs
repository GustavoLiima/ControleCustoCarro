using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteRec.Db.Models;
using TesteRec.Db.Services;

namespace Cofauto.Db.Services
{
    public class DespesaDB
    {
        private readonly SQLiteAsyncConnection _database;

        public DespesaDB()
        {
            _database = BaseDB.GetDatabaseConnection();
        }

        public async Task<int> AddDespesaAsync(Despesa pTipoServico)
        {
            return await _database.InsertOrReplaceAsync(pTipoServico);
        }
    }
}
