using TesteRec.Db;
using TesteRec.Layouts.crud;
using TesteRec.Layouts.Iniciais;
using TesteRec.Layouts.listas;
using TesteRec.Layouts.Templates;

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
        new Menu()
        {
            Imagem = "calculator.png",
            Descricao = "Calculadora flex"
        },
        new Menu()
        {
            Imagem = "sairusuario.png",
            Descricao = "Sair"
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

    private async void NavegarParaTela(string pTela)
    {
        switch (pTela)
        {
            case "Veículos":
                await Navigation.PushAsync(new listaVeiculos());
                break;
            case "Combustíveis":
                await Navigation.PushAsync(new TiposCombustivel());
                break;
            case "Tipos de serviço":
                await Navigation.PushAsync(new TiposServico());
                break;
            case "Formas de pagamento":
                await Navigation.PushAsync(new FormasPagamento());
                break;
            case "Tipos de custos":
                await Navigation.PushAsync(new TiposDespesas());
                break;
            case "Tipos de receitas":
                await Navigation.PushAsync(new TipoReceita());
                break;
            case "Calculadora flex":
                await Navigation.PushAsync(new CalculadoraFlex());
                break;
            case "Sair":
                if(await DisplayAlert("Atenção", "Você tem certeza que deseja deslogar?", "confirmar", "cancelar"))
                {
                    Global._login = null;
                    SecureStorage.Default.Remove("login");
                    await Navigation.PushAsync(new PaginaLogin());
                }
                break;
        }
    }
}

public class Menu
{
    public string Imagem { get; set; }
	public string Descricao { get; set; }
}