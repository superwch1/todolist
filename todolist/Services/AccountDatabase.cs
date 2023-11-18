using System;
using SQLite;

namespace todolist.Services
{
	public class AccountDatabase
	{

        public const string DatabaseFilename = "Database.db3";

        public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        SQLiteAsyncConnection _connection;

        public AccountDatabase()
		{
            _connection = new SQLiteAsyncConnection(DatabasePath, Flags);
        }

        public async Task CreateTable()
        {
            await _connection.CreateTableAsync<AccountModel>();
        }


        public async Task<AccountModel?> ReadItemAsync()
        {
            return await _connection.Table<AccountModel>().Where(i => i.Id == 1).FirstOrDefaultAsync();
        }


        public async Task CreateItemAsync(AccountModel item)
        {
            await _connection.InsertAsync(item);
        }

        public async Task UpdateItemAsync(AccountModel item)
        {
            await _connection.UpdateAsync(item);
        }
    }
}

