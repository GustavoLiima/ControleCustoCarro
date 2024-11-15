using TesteRec.Db.Models;

namespace TesteRec.Layouts.listas;

public partial class TiposServico : ContentPage
{
    public TiposServico()
	{
		InitializeComponent();
        CollectionView_TipoCombustivel.ItemsSource = DbTipoServico._tipoServicos;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((CollectionView)sender).SelectedItem = null;
    }
}