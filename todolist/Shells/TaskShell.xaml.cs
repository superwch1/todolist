using Microsoft.AspNetCore.SignalR.Client;

namespace todolist.Shells;

public partial class TaskShell : Shell
{
	public TaskShell(List<TaskModel> tasks, string jwtToken, HubConnection connection)
	{
		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, jwtToken, connection);
		followUpTask.Content = new TaskView(tasks, 1, jwtToken, connection);
    }
}

