using SQLite;
using TesteRec.API.Models;
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
        public async Task<int> AddVeiculoAsync(VeiculoModel pVeiculo)
        {
            return await _database.InsertAsync(pVeiculo);
        }

        public async Task<int> UpdateVeiculoAsync(VeiculoModel pVeiculo)
        {
            return await _database.UpdateAsync(pVeiculo);
        }

        // Obtém todos os veiculos
        public async Task<List<VeiculoModel>> GetVeiculosAsync()
        {
            return await _database.Table<VeiculoModel>().ToListAsync();
        }

        // Remove um veiculo
        public async Task<int> DeleteVeiculoAsync(VeiculoModel pVeiculo)
        {
            return await _database.DeleteAsync(pVeiculo);
        }
    }
}
