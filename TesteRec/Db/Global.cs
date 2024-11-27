using TesteRec.Db.Models;

namespace TesteRec.Db
{
    public static class Global
    {
        public static Veiculo carroSelecionado { get; set; }
        public static string _Token;
        public static DateTime _DataToken;
        public static string EnderecoApi = "https://10.0.2.2:7115";
        public static string _Usuario;
        public static string _Senha;
    }
}
