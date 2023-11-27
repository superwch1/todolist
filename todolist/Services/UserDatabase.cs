using SQLite;

namespace todolist.Services
{
	public static class UserDatabase
	{

        public const string DatabaseFilename = "Database.db3";

        public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        static SQLiteAsyncConnection _connection = new SQLiteAsyncConnection(DatabasePath, Flags);


        public static async Task CreateTable()
        {
            await _connection.CreateTableAsync<UserModel>();
        }


        public static async Task<UserModel?> ReadItemAsync()
        {
            return await _connection.Table<UserModel>().Where(i => i.Id == 1).FirstOrDefaultAsync();
        }


        public static async Task CreateItemAsync(UserModel item)
        {
            await _connection.InsertAsync(item);
        }

        public static async Task UpdateItemAsync(UserModel item)
        {
            await _connection.UpdateAsync(item);
        }
    }
}

