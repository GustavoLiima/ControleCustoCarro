using System.Collections.ObjectModel;
using TesteRec.Db.Models;

namespace Cofauto.Layouts.Templates;

public partial class PopUpDespesas : ContentPage
{
    private TaskCompletionSource<List<Despesa>> _taskCompletionSource;
    private ObservableCollection<Despesa> DespesaOriginal { get; set; }
    private ObservableCollection<Despesa> DespesaFiltrados { get; set; }
    public PopUpDespesas(List<Despesa> pDespesa)
    {
        InitializeComponent();

        DespesaOriginal = new ObservableCollection<Despesa>(pDespesa);
        DespesaFiltrados = new ObservableCollection<Despesa>(pDespesa);
        DespesasListView.ItemsSource = DespesaFiltrados;
        _taskCompletionSource = new TaskCompletionSource<List<Despesa>>();
    }

    public Task<List<Despesa>> ObterDespesasSelecionadoAsync()
    {
        return _taskCompletionSource.Task;
    }

    private void FiltroEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = e.NewTextValue?.ToLower() ?? string.Empty;

        DespesaFiltrados.Clear();
        foreach (var desp in DespesaOriginal)
        {
            if (desp.Descricao.ToLower().Contains(filtro))
            {
                DespesaFiltrados.Add(desp);
            }
        }
    }

    private void DespesasListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Recupera os itens selecionados (onde IsSelected é true)
        var despesasSelecionados = DespesaOriginal
        .Where(s => s.IsSelected)
        .ToList();

        _taskCompletionSource.TrySetResult(despesasSelecionados);
        // Fecha o popup
        Navigation.PopAsync();
    }
}