using Microsoft.AspNetCore.SignalR.Client;

namespace todolist;

public partial class AppShell : Shell
{
	public AppShell(List<TaskModel> tasks, string jwtToken, 
		HubConnection connection, AccountDatabase accountDatabase)
	{
		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, jwtToken, connection, accountDatabase);
		followUpTask.Content = new TaskView(tasks, 1, jwtToken, connection, accountDatabase);
    }
}

