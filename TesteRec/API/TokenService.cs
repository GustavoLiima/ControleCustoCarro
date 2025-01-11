using Newtonsoft.Json;
using System.Text;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Services;

namespace TesteRec.API
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;

        public TokenService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Global.EnderecoApi) // Substitua pelo seu endpoint base
            };
        }

        public async Task<ApiResponse<string>> GetTokenAsync(TokenVM tokenRequest)
        {
            try
            {
                // Serializando o objeto TokenVM em JSON
                var jsonContent = System.Text.Json.JsonSerializer.Serialize(tokenRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Configurando a requisição
                var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/token")
                {
                    Content = content // Adicionando o conteúdo JSON à requisição
                };

                // Enviando a requisição
                var response = await _httpClient.SendAsync(request);

                // Verificando se a resposta foi bem-sucedida
                if (response.IsSuccessStatusCode)
                {
                    TokenReturn objRet = JsonConvert.DeserializeObject<TokenReturn>(await response.Content.ReadAsStringAsync());
                    // Lendo o conteúdo da resposta como string
                    Global._DataToken = DateTime.Now;
                    Global._Token = objRet.Token;
                    Global._UsuarioSelecionado = objRet.Usuario;

                    if (objRet.veiculos != null && objRet.veiculos.Count != 0)
                    {
                        Global._Veiculos = objRet.veiculos;
                        
                    }

                    await SecureStorage.Default.SetAsync("usuario", JsonConvert.SerializeObject(objRet.Usuario));
                    return new ApiResponse<string>()
                    {
                        Sucesso = true,
                        Valor = Global._Token
                    };
                }
                else
                {
                    var teste = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<string>()
                    {
                        Sucesso = false,
                        Mensagem = await response.Content.ReadAsStringAsync()
                    };
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções
                return new ApiResponse<string>()
                {
                    Sucesso = false,
                    Mensagem = $"Verifique sua conexão e tente novamente\nCaso o erro persista, aguarde um pouco e tente novamente mais tarde"
                };
            }
        }
    }
}
