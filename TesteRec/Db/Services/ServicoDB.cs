using SQLite;
using TesteRec.Db.Models;
using TesteRec.Enum;

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
            return await _database.InsertOrReplaceAsync(pTipoServico);
        }

        public async Task<List<TipoServico>> GetTipoServicoPorIDAsync(int pId)
        {
            return await _database.Table<TipoServico>().Where(x => x.IdServico == pId).ToListAsync();
        }

        public async Task<string> GetDescricaoServicoAsync(int pId)
        {
            // Busca a lista de serviços filtrada
            var servicos = await _database.Table<TipoServico>()
                                          .Where(x => x.IdServico == pId)
                                          .ToListAsync();

            // Verifica se há pelo menos um serviço
            if (servicos == null || !servicos.Any())
                return string.Empty; // Retorna vazio se não houver nenhum serviço

            // Pega a descrição do primeiro serviço
            string descricao = servicos[0].Descricao;

            // Verifica se há mais de um serviço
            if (servicos.Count > 1)
            {
                int quantidadeExtra = servicos.Count - 1;
                return $"{descricao} (+{quantidadeExtra})";
            }

            // Se houver apenas um serviço, retorna a descrição
            return descricao;
        }

        // Adiciona um novo serviço
        public async Task<int> AddServicoAsync(Servico servico)
        {
            servico.idVeiculo = Global.carroSelecionado.ID;
            if (servico.Id == 0)
            {
                await _database.InsertAsync(servico);
            }
            else
            {
                await _database.UpdateAsync(servico);
            }
            return servico.Id;
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
            .Where(s => s.Data >= pDataInicial && s.Data <= pDataFinal && s.AcaoSelecionada != (int)EMenuSelecionado.Lembrete && s.idVeiculo == pVeiculo)
            .ToListAsync();
            return listaServicos;
        }

        public async Task<List<Servico>> GetServicosEntreDatasDespesas(DateTime pDataInicial, DateTime pDataFinal, int pVeiculo)
        {
            var listaServicos = await _database.Table<Servico>()
            .Where(s => s.Data >= pDataInicial && s.Data <= pDataFinal && s.AcaoSelecionada == (int)EMenuSelecionado.Despesa && s.idVeiculo == pVeiculo)
            .ToListAsync();
            return listaServicos;
        }
        public async Task<List<Servico>> GetServicosEntreDatasGanhos(DateTime pDataInicial, DateTime pDataFinal, int pVeiculo)
        {
            var listaServicos = await _database.Table<Servico>()
            .Where(s => s.Data >= pDataInicial && s.Data <= pDataFinal && s.AcaoSelecionada == (int)EMenuSelecionado.Receita && s.idVeiculo == pVeiculo)
            .ToListAsync();
            return listaServicos;
        }

        public async Task<List<Servico>> GetServicosEntreDatasServico(DateTime pDataInicial, DateTime pDataFinal, int pVeiculo)
        {
            var listaServicos = await _database.Table<Servico>()
            .Where(s => s.Data >= pDataInicial && s.Data <= pDataFinal && s.AcaoSelecionada == (int)EMenuSelecionado.Serviço && s.idVeiculo == pVeiculo)
            .ToListAsync();
            return listaServicos;
        }

        public async Task<List<Servico>> GetServicosEntreDatasAbastecimento(DateTime pDataInicial, DateTime pDataFinal, int pVeiculo)
        {
            var listaServicos = await _database.Table<Servico>()
            .Where(s => s.Data >= pDataInicial && s.Data <= pDataFinal && s.AcaoSelecionada == (int)EMenuSelecionado.Abastecimento && s.idVeiculo == pVeiculo)
            .ToListAsync();
            return listaServicos;
        }

        public async Task<Servico> GetUltimoAbastecimento()
        {
            return await _database.Table<Servico>().Where(s => s.AcaoSelecionada == (int)EMenuSelecionado.Abastecimento).OrderByDescending(s => s.Data).FirstOrDefaultAsync();
        }

        public async Task<Servico> GetUltimoGasto()
        {
            return await _database.Table<Servico>().Where(s => s.AcaoSelecionada == (int)EMenuSelecionado.Despesa).OrderByDescending(s => s.Data).FirstOrDefaultAsync();
        }
    }
}
