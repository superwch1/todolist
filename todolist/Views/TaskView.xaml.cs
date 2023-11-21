using System.Collections.ObjectModel;
using Mopups.Services;

namespace todolist.Views;

public partial class TaskView : ContentPage
{
	ObservableCollection<TaskModel> Tasks { get; set;} = new ObservableCollection<TaskModel>();
	int IntType { get; }

	public TaskView(List<TaskModel> tasks, int intType, AccountDatabase accountDatabase)
	{
		InitializeComponent();

		IntType = intType;

		tasks = tasks
			.Where(x => x.IntType == IntType)
			.OrderBy(x => x.DueDate)
			.ToList();
		
		Tasks = new ObservableCollection<TaskModel>(tasks);

		BindableLayout.SetItemsSource(collectionView, Tasks);
    }


	async void ShowOrHideContent(object sender, TappedEventArgs e)
    {
		var image = (Image)sender;
    	var selectedTask = (TaskModel)image.BindingContext;

		// Use LINQ to find the task
		var task = Tasks.FirstOrDefault(t => t.Id == selectedTask.Id);

		if (task != null)
		{
			task.IsContentVisible = !selectedTask.IsContentVisible;
			task.ArrowImageSource = task.IsContentVisible == true ? "downarrow" : "uparrow";
		}

		//reduce the height of scrollview after change back to invisible
		(scrollview as IView).InvalidateMeasure();
    }


	async void EditTask(object sender, TappedEventArgs e)
    {
		var image = (VerticalStackLayout)sender;
    	var selectedTask = (TaskModel)image.BindingContext;

		await MopupService.Instance.PushAsync(new EditTaskView(selectedTask));
    }

}
