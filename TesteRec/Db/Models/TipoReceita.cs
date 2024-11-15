using SQLite;

namespace TesteRec.Db.Models
{
    public class TipoReceita
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string Imagem { get; set; }
    }
    public static class DbTipoReceita
    {
        public static List<TipoReceita> _tipoReceitas = new List<TipoReceita>()
        {
            new TipoReceita()
            {
                Id = 1,
                Descricao = "Aplicativo de transporte",
            },
            new TipoReceita()
            {
                Id = 2,
                Descricao = "Carona",
            },
            new TipoReceita()
            {
                Id = 3,
                Descricao = "Frete",
            },
            new TipoReceita()
            {
                Id = 4,
                Descricao = "Reembolso",
            },
            new TipoReceita()
            {
                Id = 5,
                Descricao = "Venda do veículo",
            },
        };
    }
}
