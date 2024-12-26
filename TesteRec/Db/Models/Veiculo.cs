using SQLite;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteRec.Db.Models
{
    public class Veiculo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TipoVeiculo { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Chassi { get; set; }
        public string Renavam { get; set; }
        public double Quilometros { get; set; }
        public int TipoCombustivel1 { get; set; }
        public double VolumeCombustivel1 { get; set; }
        public int TipoCombustivel2 { get; set; }
        public double VolumeCombustivel2 { get; set; }
        [NotMapped]
        public string Imagem
        {
            get
            {
                switch (TipoVeiculo)
                {
                    case "Carro":
                        return "carro.png";
                    case "Moto":
                        return "moto.png";
                    case "Ônibus":
                        return "onibus.png";
                    case "Caminhão":
                        return "caminhao.png";
                    case "Van":
                        return "van.png";
                    case "Bicicleta":
                        return "bicicleta.png";
                    case "Caminhonete":
                        return "caminhonete.png";
                    default:
                        return null;
                }
            }
        }
    }
}