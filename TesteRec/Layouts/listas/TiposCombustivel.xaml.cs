namespace TesteRec.Layouts.listas;

public partial class TiposCombustivel : ContentPage
{
    public List<TesteRec.Model.TiposCombustivel> _tipoCombustivel = new List<TesteRec.Model.TiposCombustivel>()
    {
        new TesteRec.Model.TiposCombustivel() 
        {
            Id = 1,
            Descricao = "Gas. Comum",
            Imagem = "posto.png"
        },
        new TesteRec.Model.TiposCombustivel()
        {
            Id = 2,
            Descricao = "Gas. Aditivada",
            Imagem = "posto.png"
        },
        new TesteRec.Model.TiposCombustivel()
        {
            Id = 3,
            Descricao = "Gas. Premium",
            Imagem = "posto.png"
        },
        new TesteRec.Model.TiposCombustivel()
        {
            Id = 4,
            Descricao = "Etanol",
            Imagem = "etanol.png"
        },
        new TesteRec.Model.TiposCombustivel()
        {
            Id = 5,
            Descricao = "Diesel",
            Imagem = "diesel.png"
        },
        new TesteRec.Model.TiposCombustivel()
        {
            Id = 6,
            Descricao = "GNV",
            Imagem = "gnv.png"
        },
        new TesteRec.Model.TiposCombustivel()
        {
            Id = 7,
            Descricao = "Elétrico",
            Imagem = "tomada.png"
        },
        new TesteRec.Model.TiposCombustivel()
        {
            Id = 8,
            Descricao = "LPG",
            Imagem = "cilindrogas.png"
        },
    };
	public TiposCombustivel()
	{
		InitializeComponent();
        CollectionView_TipoCombustivel.ItemsSource = _tipoCombustivel;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((CollectionView)sender).SelectedItem = null;
    }
}