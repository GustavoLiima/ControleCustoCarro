using System.Text;
using System.Text.Json;
using TesteRec.API.Models;
using TesteRec.Db;

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
                var jsonContent = JsonSerializer.Serialize(tokenRequest);
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
                    // Lendo o conteúdo da resposta como string
                    Global._DataToken = DateTime.Now;
                    Global._Token = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<string>()
                    {
                        Sucesso = true,
                        Valor = Global._Token
                    };
                }
                else
                {
                    return new ApiResponse<string>()
                    {
                        Sucesso = false,
                        Mensagem = $"Erro ao obter o token: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"
                    };
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções
                return new ApiResponse<string>()
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao obter o token: {ex.Message}"
                };
            }
        }
    }
}
