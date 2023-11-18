using SQLite;

namespace todolist;

public partial class App : Application
{
	public App(AccountDatabase accountDatabase)
	{
        InitializeComponent();

        MainPage = new LoginView(accountDatabase);
    }
}

