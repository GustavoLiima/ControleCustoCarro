using TesteRec.Db.Services;
using TesteRec.Layouts.Iniciais;

namespace TesteRec
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new Apresentacao();
        }

        protected override async void OnStart()
        {
            var databaseService = new BaseDB();
            await databaseService.InitializeDatabaseAsync();
        }
    }
}