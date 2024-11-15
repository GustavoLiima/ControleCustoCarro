using SQLite;
using TesteRec.Db.Models;

namespace TesteRec.Db.Services
{
    public class VeiculoDB
    {
        private readonly SQLiteAsyncConnection _database;

        public VeiculoDB()
        {
            _database = BaseDB.GetDatabaseConnection();
        }

        // Adiciona um novo veiculo
        public async Task<int> AddVeiculoAsync(Veiculo pVeiculo)
        {
            return await _database.InsertAsync(pVeiculo);
        }

        // Obtém todos os veiculos
        public async Task<List<Veiculo>> GetVeiculosAsync()
        {
            return await _database.Table<Veiculo>().ToListAsync();
        }

        // Remove um veiculo
        public async Task<int> DeleteVeiculoAsync(Veiculo pVeiculo)
        {
            return await _database.DeleteAsync(pVeiculo);
        }
    }
}
