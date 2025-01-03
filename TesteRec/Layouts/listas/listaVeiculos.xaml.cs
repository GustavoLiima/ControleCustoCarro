using Cofauto.Layouts.listas;
using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json;
using TesteRec.API.Communic;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;
using TesteRec.Layouts.crud;

namespace TesteRec.Layouts.listas;

public partial class listaVeiculos : ContentPage
{
    public List<VeiculoModel> Veiculos = null;
    public listaVeiculos()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarDados(false);
    }

    private async void CarregarDados(bool pSelecionarOutroVeiculo)
    {
        CollectionView_Veiculos.ItemsSource = null;
        Veiculos = await new VeiculoDB().GetVeiculosAsync();
        CollectionView_Veiculos.ItemsSource = Veiculos;
        if(pSelecionarOutroVeiculo && Veiculos.Count >= 1)
        {
            Global.carroSelecionado = Veiculos[0];
            await Toast.Make($"Você selecionou {Global.carroSelecionado.NomeVeiculo}").Show();
            await SecureStorage.Default.SetAsync("veiculoSelecionado", JsonConvert.SerializeObject(Global.carroSelecionado));
        }
        else if(pSelecionarOutroVeiculo && Veiculos.Count == 0)
        {
            await DisplayAlert("Atenção", "Você não está com nenhum cadastrado, faça o cadastro para continuar o uso do aplicativo", "Continuar");
            await Navigation.PushAsync(new CadastroVeiculo(false));
        }
    }
    private async void OnAddVeiculoClicked(object sender, EventArgs e)
    {
        if(Global._UsuarioSelecionado.Plano == 0 && Veiculos.Count >= 3)
        {
            if(await DisplayAlert("Você já atingiu o limite de carros", "Para adicionar mais carros, contrate outro plano! \n A partir de R$5,90!", "Melhorar plano", "Cancelar"))
            {
                await Navigation.PushAsync(new Planos());
            }
            return;
        }

        await Navigation.PushAsync(new CadastroVeiculo(false));
    }

    private async void CollectionView_Veiculos_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        VeiculoModel veiculoSelecionado = (VeiculoModel)((ListView)sender).SelectedItem;

        if (((ListView)sender).SelectedItem != null)
        {
            ((ListView)sender).SelectedItem = null;
            string action = await DisplayActionSheet("O que você deseja com este veículo?", "Cancelar", null, "Selecionar veículo", "Editar veículo", "Excluir veículo");
            // Verifica a ação selecionada
            switch (action)
            {
                case "Selecionar veículo":
                    Global.carroSelecionado = veiculoSelecionado;
                    await Toast.Make($"Você selecionou {Global.carroSelecionado.NomeVeiculo}").Show();
                    await SecureStorage.Default.SetAsync("veiculoSelecionado", JsonConvert.SerializeObject(Global.carroSelecionado));
                    await Navigation.PopAsync();
                    break;
                case "Editar veículo":
                    await Navigation.PushAsync(new CadastroVeiculo(veiculoSelecionado));
                    break;
                case "Excluir veículo":
                    if(await DisplayAlert("Atenção", "Você realmente deseja excluir este veículo?", "Confirmar", "Cancelar"))
                    {
                        PopupServiceSelection.IsVisible = true;
                        VeiculoController req = new VeiculoController();
                        var retorno = await req.DesabilitarVeiculo(veiculoSelecionado);
                        PopupServiceSelection.IsVisible = false;
                        if (retorno.Sucesso)
                        {
                            VeiculoDB db = new VeiculoDB();
                            await db.DeleteVeiculoAsync(veiculoSelecionado);
                            CarregarDados(true);
                        }
                        else
                        {
                            await DisplayAlert("Atenção", "Verifique sua conexão, para remover o veículo é necessário conexão com a internet.", "Continuar");
                        }
                    }
                    break;
                default:
                    // O usuário cancelou ou fechou o menu
                    break;
            }
        }
    }
}