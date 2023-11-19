namespace todolist.Views;

public partial class TaskView : ContentPage
{
	List<TaskModel> Tasks { get; set;} = new List<TaskModel>();
	int IntType { get; }

	public TaskView(List<TaskModel> tasks, int intType, AccountDatabase accountDatabase)
	{
		InitializeComponent();

		IntType = intType;

		Tasks = tasks
			.Where(x => x.IntType == IntType)
			.OrderBy(x => x.DueDate)
			.ToList();

		BindableLayout.SetItemsSource(collectionView, Tasks);
    }


	async void ShowOrHideContent(object sender, EventArgs e)
    {
		var imageButton = sender as ImageButton;
		var selectedTask = imageButton.CommandParameter as TaskModel;

		for(var i = 0; i < Tasks.Count; i++){
			if (Tasks[i].Id == selectedTask.Id){
				Tasks[i].IsContentVisible = !selectedTask.isContentVisible;
			}
		}

		//reduce the height of scrollview after change back to invisible
		(scrollview as IView).InvalidateMeasure();
    }
}
