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
            await Toast.Make($"Voc� selecionou {Global.carroSelecionado.NomeVeiculo}").Show();
            await SecureStorage.Default.SetAsync("veiculoSelecionado", JsonConvert.SerializeObject(Global.carroSelecionado));
        }
        else if(pSelecionarOutroVeiculo && Veiculos.Count == 0)
        {
            await DisplayAlert("Aten��o", "Voc� n�o est� com nenhum cadastrado, fa�a o cadastro para continuar o uso do aplicativo", "Continuar");
            await Navigation.PushAsync(new CadastroVeiculo(false));
        }
    }
    private async void OnAddVeiculoClicked(object sender, EventArgs e)
    {
        if(Global._UsuarioSelecionado.Plano == 0 && Veiculos.Count >= 3)
        {
            if(await DisplayAlert("Voc� j� atingiu o limite de carros", "Para adicionar mais carros, contrate outro plano! \n A partir de R$5,90!", "Melhorar plano", "Cancelar"))
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
            string action = await DisplayActionSheet("O que voc� deseja com este ve�culo?", "Cancelar", null, "Selecionar ve�culo", "Editar ve�culo", "Excluir ve�culo");
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
                case "Excluir ve�culo":
                    if(await DisplayAlert("Aten��o", "Voc� realmente deseja excluir este ve�culo?", "Confirmar", "Cancelar"))
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
                            await DisplayAlert("Aten��o", "Verifique sua conex�o, para remover o ve�culo � necess�rio conex�o com a internet.", "Continuar");
                        }
                    }
                    break;
                default:
                    // O usu�rio cancelou ou fechou o menu
                    break;
            }
        }
    }
}