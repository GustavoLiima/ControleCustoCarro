using SQLite;

namespace TesteRec.Model
{
    public class Pagamentos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }
}
