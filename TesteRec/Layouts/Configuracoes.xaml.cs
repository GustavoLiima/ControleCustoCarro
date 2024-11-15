using TesteRec.Layouts.crud;
using TesteRec.Layouts.listas;

namespace TesteRec.Layouts;

public partial class Configuracoes : ContentPage
{
	List<Menu> _listaMenus = new List<Menu> 
	{
		new Menu()
		{
			Imagem = "carro.png",
			Descricao = "Veículos"
		},
        new Menu()
        {
            Imagem = "posto.png",
            Descricao = "Combustíveis"
        },
        new Menu()
        {
            Imagem = "oil.png",
            Descricao = "Tipos de serviço"
        },
        new Menu()
        {
            Imagem = "dinheiro.png",
            Descricao = "Formas de pagamento"
        },
        new Menu()
        {
            Imagem = "custos.png",
            Descricao = "Tipos de custos"
        },
        new Menu()
        {
            Imagem = "dinheiro.png",
            Descricao = "Tipos de receitas"
        },
    };
	public Configuracoes()
	{
		InitializeComponent();
        ListView_Interacoes.ItemsSource = _listaMenus;
	}

    private void ListView_Interacoes_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        // Verifica se o item é do tipo esperado
        var selectedItem = e.Item as Menu;
        if (selectedItem != null)
        {
            // Realiza uma operação com o item selecionado, por exemplo, exibe um alerta
            NavegarParaTela(selectedItem.Descricao);

            // Opcional: Limpa a seleção para remover o destaque visual
            ((ListView)sender).SelectedItem = null;
        }
    }

    private void NavegarParaTela(string pTela)
    {
        switch (pTela)
        {
            case "Veículos":
                Navigation.PushAsync(new CadastroVeiculo());
                break;
            case "Combustíveis":
                Navigation.PushAsync(new TiposCombustivel());
                break;
            case "Tipos de serviço":
                Navigation.PushAsync(new TiposServico());
                break;
            case "Formas de pagamento":
                Navigation.PushAsync(new FormasPagamento());
                break;
            case "Tipos de custos":
                Navigation.PushAsync(new TiposDespesas());
                break;
            case "Tipos de receitas":
                Navigation.PushAsync(new TipoReceita());
                break;
        }
    }
}

public class Menu
{
    public string Imagem { get; set; }
	public string Descricao { get; set; }
}