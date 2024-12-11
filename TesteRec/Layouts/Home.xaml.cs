using Newtonsoft.Json;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;

namespace TesteRec.Layouts;

public partial class Home : ContentPage
{
    ServicoDB servicoService = new ServicoDB();
    VeiculoDB veiculoDB = new VeiculoDB();
    private bool _CarregouBase = false;

    public Home()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        var menu = DbMenu._menus;
        CollectionView_Inclusoes.ItemsSource = DbMenu._menus;
    }

    private async Task CarregarInformacaoInicial()
    {
        if(Global._Veiculos.Count == 0)
        {
            Global._Veiculos = await veiculoDB.GetVeiculosAsync();
        }
        string veiculoSelecionado = await SecureStorage.Default.GetAsync("veiculoSelecionado");
        if (!string.IsNullOrEmpty(veiculoSelecionado))
        {
            Global.carroSelecionado = JsonConvert.DeserializeObject<VeiculoModel>(veiculoSelecionado);
        }
        else
        {
            Global.carroSelecionado = Global._Veiculos[0];
            await SecureStorage.Default.SetAsync("veiculoSelecionado", JsonConvert.SerializeObject(Global.carroSelecionado));
        }
        string usuario = await SecureStorage.Default.GetAsync("usuario");
        if (!string.IsNullOrEmpty(usuario))
        {
            Global._UsuarioSelecionado = JsonConvert.DeserializeObject<UsuarioVM>(usuario);
        }
        _CarregouBase = true;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if(!_CarregouBase)
        {
            await CarregarInformacaoInicial();
        }
        carregarTela();
    }

    private async void carregarTela()
    {
        if(Global.carroSelecionado != null)
        {
            Label_VeiculoSelecionado.Text = Global.carroSelecionado.NomeVeiculo;
            switch (Global.carroSelecionado.TipoVeiculo)
            {
                case 0:
                    Image_TipoVeiculo.Source = "carro.png";
                    break;
                case 1:
                    Image_TipoVeiculo.Source = "moto.png";
                    break;
                case 2:
                    Image_TipoVeiculo.Source = "onibus.png";
                    break;
                case 3:
                    Image_TipoVeiculo.Source = "caminhao.png";
                    break;
                case 4:
                    Image_TipoVeiculo.Source = "van.png";
                    break;
                case 5:
                    Image_TipoVeiculo.Source = "bicicleta.png";
                    break;
                case 6:
                    Image_TipoVeiculo.Source = "caminhonete.png";
                    break;
                default:
                    break;
            }
            Label_QuilometragemAtual.Text = $"Última KM registrada: {Global.carroSelecionado.Kilometragem}";
        }
        if(Global.carroSelecionado != null)
        {
            var objCarregar = await servicoService.GetServicosAsync(Global.carroSelecionado.ID);
            CollectionView_Servicos.ItemsSource = null;
            CollectionView_Servicos.ItemsSource = objCarregar;
        }
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