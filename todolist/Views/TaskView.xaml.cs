namespace todolist.Views;

public partial class TaskView : ContentPage
{
	List<TaskModel> Tasks;

	public TaskView(List<TaskModel> tasks)
	{
		InitializeComponent();
		Tasks = tasks;

		collectionView.ItemsSource = Tasks;
    }
}
