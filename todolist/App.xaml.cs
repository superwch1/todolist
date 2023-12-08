using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using todolist.Views.AccountViews;
using todolist.Views.TaskViews;

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

            connection = await SignalR.BuildHubConnection(account.JwtToken);
            if (connection == null)
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
                MainPage = new TaskShell(tasks, jwtToken, connection);
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

