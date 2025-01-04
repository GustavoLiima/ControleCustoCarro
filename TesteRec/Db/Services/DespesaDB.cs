using SQLite;
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

        public async Task<List<Despesa>> GetTipoDespesaPorIDAsync(int pId)
        {
            return await _database.Table<Despesa>().Where(x => x.IdServico == pId).ToListAsync();
        }
    }
}
