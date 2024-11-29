namespace TesteRec.API.Models
{
    public class ApiResponse<T>
    {
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
        public T? Valor { get; set; }
    }
}
