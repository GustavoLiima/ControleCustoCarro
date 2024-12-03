namespace TesteRec.API.Models
{
    public class RecuperacaoSenhaRequest
    {
        public string Email { get; set; }
        public string Codigo { get; set; }
        public string NovaSenha { get; set; }
    }
}
