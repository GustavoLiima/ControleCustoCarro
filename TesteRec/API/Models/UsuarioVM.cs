namespace TesteRec.API.Models
{
    public class UsuarioVM
    {
        public int cd_usuario { get; set; }
        public string usuario { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public bool ativo { get; set; }
    }
}
