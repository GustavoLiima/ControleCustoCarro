using SQLite;
using TesteRec.API.Models;
using TesteRec.Db.Models;

namespace TesteRec.Db.Services
{
    public class BaseDB
    {
        private static SQLiteAsyncConnection? _databaseConnection;

        // Cria ou retorna uma conexão SQLite com o banco de dados
        public static SQLiteAsyncConnection GetDatabaseConnection()
        {
            if (_databaseConnection == null)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "db.db3");
                _databaseConnection = new SQLiteAsyncConnection(dbPath);
            }
            return _databaseConnection;
        }

        // Inicializa o banco de dados (cria tabelas se não existirem)
        public async Task InitializeDatabaseAsync()
        {
            var database = GetDatabaseConnection();
            await database.CreateTableAsync<Servico>();
            await database.CreateTableAsync<TipoServico>();
            await database.CreateTableAsync<VeiculoModel>();
            await database.CreateTableAsync<Despesa>();
        }
    }
}
