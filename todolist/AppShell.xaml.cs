namespace todolist;

public partial class AppShell : Shell
{
	public AppShell(List<TaskModel> tasks, AccountDatabase accountDatabase)
	{
		InitializeComponent();
		myTask.Content = new TaskView(tasks, accountDatabase);
		followUpTask.Content = new TaskView(tasks, accountDatabase);
    }
}

