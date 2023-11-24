using Microsoft.AspNetCore.SignalR.Client;

namespace todolist;

public partial class App : Application
{
	public App()
	{
        InitializeComponent();

        List<TaskModel>? tasks = null;
        string view = "";
        string jwtToken = "";
        HubConnection? connection = null;


        Task.Run(async () =>
        {
            await AccountDatabase.CreateTable();
            var item = await AccountDatabase.ReadItemAsync();
            if (item == null)
            {
                await AccountDatabase.CreateItemAsync(new AccountModel() { Id = 1, JwtToken = "" });
                view = "Login";
            }
            else
            {
                var account = await AccountDatabase.ReadItemAsync();
                tasks = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, account.JwtToken);
            
                if (tasks != null)
                {
                    view = "AppShell";
                    jwtToken = account.JwtToken;
                    connection = await SignalR.BuildHubConnection(jwtToken);
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
                MainPage = new LoginView();
                break;
            
            case "AppShell":
                MainPage = new AppShell(tasks, jwtToken, connection);
                break;
        }
    }


    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Activated += async (s, e) =>
        {
            foreach (var action in LifeCycleMethods.ActivatedActions)
            {
                await action();
            };
        };

        window.Deactivated += async (s, e) =>
        {
            foreach (var action in LifeCycleMethods.DeactivatedActions)
            {
                await action();
            };
        };

        return window;
    }
}

