using TesteRec.Db.Models;

namespace TesteRec.Layouts.listas;

public partial class TipoReceita : ContentPage
{
	public TipoReceita()
	{
		InitializeComponent();
        CollectionView_TipoReceita.ItemsSource = DbTipoReceita._tipoReceitas;
	}

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((CollectionView)sender).SelectedItem = null;
    }
}