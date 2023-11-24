using Microsoft.AspNetCore.SignalR.Client;

namespace todolist;

public partial class AppShell : Shell
{
	public AppShell(List<TaskModel> tasks, string jwtToken, HubConnection connection)
	{
		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, jwtToken, connection);
		followUpTask.Content = new TaskView(tasks, 1, jwtToken, connection);
    }
}

