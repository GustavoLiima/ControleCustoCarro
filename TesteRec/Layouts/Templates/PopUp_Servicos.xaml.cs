using System.Collections.ObjectModel;
using TesteRec.Db.Models;

namespace Cofauto.Layouts.Templates;

public partial class PopUp_Servicos : ContentPage
{
    private TaskCompletionSource<Servico> _taskCompletionSource;
    private ObservableCollection<Servico> ServicosOriginal { get; set; }
    private ObservableCollection<Servico> ServicosFiltrados { get; set; }
    public PopUp_Servicos()
	{
		InitializeComponent();

        //ServicosOriginal = new ObservableCollection<Servico>(veiculos);
        //ServicosFiltrados = new ObservableCollection<Servico>(veiculos);

        ServicosListView.ItemsSource = ServicosFiltrados;
        _taskCompletionSource = new TaskCompletionSource<Servico>();

        // Configura o evento de seleção
        ServicosListView.ItemTapped += ServicoListView_ItemTapped;
    }

    public Task<Servico> ObterServicoSelecionadoAsync()
    {
        return _taskCompletionSource.Task;
    }

    private void ServicoListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Servico servicoSelecionado)
        {
            // Completa a Task com o veículo selecionado
            _taskCompletionSource.TrySetResult(servicoSelecionado);
            ((ListView)sender).SelectedItem = null;
            // Fecha o popup
            Navigation.PopAsync();
        }
    }

    private void FiltroEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = e.NewTextValue?.ToLower() ?? string.Empty;

        ServicosFiltrados.Clear();
        foreach (var veiculo in ServicosOriginal)
        {
            if (veiculo.DescricaoReceita.ToLower().Contains(filtro))
            {
                ServicosFiltrados.Add(veiculo);
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}