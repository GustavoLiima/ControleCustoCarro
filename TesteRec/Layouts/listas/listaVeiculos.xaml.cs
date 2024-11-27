using CommunityToolkit.Maui.Alerts;
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
        Veiculo veiculoSelecionado = (Veiculo)((CollectionView)sender).SelectedItem;

        if (((CollectionView)sender).SelectedItem != null)
        {
            ((CollectionView)sender).SelectedItem = null;
            string action = await DisplayActionSheet("O que você deseja com este veículo?", "Cancelar", null, "Selecionar veículo", "Editar veículo");
            // Verifica a ação selecionada
            switch (action)
            {
                case "Selecionar veículo":
                    Global.carroSelecionado = veiculoSelecionado;
                    await Toast.Make($"Você selecionou {Global.carroSelecionado.Nome}").Show();
                    await Navigation.PopAsync();
                    break;
                case "Editar veículo":
                    await Navigation.PushAsync(new CadastroVeiculo(veiculoSelecionado));
                    await DisplayAlert("Ação", "Você escolheu editar o veículo!", "OK");
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