using Microsoft.AspNetCore.SignalR.Client;

namespace todolist.Views.TaskViews;

public partial class HomeView : Shell
{
	public HomeView(List<TaskModel> tasks, string jwtToken, HubConnection connection)
	{
		Routing.RegisterRoute("login", typeof(ForgetPasswordView));

		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, jwtToken, connection);
		followUpTask.Content = new TaskView(tasks, 1, jwtToken, connection);
    }
}

