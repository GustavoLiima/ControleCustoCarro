using TesteRec.API.Models;

namespace TesteRec.API.Communic
{
    public class VeiculoController
    {
        private string Endereco = "/api/v1/Veiculo";

        public async Task<ApiResponse<VeiculoModel>> CriarVeiculo(VeiculoModel pVeiculo)
        {
            return await ApiService.SendRequestAsync<VeiculoModel, VeiculoModel>(Endereco + "/criar", HttpMethod.Post, pVeiculo, true);
        }
        public async Task<ApiResponse<string>> AtualizarVeiculo(VeiculoModel pVeiculo)
        {
            return await ApiService.SendRequestAsync<VeiculoModel, string>(Endereco + "/atualizar", HttpMethod.Put, pVeiculo, true);
        }

        public async Task<ApiResponse<VeiculoModel>> DesabilitarVeiculo(VeiculoModel pVeiculo)
        {
            return await ApiService.SendRequestAsync<VeiculoModel, VeiculoModel>(Endereco + "/Desabilitar", HttpMethod.Delete, pVeiculo, true);
        }
    }
}
