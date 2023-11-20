using SQLite;

namespace todolist;

public partial class App : Application
{
	public App(AccountDatabase accountDatabase)
	{
        InitializeComponent();

        List<TaskModel>? tasks = null;
        string view = "";


        Task.Run(async () =>
        {
            await accountDatabase.CreateTable();
            var item = await accountDatabase.ReadItemAsync();
            if (item == null)
            {
                await accountDatabase.CreateItemAsync(new AccountModel() { Id = 1, JwtToken = "" });
                view = "Login";
            }
            else
            {
                var account = await accountDatabase.ReadItemAsync();
                tasks = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, account.JwtToken);
            
                if (tasks != null)
                {
                    view = "AppShell";
                }
                else 
                {
                    view = "Login";
                }
                
            }
        }).Wait();

        switch (view)
        {
            case "Login":
                MainPage = new LoginView(accountDatabase);
                break;
            
            case "AppShell":
                MainPage = new AppShell(tasks, accountDatabase);
                break;
        }
    }


    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Activated += async (s, e) =>
        {
            await ToastBar.DisplayToast("Hello");
        };

        window.Deactivated += async (s, e) =>
        {
            await ToastBar.DisplayToast("Bye");
        };

        return window;
    }
}

