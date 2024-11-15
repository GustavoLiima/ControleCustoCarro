using Newtonsoft.Json;
using TesteRec.Model;

namespace TesteRec
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        Marca _marcaSelecionada;

        public MainPage()
        {
            InitializeComponent();
            CarregarInformacoes();
            BindingContext = new MainViewModel();
        }

        public void CarregarInformacoes()
        {
            List<Marca>? listaMarcas = JsonConvert.DeserializeObject<List<Marca>>(BancoMarca.json);
            if (listaMarcas != null)
            {
                BancoMarca._ListaMarcas = listaMarcas;
            }
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
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
    }
}
