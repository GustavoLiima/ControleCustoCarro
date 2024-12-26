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
    private void OnAddVeiculoClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CadastroVeiculo(false));
    }

    private async void CollectionView_Veiculos_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        VeiculoModel veiculoSelecionado = (VeiculoModel)((ListView)sender).SelectedItem;

        if (((ListView)sender).SelectedItem != null)
        {
            ((ListView)sender).SelectedItem = null;
            string action = await DisplayActionSheet("O que voc� deseja com este ve�culo?", "Cancelar", null, "Selecionar ve�culo", "Editar ve�culo");
            // Verifica a a��o selecionada
            switch (action)
            {
                case "Selecionar ve�culo":
                    Global.carroSelecionado = veiculoSelecionado;
                    await Toast.Make($"Voc� selecionou {Global.carroSelecionado.NomeVeiculo}").Show();
                    await SecureStorage.Default.SetAsync("veiculoSelecionado", JsonConvert.SerializeObject(Global.carroSelecionado));
                    await Navigation.PopAsync();
                    break;
                case "Editar ve�culo":
                    await Navigation.PushAsync(new CadastroVeiculo(veiculoSelecionado));
                    break;
                default:
                    // O usu�rio cancelou ou fechou o menu
                    break;
            }
        }
    }
}