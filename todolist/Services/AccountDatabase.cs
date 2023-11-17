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
            await _connection.CreateTableAsync<AccountInfo>();
        }


        public async Task<AccountInfo?> ReadItemAsync()
        {
            return await _connection.Table<AccountInfo>().Where(i => i.Id == 1).FirstOrDefaultAsync();
        }


        public async Task CreateItemAsync(AccountInfo item)
        {
            await _connection.InsertAsync(item);
        }

        public async Task UpdateItemAsync(AccountInfo item)
        {
            await _connection.UpdateAsync(item);
        }
    }
}

