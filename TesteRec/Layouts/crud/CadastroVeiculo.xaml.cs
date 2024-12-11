using Newtonsoft.Json;
using TesteRec.API.Communic;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;
using TesteRec.Model;

namespace TesteRec.Layouts.crud;

public partial class CadastroVeiculo : ContentPage
{
    VeiculoModel _veiculo;
    bool _novoCadastro = false;
    private TipoVeiculo _tipoVeiculoSelecionado = null;
    private List<TipoVeiculo> _listaTipoVeiculo = new List<TipoVeiculo>()
        {
            new TipoVeiculo()
            {
                ID = 0,
                Descricao = "Carro",
                Imagem = "carro.png"
            },
            new TipoVeiculo()
            {
                ID = 1,
                Descricao = "Moto",
                Imagem = "moto.png"
            },
            new TipoVeiculo()
            {
                ID = 2,
                Descricao = "Ônibus",
                Imagem = "onibus.png"
            },
            new TipoVeiculo()
            {
                ID = 3,
                Descricao = "Caminhão",
                Imagem = "caminhao.png"
            },
            new TipoVeiculo()
            {
                ID = 4,
                Descricao = "Van",
                Imagem = "van.png"
            },
            new TipoVeiculo()
            {
                ID = 5,
                Descricao = "Bicicleta",
                Imagem = "bicicleta.png"
            },
            new TipoVeiculo()
            {
                ID = 6,
                Descricao = "Caminhonete",
                Imagem = "caminhonete.png"
            },
        };

    public CadastroVeiculo(bool pNovoCadastro)
    {
        InitializeComponent();
        _novoCadastro = pNovoCadastro;
    }

    public CadastroVeiculo(VeiculoModel pVeiculoSelecionado)
    {
        InitializeComponent();
        _veiculo = pVeiculoSelecionado;
        _tipoVeiculoSelecionado = _listaTipoVeiculo.Find(x => x.ID == _veiculo.TipoVeiculo);
        Entry_MarcaVeiculo.Text = _veiculo.Marca;
        Entry_TipoVeiculo.Text = _tipoVeiculoSelecionado.Descricao;
        Entry_NomeVeiculo.Text = _veiculo.NomeVeiculo;
        Entry_Quilometragem.Text = _veiculo.Kilometragem?.ToString();
        Entry_AnoFabricacao.Text = _veiculo.AnoFabricacao?.ToString();
        Entry_AnoModelo.Text = _veiculo.AnoModelo?.ToString();
        Entry_Placa.Text = _veiculo.Placa;
        Entry_Renavam.Text = _veiculo.Renavam?.ToString();
        Entry_Chassi.Text = _veiculo.Chassi;
        Entry_LitragemVeiculo.Text = _veiculo.CapacidadeTanque?.ToString();
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
        CollectionView_TipoVeiculo.ItemsSource = _listaTipoVeiculo;
        _tipoVeiculoSelecionado = _listaTipoVeiculo[0];
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
                //Image_FotoCarro.Source = ImageSource.FromStream(() => stream);
            }
        }
        else if (action == "Escolher da Galeria")
        {
            // Selecionar foto da galeria
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                //Image_FotoCarro.Source = ImageSource.FromStream(() => stream);
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
            _tipoVeiculoSelecionado = selectedItem;
            Entry_TipoVeiculo.Text = selectedItem.Descricao;
            Popup_TipoVeiculo.IsVisible = false;
            Entry_NomeVeiculo.Focus();
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

    private void InverterBotoes()
    {
        Frame_LoadingLogin.IsVisible = !Frame_LoadingLogin.IsVisible;
        Button_CadastrarCarro.IsVisible = !Button_CadastrarCarro.IsVisible;
    }

    private async void CadastrarVeiculo_Clicked(object sender, EventArgs e)
    {
        if (_tipoVeiculoSelecionado == null)
        {
            await DisplayAlert("Atenção", "É necessário selecionar um tipo de veículo", "continuar");
            Entry_TipoVeiculo_Focused(null, null);
            return;
        }
        InverterBotoes();
        VeiculoDB instancia = new VeiculoDB();
        VeiculoController communic = new VeiculoController();
        VeiculoModel objAdd = new VeiculoModel()
        {
            AnoFabricacao = int.Parse(Entry_AnoFabricacao.Text),
            AnoModelo = int.Parse(Entry_AnoModelo.Text),
            Kilometragem = int.Parse(Entry_Quilometragem.Text),
            CapacidadeTanque = int.Parse(Entry_LitragemVeiculo.Text),
            NomeVeiculo = Entry_NomeVeiculo.Text,
            Marca = Entry_MarcaVeiculo.Text,
            Chassi = Entry_Chassi.Text,
            Renavam = Entry_Renavam.Text,
            TipoVeiculo = _tipoVeiculoSelecionado.ID,
            Placa = Entry_Placa.Text,
        };
        if (_veiculo != null)
        {
            objAdd.ID = _veiculo.ID;
            var retorno = await communic.AtualizarVeiculo(objAdd);
            if (retorno.Sucesso)
            {
                await instancia.UpdateVeiculoAsync(objAdd);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Atenção", retorno.Mensagem, "continuar");
            }
        }
        else
        {
            var retorno = await communic.CriarVeiculo(objAdd);
            if (retorno.Sucesso)
            {
                await instancia.AddVeiculoAsync(retorno.Valor);
                if (_novoCadastro)
                {
                    Global.carroSelecionado = retorno.Valor;
                    await SecureStorage.Default.SetAsync("veiculoSelecionado", JsonConvert.SerializeObject(Global.carroSelecionado));
                    Application? current = Application.Current;
                    if (current != null)
                    {
                        current.MainPage = new AppShell();
                    }
                    return;
                }
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Atenção", retorno.Mensagem, "continuar");
            }
        }
        InverterBotoes();
    }

    private void Entry_MarcaVeiculo_Completed(object sender, EventArgs e)
    {
        Entry_MarcaVeiculo.Unfocus();
        Popup_TipoVeiculo.IsVisible = true;
    }

    private void Entry_NomeVeiculo_Completed(object sender, EventArgs e)
    {
        Entry_Quilometragem.Focus();
    }

    private void Entry_Quilometragem_Completed(object sender, EventArgs e)
    {
        Entry_AnoFabricacao.Focus();
    }

    private void Entry_AnoFabricacao_Completed(object sender, EventArgs e)
    {
        Entry_AnoModelo.Focus();
    }

    private void Entry_AnoModelo_Completed(object sender, EventArgs e)
    {
        Entry_Placa.Focus();
    }

    private void Entry_Placa_Completed(object sender, EventArgs e)
    {
        Entry_LitragemVeiculo.Focus();
    }

    private void Entry_LitragemVeiculo_Completed(object sender, EventArgs e)
    {
        Entry_Renavam.Focus();
    }

    private void Entry_Renavam_Completed(object sender, EventArgs e)
    {
        Entry_Chassi.Focus();
    }

    private void Entry_Chassi_Completed(object sender, EventArgs e)
    {
        Entry_Chassi.Unfocus();
        ScrollView.ScrollToAsync(0, ScrollView.ContentSize.Height, true);
    }
}

public class TipoVeiculo
{
    public int ID { get; set; }
    public string? Descricao { get; set; }
    public string? Imagem { get; set; }
}