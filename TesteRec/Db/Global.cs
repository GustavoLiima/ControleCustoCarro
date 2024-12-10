using TesteRec.API.Models;
using TesteRec.Db.Models;

namespace TesteRec.Db
{
    public static class Global
    {
        public static VeiculoModel carroSelecionado { get; set; }
        public static string _Token;
        public static DateTime _DataToken;
        public static string EnderecoApi = "https://apicontroledecusto-a3ehhqhmcvgdencd.brazilsouth-01.azurewebsites.net";
        public static string _Usuario;
        public static string _Senha;
        public static TokenVM _login;
        public static List<VeiculoModel> _Veiculos = new List<VeiculoModel>();
        public static UsuarioVM _UsuarioSelecionado { get; set; }
    }
}
