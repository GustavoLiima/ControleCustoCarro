using TesteRec.API.Models;

namespace TesteRec.API.Communic
{
    public class RecuperacaoSenhaCommunic
    {
        private string Endereco = "/api/v1/RecuperacaoSenha";

        public async Task<ApiResponse<string>> SolicitarCodigo(string pEmail)
        {
            return await ApiService.SendRequestAsync<string, string>(Endereco + "/solicitar-codigo", HttpMethod.Post, pEmail, false);
        }
        public async Task<ApiResponse<string>> RedefinirSenha(RecuperacaoSenhaRequest pObj)
        {
            return await ApiService.SendRequestAsync<RecuperacaoSenhaRequest, string>(Endereco + "/redefinir-senha", HttpMethod.Post, pObj, false);
        }
    }
}
