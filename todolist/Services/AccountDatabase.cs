using SQLite;

namespace todolist.Services
{
	public static class AccountDatabase
	{

        public const string DatabaseFilename = "Database.db3";

        public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        static SQLiteAsyncConnection _connection = new SQLiteAsyncConnection(DatabasePath, Flags);


        public static async Task CreateTable()
        {
            await _connection.CreateTableAsync<AccountModel>();
        }


        public static async Task<AccountModel?> ReadItemAsync()
        {
            return await _connection.Table<AccountModel>().Where(i => i.Id == 1).FirstOrDefaultAsync();
        }


        public static async Task CreateItemAsync(AccountModel item)
        {
            await _connection.InsertAsync(item);
        }

        public static async Task UpdateItemAsync(AccountModel item)
        {
            await _connection.UpdateAsync(item);
        }
    }
}

