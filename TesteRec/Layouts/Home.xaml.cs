using Newtonsoft.Json;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;

namespace TesteRec.Layouts;

public partial class Home : ContentPage
{
    ServicoDB servicoService = new ServicoDB();

    public Home()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        
        CollectionView_Inclusoes.ItemsSource = DbMenu._menus;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        string veiculoSelecionado = await SecureStorage.Default.GetAsync("veiculoSelecionado");
        if (!string.IsNullOrEmpty(veiculoSelecionado))
        {
            Global.carroSelecionado = JsonConvert.DeserializeObject<Veiculo>(veiculoSelecionado);
        }
        carregarTela();
    }

    private async void carregarTela()
    {
        if(Global.carroSelecionado != null)
        {
            Label_VeiculoSelecionado.Text = Global.carroSelecionado.Nome;
            switch (Global.carroSelecionado.TipoVeiculo)
            {
                case "Carro":
                    Image_TipoVeiculo.Source = "carro.png";
                    break;
                case "Moto":
                    Image_TipoVeiculo.Source = "moto.png";
                    break;
                case "Onibus":
                    Image_TipoVeiculo.Source = "onibus.png";
                    break;
                case "Caminhao":
                    Image_TipoVeiculo.Source = "caminhao.png";
                    break;
                default:
                    break;
            }
            Label_QuilometragemAtual.Text = $"Última KM registrada: {Global.carroSelecionado.Quilometros}";
        }
        CollectionView_Servicos.ItemsSource = null;
        CollectionView_Servicos.ItemsSource = await servicoService.GetServicosAsync();
    }

    private void OnAddServiceClicked(object sender, EventArgs e)
    {
        PopupServiceSelection.IsVisible = true;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        carregarTela();
    }

    private void OnCancelPopupClicked(object sender, EventArgs e)
    {
        PopupServiceSelection.IsVisible = false;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Db.Models.Menu? selectedItem = e.CurrentSelection.FirstOrDefault() as Db.Models.Menu;

        if (selectedItem != null)
        {
            switch (selectedItem.Descricao)
            {
                case "Lembrete":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Lembrete));
                    break;
                case "Checklist":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Checklist));
                    break;
                case "Ganhos":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Receita));
                    break;
                case "Percurso":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Percurso));
                    break;
                case "Serviço":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Serviço));
                    break;
                case "Gastos":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Despesa));
                    break;
                case "Abastecimento":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Abastecimento));
                    break;
            }
            ((CollectionView)sender).SelectedItem = null;
            PopupServiceSelection.IsVisible = false;
        }
    }

    private async void RemoverClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var servico = button?.BindingContext as Servico;

        if (servico != null)
        {
            if (await DisplayAlert("Atenção", "Realmente deseja realizar a exclusão?", "Confirmar", "Cancelar"))
            {
                await servicoService.DeleteServicoAsync(servico);
                carregarTela();
            }
        }
    }

    private async void EditarClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var servico = button?.BindingContext as Servico;

        if (servico != null)
        {
            await Navigation.PushAsync(new InclusaoServico(servico));
        }
    }
}