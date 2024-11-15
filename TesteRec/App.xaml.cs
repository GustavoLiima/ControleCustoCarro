using TesteRec.Db.Services;

namespace TesteRec
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            var databaseService = new BaseDB();
            await databaseService.InitializeDatabaseAsync();
        }
    }
}