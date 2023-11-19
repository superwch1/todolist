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


	async void ShowOrHideContent(object sender, EventArgs e)
    {
        var imageButton = sender as ImageButton;
		var selectedTask = imageButton.CommandParameter as TaskModel;
		selectedTask.IsContentVisible = !selectedTask.IsContentVisible;
    }

	
}
