namespace TesteRec.API.Models
{
    public class TokenReturn
    {
        public UsuarioVM Usuario { get; set; }
        public string Token { get; set; }
        public List<VeiculoModel> veiculos { get; set; }
    }
}
