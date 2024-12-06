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
}