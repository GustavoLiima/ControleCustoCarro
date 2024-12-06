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
    }
}
