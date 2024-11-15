using SQLite;

namespace TesteRec.Db.Models
{
    public class Veiculo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TipoVeiculo { get; set; }
        public string Nome { get; set; }
        public int Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Chassi { get; set; }
        public string Renavam { get; set; }
        public bool Quilometros { get; set; }
        public int TipoCombustivel1 { get; set; }
        public double VolumeCombustivel1 { get; set; }
        public int TipoCombustivel2 { get; set; }
        public double VolumeCombustivel2 { get; set; }
    }
}
