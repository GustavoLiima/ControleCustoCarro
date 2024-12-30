namespace Cofauto.Db.Models
{
    public class Plano
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Beneficios { get; set; }
        public string ValorMensal { get; set; }
        public string ValorAnual { get; set; }
        public bool PlanoAtual { get; set; }
        public bool IsExpanded { get; set; } // Controle para o Expandable
    }
}
