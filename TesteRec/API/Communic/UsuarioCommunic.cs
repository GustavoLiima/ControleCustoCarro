using TesteRec.API.Models;

namespace TesteRec.API.Communic
{
    public class UsuarioCommunic
    {
        private string Endereco = "/api/v1/Usuario";

        public async Task<bool> CadastrarUsuario(UsuarioVM pUsuario)
        {
            return await ApiService.SendRequestAsync<UsuarioVM, bool>(Endereco, HttpMethod.Post, pUsuario, false);
        }
    }
}
