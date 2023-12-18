namespace todolist.Views.TaskViews;

public partial class TaskShell : Shell
{
	public TaskShell(List<TaskModel> tasks, string jwtToken, bool isNavigatedFromLaunch)
	{
		Routing.RegisterRoute("searchview", typeof(SearchView));
		Routing.RegisterRoute("resetpassword", typeof(ResetPasswordView));
		Routing.RegisterRoute("policy", typeof(PolicyView));

		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, jwtToken, isNavigatedFromLaunch);
		followUpTask.Content = new TaskView(tasks, 1, jwtToken, isNavigatedFromLaunch);
    }
}

