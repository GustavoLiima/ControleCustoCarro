using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TesteRec.API.Models
{
    public class VeiculoModel
    {
        [PrimaryKey]
        public int ID { get; set; }

        public string NomeVeiculo { get; set; }

        public string Marca { get; set; }

        public string Placa { get; set; }

        public int TipoVeiculo { get; set; }

        public int? TipoCombustivel { get; set; }

        public int? CapacidadeTanque { get; set; }

        public int? Kilometragem { get; set; }

        public int? AnoFabricacao { get; set; }

        public int? AnoModelo { get; set; }

        public string? Chassi { get; set; }

        public string? Renavam { get; set; }

        public string? Anotacoes { get; set; }

        [NotMapped]
        public string Imagem
        {
            get
            {
                switch (TipoVeiculo)
                {
                    case 0:
                        return "carro.png";
                    case 1:
                        return "moto.png";
                    case 2:
                        return "caminhao.png";
                    case 3:
                        return "onibus.png";
                    default:
                        return null;
                }
            }
        }
    }
}
