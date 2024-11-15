using TesteRec.Db.Models;
using TesteRec.Enum;

namespace TesteRec.Layouts.Templates
{
    public class HistoricoServicoTemplate : DataTemplateSelector
    {
        public DataTemplate GanhosTemplate { get; set; }
        public DataTemplate CustosTemplate { get; set; }
        public DataTemplate AbastecimentoTemplate { get; set; }
        public DataTemplate ServicoTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Servico itemData)
            {
                switch (itemData.AcaoSelecionada)
                {
                    case (int)EMenuSelecionado.Receita:
                        return GanhosTemplate;
                    case (int)EMenuSelecionado.Despesa:
                        return CustosTemplate;
                    case (int)EMenuSelecionado.Abastecimento:
                        return AbastecimentoTemplate;
                    case (int)EMenuSelecionado.Serviço:
                        return ServicoTemplate;
                    default:
                        return GanhosTemplate;
                }
            }
            return null;
        }
    }
}
