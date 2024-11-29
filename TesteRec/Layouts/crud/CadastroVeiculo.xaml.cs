using Newtonsoft.Json;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;
using TesteRec.Model;

namespace TesteRec.Layouts.crud;

public partial class CadastroVeiculo : ContentPage
{
    Veiculo _veiculo;
    bool _novoCadastro = false;

    public CadastroVeiculo(bool pNovoCadastro)
    {
        InitializeComponent();
        _novoCadastro = pNovoCadastro;
    }

    public CadastroVeiculo(Veiculo pVeiculoSelecionado)
    {
        InitializeComponent();
        _veiculo = pVeiculoSelecionado;
        Entry_ApelidoVeiculo.Text = _veiculo.Nome;
        Entry_MarcaVeiculo.Text = _veiculo.Marca;
        Entry_TipoVeiculo.Text = _veiculo.TipoVeiculo;
        Entry_NomeVeiculo.Text = _veiculo.Modelo;
        Entry_Quilometragem.Text = _veiculo.Quilometros.ToString();
        Entry_AnoFabricacao.Text = _veiculo.Ano.ToString();
        Entry_AnoModelo.Text = _veiculo.Ano.ToString();
        Entry_Placa.Text = _veiculo.Placa;
        Entry_Renavam.Text = _veiculo.Renavam.ToString();
        Entry_Chassi.Text = _veiculo.Chassi;
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
        TipoVeiculo? selectedItem = e.CurrentSelection.FirstOrDefault() as TipoVeiculo;

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

    private async void CadastrarVeiculo_Clicked(object sender, EventArgs e)
    {
        VeiculoDB instancia = new VeiculoDB();
        Veiculo objAdd = new Veiculo()
        {
            Ano = int.Parse(Entry_AnoFabricacao.Text),
            Quilometros = double.Parse(Entry_Quilometragem.Text),
            Nome = Entry_ApelidoVeiculo.Text,
            Marca = Entry_MarcaVeiculo.Text,
            Chassi = Entry_Chassi.Text,
            Renavam = Entry_Renavam.Text,
            Modelo = Entry_NomeVeiculo.Text,
            TipoVeiculo = Entry_TipoVeiculo.Text,
            Placa = Entry_Placa.Text,
        };
        if(_veiculo != null)
        {
            objAdd.Id = _veiculo.Id;
            await instancia.UpdateVeiculoAsync(objAdd);
        }
        else
        {
            await instancia.AddVeiculoAsync(objAdd);
        }

        if(_novoCadastro)
        {
            Global.carroSelecionado = objAdd;
            Application? current = Application.Current;
            if (current != null)
            {
                current.MainPage = new AppShell();
            }
        }
        else
        {
            await Navigation.PopAsync();
        }
    }
}

public class TipoVeiculo
{
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
}