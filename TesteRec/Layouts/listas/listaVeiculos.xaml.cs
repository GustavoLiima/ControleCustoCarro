using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;
using TesteRec.Layouts.crud;

namespace TesteRec.Layouts.listas;

public partial class listaVeiculos : ContentPage
{
    public listaVeiculos()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarDados();
    }

    private async void CarregarDados()
    {
        CollectionView_Veiculos.ItemsSource = null;
        CollectionView_Veiculos.ItemsSource = await new VeiculoDB().GetVeiculosAsync();
    }

    private async void CollectionView_Veiculos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        VeiculoModel veiculoSelecionado = (VeiculoModel)((CollectionView)sender).SelectedItem;

        if (((CollectionView)sender).SelectedItem != null)
        {
            ((CollectionView)sender).SelectedItem = null;
            string action = await DisplayActionSheet("O que você deseja com este veículo?", "Cancelar", null, "Selecionar veículo", "Editar veículo");
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
                default:
                    // O usuário cancelou ou fechou o menu
                    break;
            }
        }
    }

    private void OnAddVeiculoClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CadastroVeiculo(false));
    }
}