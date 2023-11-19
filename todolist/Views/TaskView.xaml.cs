namespace todolist.Views;

public partial class TaskView : ContentPage
{
	List<TaskModel> Tasks;

	public TaskView(List<TaskModel> tasks, AccountDatabase accountDatabase)
	{
		InitializeComponent();
		Tasks = tasks;

		collectionView.ItemsSource = Tasks;
    }
}
