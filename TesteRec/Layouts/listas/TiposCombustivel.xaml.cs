namespace TesteRec.Layouts.listas;

public partial class TiposCombustivel : ContentPage
{
    public List<Model.TiposCombustivel> _tipoCombustivel = new List<Model.TiposCombustivel>()
    {
        new Model.TiposCombustivel() 
        {
            Id = 1,
            Descricao = "Gas. Comum",
            Imagem = "posto.png"
        },
        new Model.TiposCombustivel()
        {
            Id = 2,
            Descricao = "Gas. Aditivada",
            Imagem = "posto.png"
        },
        new Model.TiposCombustivel()
        {
            Id = 3,
            Descricao = "Gas. Premium",
            Imagem = "posto.png"
        },
        new Model.TiposCombustivel()
        {
            Id = 4,
            Descricao = "Etanol",
            Imagem = "etanol.png"
        },
        new Model.TiposCombustivel()
        {
            Id = 5,
            Descricao = "Diesel",
            Imagem = "diesel.png"
        },
        new Model.TiposCombustivel()
        {
            Id = 6,
            Descricao = "GNV",
            Imagem = "gnv.png"
        },
        new Model.TiposCombustivel()
        {
            Id = 7,
            Descricao = "Elétrico",
            Imagem = "tomada.png"
        }
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