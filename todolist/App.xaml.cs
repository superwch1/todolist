using System.Net;
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
            await UserDatabase.CreateTable();
            var account = await UserDatabase.ReadItemAsync();
            if (account == null)
            {
                await UserDatabase.CreateItemAsync(new UserModel() { Id = 1, JwtToken = "" });
                view = "AccountShell";
                return;
            }

            if (account.JwtToken == "")
            {
                view = "AccountShell";
                return;
            }

            var taskReponse = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, account.JwtToken);
            if (taskReponse.Item2 != HttpStatusCode.OK)
            {
                view = "AccountShell";
                return;
            }

            SignalR.Connection = await SignalR.BuildHubConnection(account.JwtToken);
            if (SignalR.Connection == null)
            {
                view = "AccountShell";
                return;
            }

            view = "TaskShell";
            tasks = taskReponse.Item1;
            jwtToken = account.JwtToken;

        }).Wait();

        switch (view)
        {
            case "AccountShell":
                MainPage = new AccountShell();
                break;
            
            case "TaskShell":
                MainPage = new TaskShell(tasks, jwtToken, true);
                break;
        }
    }


    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        //it should be resumed since activated will call also when app is opened
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

