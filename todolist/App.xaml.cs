using SQLite;

namespace todolist;

public partial class App : Application
{
	public App(AccountDatabase accountDatabase)
	{
        InitializeComponent();

        Task.Run(async () =>
        {
            await accountDatabase.CreateTable();
            var item = await accountDatabase.ReadItemAsync();
            if (item == null)
            {
                await accountDatabase.CreateItemAsync(new AccountInfo() { Id = 1, JwtToken = "" });
            }
            else
            {
                var accountInfo = await accountDatabase.ReadItemAsync();
                Console.WriteLine(accountInfo.JwtToken);
            }
        });

        MainPage = new LoginView(accountDatabase);
    }
}

