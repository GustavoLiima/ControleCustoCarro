using TesteRec.API.Models;

namespace TesteRec.API.Communic
{
    public class UsuarioCommunic
    {
        private string Endereco = "/api/v1/Usuario";

        public async Task<ApiResponse<UsuarioVM>> CadastrarUsuario(UsuarioVM pUsuario)
        {
            return await ApiService.SendRequestAsync<UsuarioVM, UsuarioVM>(Endereco, HttpMethod.Post, pUsuario, false);
        }
        public async Task<ApiResponse<UsuarioVM>> AtualizarUsuario(UsuarioVM pUsuario)
        {
            return await ApiService.SendRequestAsync<UsuarioVM, UsuarioVM>(Endereco + "/AtualizarUsuario", HttpMethod.Post, pUsuario, true);
        }
        public async Task<ApiResponse<string>> AlterarSenhaUsuario(string pSenha)
        {
            return await ApiService.SendRequestAsync<string, string>(Endereco + "/AtualizarSenha", HttpMethod.Put, pSenha, true);
        }
    }
}
