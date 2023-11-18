namespace todolist;

public partial class AppShell : Shell
{
	public AppShell(List<TaskModel> tasks)
	{
		InitializeComponent();
		myTask.Content = new TaskView(tasks);
		followUpTask.Content = new TaskView(tasks);
    }
}

