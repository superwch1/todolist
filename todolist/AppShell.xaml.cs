namespace todolist;

public partial class AppShell : Shell
{
	public AppShell(List<TaskModel> tasks, AccountDatabase accountDatabase)
	{
		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, accountDatabase);
		followUpTask.Content = new TaskView(tasks, 1, accountDatabase);
    }
}

