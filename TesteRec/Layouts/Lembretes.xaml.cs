using TesteRec.Db.Models;
using TesteRec.Db.Services;

namespace TesteRec.Layouts;

public partial class Lembretes : ContentPage
{
    ServicoDB servicoService = new ServicoDB();
    public Lembretes()
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
        var lembretes = await servicoService.GetLembretesAsync();
        CollectionView_Lembretes.ItemsSource = null;
        CollectionView_Lembretes.ItemsSource = lembretes;
    }

    private async void ButtonRemover_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var servico = button?.BindingContext as Servico;

        if (servico != null)
        {
            if (await DisplayAlert("Atenção", "Deseja realmente remover este lembrete?", "Confirmar", "Cancelar"))
            {
                await servicoService.DeleteServicoAsync(servico);
                CarregarDados();
            }
        }
    }

    private async void ButtonRenovar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var servico = button?.BindingContext as Servico;

        if (servico != null)
        {
            await Navigation.PushAsync(new InclusaoServico(servico));
        }
    }
}