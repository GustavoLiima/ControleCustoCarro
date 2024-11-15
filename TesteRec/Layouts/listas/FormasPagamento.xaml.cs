using TesteRec.Model;

namespace TesteRec.Layouts.listas;

public partial class FormasPagamento : ContentPage
{
    public List<Pagamentos> _tiposPagamentos = new List<Pagamentos>()
    {
        new Pagamentos()
        {
            Id = 1,
            Descricao = "Dinheiro",
            Imagem = "dinheiro.png"
        },
        new Pagamentos()
        {
            Id = 2,
            Descricao = "D�bito",
            Imagem = "cartao.png"
        },
        new Pagamentos()
        {
            Id = 3,
            Descricao = "Cr�dito",
            Imagem = "cartao.png"
        },
        new Pagamentos()
        {
            Id = 4,
            Descricao = "Pix",
            Imagem = "pix.png"
        },
        new Pagamentos()
        {
            Id = 5,
            Descricao = "Vale Combust�vel",
            Imagem = "ticket.png"
        },
    };
	public FormasPagamento()
	{
		InitializeComponent();
        CollectionView_FormasPagamento.ItemsSource = _tiposPagamentos;

    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((CollectionView)sender).SelectedItem = null;
    }
}