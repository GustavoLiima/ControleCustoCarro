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
        public string IdPlanoMensal { get; set; }
        public string IdPlanoSemestral { get; set; }
        public bool MostraBotoes
        {
            get
            {
                if(ValorMensal == "Grátis" || PlanoAtual)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool IsExpanded { get; set; } // Controle para o Expandable
    }
}
