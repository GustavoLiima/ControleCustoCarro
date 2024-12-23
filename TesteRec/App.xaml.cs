using Newtonsoft.Json;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Services;
using TesteRec.Layouts.Iniciais;

namespace TesteRec
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Define uma página inicial temporária
            Application.Current.UserAppTheme = AppTheme.Light;
            MainPage = new ContentPage
            {
                Content = new ActivityIndicator
                {
                    IsRunning = true,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };

            CarregarPaginaInicial();
        }

        private async void CarregarPaginaInicial()
        {
            string retorno = await SecureStorage.Default.GetAsync("usuario");

            if (retorno == null)
            {
                MainPage = new Apresentacao();
            }
            else
            {
                Global._UsuarioSelecionado = JsonConvert.DeserializeObject<UsuarioVM>(retorno);
                MainPage = new AppShell();
            }
        }

        protected override async void OnStart()
        {
            var databaseService = new BaseDB();
            await databaseService.InitializeDatabaseAsync();
        }
    }
}