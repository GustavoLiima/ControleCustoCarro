using SQLite;

namespace TesteRec.Db.Models
{
    public class TipoServico
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Descricao { get; set; }
    }

    public static class DbTipoServico
    {
        public static List<TipoServico> _tipoServicos { get; set; } =
        new List<TipoServico>()
    {
        new TipoServico()
        {
            Id = 1,
            Descricao = "Ar condicionado",
        },
        new TipoServico()
        {
            Id = 2,
            Descricao = "Bateria",
        },
        new TipoServico()
        {
            Id = 3,
            Descricao = "Correias",
        },
        new TipoServico()
        {
            Id = 4,
            Descricao = "Filtro de ar",
        },
        new TipoServico()
        {
            Id = 5,
            Descricao = "Filtro de combustível",
        },
        new TipoServico()
        {
            Id = 6,
            Descricao = "Filtro de óleo",
        },
        new TipoServico()
        {
            Id = 7,
            Descricao = "Fluido de freio",
        },
        new TipoServico()
        {
            Id = 8,
            Descricao = "Lava-rápido",
        },
        new TipoServico()
        {
            Id = 9,
            Descricao = "Luzes",
        },
        new TipoServico()
        {
            Id = 10,
            Descricao = "Mão de obra",
        },
        new TipoServico()
        {
            Id = 11,
            Descricao = "Outros",
        },
        new TipoServico()
        {
            Id = 12,
            Descricao = "Pastilha de freio",
        },
        new TipoServico()
        {
            Id = 13,
            Descricao = "Pneus",
        },
        new TipoServico()
        {
            Id = 14,
            Descricao = "Revisão",
        },
        new TipoServico()
        {
            Id = 15,
            Descricao = "Suspensão",
        },
        new TipoServico()
        {
            Id = 16,
            Descricao = "Troca de óleo",
        },
    };
    }
}
