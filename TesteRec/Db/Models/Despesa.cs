using SQLite;

namespace TesteRec.Db.Models
{
    public class Despesa
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string Imagem { get; set; }
    }
    public static class DbDespesa
    {
        public static List<Despesa> _listaDespesa { get; set; } = new List<Despesa>()
        { 
            new Despesa()
            {
                Id = 1,
                Descricao = "Estacionamento",
            },
            new Despesa()
            {
                Id = 2,
                Descricao = "Financiamento",
            },
            new Despesa()
            {
                Id = 3,
                Descricao = "Impostos (IPVA/DPVAT)",
            },
            new Despesa()
            {
                Id = 4,
                Descricao = "Licenciamento",
            },
            new Despesa()
            {
                Id = 5,
                Descricao = "Multa",
            },
            new Despesa()
            {
                Id = 6,
                Descricao = "Pedágio",
            },
            new Despesa()
            {
                Id = 7,
                Descricao = "Reembolso",
            },
            new Despesa()
            {
                Id = 8,
                Descricao = "Seguro",
            },
        };
    }
}
