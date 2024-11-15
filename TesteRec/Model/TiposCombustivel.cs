using SQLite;

namespace TesteRec.Model
{
    public class TiposCombustivel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }
}
