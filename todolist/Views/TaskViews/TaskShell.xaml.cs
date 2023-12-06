using Microsoft.AspNetCore.SignalR.Client;

namespace todolist.Views.TaskViews;

public partial class TaskShell : Shell
{
	public TaskShell(List<TaskModel> tasks, string jwtToken, HubConnection connection)
	{
		Routing.RegisterRoute("searchview", typeof(SearchView));
		Routing.RegisterRoute("resetpassword", typeof(ResetPasswordView));

		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, jwtToken, connection);
		followUpTask.Content = new TaskView(tasks, 1, jwtToken, connection);
    }
}

