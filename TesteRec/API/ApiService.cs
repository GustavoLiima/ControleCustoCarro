using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TesteRec.API.Models;
using TesteRec.Db;

namespace TesteRec.API
{
    public static class ApiService
    {
        /// <summary>
        /// Ignorar Certificado SSL (para desenvolvimento): Configure o HttpClientHandler para ignorar a validação de certificados
        /// </summary>
        private static HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        };
        private static readonly HttpClient _httpClient = new HttpClient(handler);
        /// <summary>
        /// Para Android Emulator: Use 10.0.2.2 em vez de localhost
        /// Para Windows ou iOS Simulator: Use 127.0.0.1 em vez de localhost
        /// </summary>
        private static string _UrlAtual;

        public static async Task<TResult> SendRequestAsync<TData, TResult>(string url, HttpMethod method, TData data, bool pNecessarioToken)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(20);
            MontarURL(url);
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, _UrlAtual);
                if (pNecessarioToken)
                {
                    if (Global._Token == null || Global._DataToken.AddHours(24) < DateTime.Now)
                    {
                        var tokenService = new TokenService();
                        var tokenRequest = new TokenVM
                        {
                            user = Global._Usuario,
                            password = Global._Senha
                        };
                        await tokenService.GetTokenAsync(tokenRequest);
                    }
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Global._Token);
                }

                // Serializa o dado de entrada, se houver
                if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete))
                {
                    var json = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                // Envia a requisição
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Lê o conteúdo de retorno e desserializa para o tipo esperado
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<TResult>(jsonResult);
                    return result;
                }

                // Lida com erros de status da resposta
                throw new HttpRequestException($"Erro ao acessar a API: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                // Trate exceções
                throw new Exception($"Erro na requisição: {ex.Message}");
            }
        }

        private static void MontarURL(string pUrl)
        {
            _UrlAtual = Global.EnderecoApi + pUrl;
        }
    }

}