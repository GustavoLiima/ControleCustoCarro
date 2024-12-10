using SQLite;
using TesteRec.Db.Models;

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
            servico.idVeiculo = Global.carroSelecionado.ID;
            if(servico.Id == 0)
            {
                return await _database.InsertAsync(servico);
            }
            return await _database.InsertOrReplaceAsync(servico);

        }

        // Obtém todos os serviços, exceto lembretes
        public async Task<List<Servico>> GetServicosAsync(int pIdVeiculo)
        {
            if(pIdVeiculo == 0)
            {
                return null;
            }
            List<Servico> objRetorno = await _database.Table<Servico>().Where(x => x.AcaoSelecionada != 0 && x.idVeiculo == pIdVeiculo).OrderByDescending(x => x.Data).ToListAsync();
            if (objRetorno != null)
            {
                return objRetorno;
            }
            return null;
        }

        // Obtém todos os lembretes
        public async Task<List<Servico>> GetLembretesAsync()
        {
            List<Servico> objRetorno = await _database.Table<Servico>().Where(x => x.AcaoSelecionada == 0).ToListAsync();
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

        public async Task<List<Servico>> GetServicosEntreDatas(DateTime pDataInicial, DateTime pDataFinal, int pVeiculo)
        {
            var listaServicos = await _database.Table<Servico>()
            .Where(s => s.Data >= pDataInicial && s.Data <= pDataFinal && s.AcaoSelecionada != 0 && s.idVeiculo == pVeiculo)
            .ToListAsync();
            return listaServicos;
        }
    }
}
