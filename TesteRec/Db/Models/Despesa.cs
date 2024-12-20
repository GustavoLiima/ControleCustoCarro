using SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TesteRec.Db.Models
{
    public class Despesa : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdServico {  get; set; }
        public string Imagem { get; set; }

        public bool EnviadoServidor { get; set; }
        public string? Descricao
        {
            get => _descricao;
            set
            {
                if (_descricao != value)
                {
                    _descricao = value;
                    OnPropertyChanged(nameof(Descricao));
                }
            }
        }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        } // Para indicar se o serviço foi selecionado
        public double Valor
        {
            get => _valor;
            set
            {
                if (_valor != value)
                {
                    _valor = value;
                    OnPropertyChanged(nameof(Valor));
                }
            }
        } // Valor associado ao serviço selecionado
        [NotMapped]
        private bool _isSelected;
        [NotMapped]
        private string _descricao;
        [NotMapped]
        private double _valor;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
