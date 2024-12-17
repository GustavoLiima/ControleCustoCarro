using System.Collections.ObjectModel;
using TesteRec.Db.Models;

namespace Cofauto.Layouts.Templates;

public partial class PopUp_Servicos : ContentPage
{
    private TaskCompletionSource<List<TipoServico>> _taskCompletionSource;
    private ObservableCollection<TipoServico> ServicosOriginal { get; set; }
    private ObservableCollection<TipoServico> ServicosFiltrados { get; set; }
    public PopUp_Servicos(List<TipoServico> pServicos)
	{
		InitializeComponent();

        ServicosOriginal = new ObservableCollection<TipoServico>(pServicos);
        ServicosFiltrados = new ObservableCollection<TipoServico>(pServicos);
        ServicosListView.ItemsSource = ServicosFiltrados;
        _taskCompletionSource = new TaskCompletionSource<List<TipoServico>>();

        // Configura o evento de seleção
        ServicosListView.ItemTapped += ServicoListView_ItemTapped;
    }

    public Task<List<TipoServico>> ObterServicoSelecionadoAsync()
    {
        return _taskCompletionSource.Task;
    }

    private void ServicoListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null;
    }

    private void FiltroEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = e.NewTextValue?.ToLower() ?? string.Empty;

        ServicosFiltrados.Clear();
        foreach (var veiculo in ServicosOriginal)
        {
            if (veiculo.Descricao.ToLower().Contains(filtro))
            {
                ServicosFiltrados.Add(veiculo);
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Recupera os itens selecionados (onde IsSelected é true)
        var servicosSelecionados = ServicosOriginal
        .Where(s => s.IsSelected)
        .ToList();

        _taskCompletionSource.TrySetResult(servicosSelecionados);
        // Fecha o popup
        Navigation.PopAsync();
    }
}