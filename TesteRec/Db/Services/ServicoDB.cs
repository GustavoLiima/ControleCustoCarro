using SQLite;
using TesteRec.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteRec.Db.Services
{
    public class ServicoDB
    {
        private readonly SQLiteAsyncConnection _database;

        public ServicoDB()
        {
            _database = BaseDB.GetDatabaseConnection();
        }

        // Adiciona um novo tipo de serviço
        public async Task<int> AddTipoServicoAsync(TipoServico pTipoServico)
        {
            return await _database.InsertAsync(pTipoServico);
        }

        // Adiciona um novo serviço
        public async Task<int> AddServicoAsync(Servico servico)
        {
            if(servico.Id == 0)
            {
                return await _database.InsertAsync(servico);
            }
            return await _database.InsertOrReplaceAsync(servico);

        }

        // Obtém todos os serviços
        public async Task<List<Servico>> GetServicosAsync()
        {
            List<Servico> objRetorno = await _database.Table<Servico>().ToListAsync();
            if (objRetorno != null)
            {
                return objRetorno;
            }
            return null;
        }

        // Atualiza um serviço existente
        public async Task<int> UpdateServicoAsync(Servico servico)
        {
            return await _database.UpdateAsync(servico);
        }

        // Remove um serviço
        public async Task<int> DeleteServicoAsync(Servico servico)
        {
            return await _database.DeleteAsync(servico);
        }

        // Obtém um serviço por ID
        public async Task<Servico> GetServicoByIdAsync(int id)
        {
            return await _database.Table<Servico>().Where(s => s.Id == id).FirstOrDefaultAsync();
        }
    }
}
