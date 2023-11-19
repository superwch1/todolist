namespace todolist.Views;

public partial class TaskView : ContentPage
{
	List<TaskModel> Tasks { get; set;}
	int IntType { get; }

	public TaskView(List<TaskModel> tasks, int intType, AccountDatabase accountDatabase)
	{
		InitializeComponent();

		IntType = intType;

		Tasks = tasks
			.Where(x => x.IntType == intType)
			.OrderBy(x => x.DueDate)
			.ToList();

		collectionView.ItemsSource = Tasks;	
    }


	async void ShowOrHideContent(object sender, EventArgs e)
    {
        var imageButton = sender as ImageButton;
		var selectedTask = imageButton.CommandParameter as TaskModel;
		selectedTask.IsContentVisible = !selectedTask.IsContentVisible;
    }
}
