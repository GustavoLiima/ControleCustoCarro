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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        carregarTela();
    }

    private async void carregarTela()
    {
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
                case "Receita":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Receita));
                    break;
                case "Percurso":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Percurso));
                    break;
                case "Serviço":
                    Navigation.PushAsync(new InclusaoServico(Enum.EMenuSelecionado.Serviço));
                    break;
                case "Despesa":
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