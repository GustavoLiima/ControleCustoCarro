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

        public async Task<string> GetTokenAsync(TokenVM tokenRequest)
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
                    return Global._Token = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Lançar uma exceção caso a resposta seja uma falha
                    throw new HttpRequestException($"Erro ao obter o token: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções
                Console.WriteLine($"Erro: {ex.Message}");
                throw;
            }
        }
    }
}
