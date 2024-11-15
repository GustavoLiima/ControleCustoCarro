using TesteRec.Db.Models;

namespace TesteRec.Layouts.listas;

public partial class TiposDespesas : ContentPage
{
	public TiposDespesas()
	{
		InitializeComponent();
        CollectionView_TipoDespesa.ItemsSource = DbDespesa._listaDespesa;

    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((CollectionView)sender).SelectedItem = null;
    }
}