using Cofauto.Layouts.Templates;
using Cofauto.Model.Enum;
using CommunityToolkit.Maui.Views;
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
            Label_PlanoUsuario.Text = "Plano: " + ((EPlanos)Global._UsuarioSelecionado.Plano).ToString();
            Label_VeiculoSelecionado.Text = Global.carroSelecionado.Placa + " - " + Global.carroSelecionado.NomeVeiculo;
            Label_VeiculoSelecionadoMarca.Text = Global.carroSelecionado.Marca;
            if(Global.carroSelecionado.TipoCombustivel != null)
            {
                Label_TipoCombustivel.Text = "Combust�vel: " + ((ECombustiveis)Global.carroSelecionado.TipoCombustivel).ToString();
            }
            else
            {
                Label_TipoCombustivel.Text = "Combust�vel: N�o selecionado";
            }
            Image_TipoVeiculo.Source = Global.carroSelecionado.Imagem;
            Label_QuilometragemAtual.Text = $"�ltima KM registrada: {Global.carroSelecionado.Kilometragem}";
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
                case "Servi�o":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Servi�o));
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
            if (await DisplayAlert("Aten��o", "Realmente deseja realizar a exclus�o?", "Confirmar", "Cancelar"))
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

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var popup = new PopUp_Veiculos(Global._Veiculos);

        // Exibe o popup e aguarda o ve�culo selecionado
        var veiculoSelecionado = Navigation.PushAsync(popup);

        var resultado = await popup.ObterVeiculoSelecionadoAsync();


        // Exibe o ve�culo selecionado
        if (resultado != null)
        {
            Global.carroSelecionado = resultado;
            carregarTela();
        }
    }

    private void Expander_IsExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        var expander = (Expander)sender;

        // Acessar o item associado � c�lula (geralmente, o item de dados vinculado ao item da lista)
        var item = (Servico)expander.BindingContext;

        // Verifica o estado de expans�o do Expander e atualiza a propriedade LineBreakMode
        if (expander.IsExpanded)
        {
            item.LineBreakMode = LineBreakMode.CharacterWrap;
        }
        else
        {
            item.LineBreakMode = LineBreakMode.TailTruncation;
        }
    }

    private void Expander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {
        var expander = (Expander)sender;

        // Verifica o estado de expans�o do Expander e atualiza a propriedade LineBreakMode
        if (expander.IsExpanded)
        {
            Label_VeiculoSelecionado.LineBreakMode = LineBreakMode.CharacterWrap;
            Label_VeiculoSelecionadoMarca.LineBreakMode = LineBreakMode.CharacterWrap;
        }
        else
        {
            Label_VeiculoSelecionado.LineBreakMode = LineBreakMode.TailTruncation;
            Label_VeiculoSelecionadoMarca.LineBreakMode = LineBreakMode.TailTruncation;
        }
    }
}