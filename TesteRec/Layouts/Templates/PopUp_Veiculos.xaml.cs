using System.Collections.ObjectModel;
using TesteRec.API.Models;

namespace Cofauto.Layouts.Templates;

public partial class PopUp_Veiculos : ContentPage
{
    private TaskCompletionSource<VeiculoModel> _taskCompletionSource;
    private ObservableCollection<VeiculoModel> VeiculosOriginal { get; set; }
    private ObservableCollection<VeiculoModel> VeiculosFiltrados { get; set; }

    public PopUp_Veiculos(List<VeiculoModel> veiculos)
    {
        InitializeComponent();

        VeiculosOriginal = new ObservableCollection<VeiculoModel>(veiculos);
        VeiculosFiltrados = new ObservableCollection<VeiculoModel>(veiculos);

        VeiculosListView.ItemsSource = VeiculosFiltrados;
        _taskCompletionSource = new TaskCompletionSource<VeiculoModel>();

        // Configura o evento de seleção
        VeiculosListView.ItemTapped += VeiculosListView_ItemTapped;
    }

    // Função para retornar o veículo selecionado
    public Task<VeiculoModel> ObterVeiculoSelecionadoAsync()
    {
        return _taskCompletionSource.Task;
    }

    private void VeiculosListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is VeiculoModel veiculoSelecionado)
        {
            // Completa a Task com o veículo selecionado
            ((ListView)sender).SelectedItem = null;
            _taskCompletionSource.TrySetResult(veiculoSelecionado);
            // Fecha o popup
            Navigation.PopAsync();
        }
    }

    private void FiltroEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = e.NewTextValue?.ToLower() ?? string.Empty;

        VeiculosFiltrados.Clear();
        foreach (var veiculo in VeiculosOriginal)
        {
            if (veiculo.NomeVeiculo.ToLower().Contains(filtro))
            {
                VeiculosFiltrados.Add(veiculo);
            }
        }
    }
}