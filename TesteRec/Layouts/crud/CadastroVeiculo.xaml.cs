using Newtonsoft.Json;
using TesteRec.Model;

namespace TesteRec.Layouts.crud;

public partial class CadastroVeiculo : ContentPage
{
    Marca _marcaSelecionada;

    public CadastroVeiculo()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarInformacoes();
    }

    public void CarregarInformacoes()
    {
        List<Marca>? listaMarcas = JsonConvert.DeserializeObject<List<Marca>>(BancoMarca.json);
        if (listaMarcas != null)
        {
            BancoMarca._ListaMarcas = listaMarcas;
        }
        CollectionView_TipoVeiculo.ItemsSource = new List<TipoVeiculo>()
        {
            new TipoVeiculo()
            {
                Descricao = "Carro",
                Imagem = "carro.png"
            },
            new TipoVeiculo()
            {
                Descricao = "Moto",
                Imagem = "moto.png"
            },
            new TipoVeiculo()
            {
                Descricao = "Ônibus",
                Imagem = "onibus.png"
            },
            new TipoVeiculo()
            {
                Descricao = "Caminhão",
                Imagem = "caminhao.png"
            },
        };
    }

    private async void AtualizaFoto_Tapped(object sender, TappedEventArgs e)
    {
        string action = await DisplayActionSheet("Atualizar Foto", "Cancelar", null, "Tirar Foto", "Escolher da Galeria");

        if (action == "Tirar Foto")
        {
            // Tirar foto usando a câmera
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                Image_FotoCarro.Source = ImageSource.FromStream(() => stream);
            }
        }
        else if (action == "Escolher da Galeria")
        {
            // Selecionar foto da galeria
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                Image_FotoCarro.Source = ImageSource.FromStream(() => stream);
            }
        }
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Verifica se há algum item selecionado
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Obtém o item selecionado como Marca
            var marcaSelecionada = e.CurrentSelection.FirstOrDefault() as Marca;
            if (marcaSelecionada != null)
            {
                // Atualiza o texto no ViewModel
                var viewModel = BindingContext as MainViewModel;
                if (viewModel != null)
                {
                    _marcaSelecionada = marcaSelecionada;
                    // Define o texto do Entry para o nome da marca selecionada
                    viewModel.TextoFiltro = marcaSelecionada.nome;

                    // Oculta a CollectionView após a seleção
                    viewModel.DemonstraFiltro = false;

                    // Limpa a seleção atual da CollectionView para evitar que o item permaneça selecionado
                    CollectionView_Marcas.SelectedItem = null;
                }
            }
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Recupera o item selecionado
        TipoVeiculo selectedItem = e.CurrentSelection.FirstOrDefault() as TipoVeiculo;

        if (selectedItem != null)
        {
            Entry_TipoVeiculo.Text = selectedItem.Descricao;
            Popup_TipoVeiculo.IsVisible = false;
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void Entry_TipoVeiculo_Focused(object sender, FocusEventArgs e)
    {
        // Ocultar o teclado
        Entry_TipoVeiculo.Unfocus();


        Popup_TipoVeiculo.IsVisible = true;
    }

    private void OnCancelPopupClicked(object sender, EventArgs e)
    {
        Popup_TipoVeiculo.IsVisible = false;
    }
}

public class TipoVeiculo
{
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
}