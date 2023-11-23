using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;

namespace todolist.Views;

public partial class TaskView : ContentPage
{
	public ObservableCollection<TaskModel> Tasks { get; set;} = new ObservableCollection<TaskModel>();
	public int IntType { get; }
	public HubConnection Connection { get; set; }
	public string JwtToken { get; }

	public TaskView(List<TaskModel> tasks, int intType, string jwtToken, 
		HubConnection connection, AccountDatabase accountDatabase)
	{
		InitializeComponent();

		IntType = intType;
		JwtToken = jwtToken;
		Connection = connection;

		tasks = tasks
			.Where(x => x.IntType == IntType)
			.OrderBy(x => x.IntSymbol)
			.ThenBy(x => x.DueDate)
			.ToList();
		
		tasks.Add(new TaskModel() { Id = 0, Topic = "ABC", Content = "ABC", DueDate = DateTime.Now, IntType = 0, IntSymbol = 0, isTopicVisible = false });

		Tasks = new ObservableCollection<TaskModel>(tasks);

		BindableLayout.SetItemsSource(collectionView, Tasks);		

		Connection.On<int>("DeleteTask", DeleteTask);
		Connection.On<int, int, string, string, string, int>("DeleteThenCreateTask", DeleteThenCreateTask);
    }

	void DeleteTask(int id){
		var task = Tasks.FirstOrDefault(x => x.Id == id);
		task.IsContentVisible = false;
		task.IsTopicVisible = false;

		var index = Tasks.IndexOf(task);
		Tasks.RemoveAt(index);

		scrollview.ForceLayout();
		(scrollview as IView).InvalidateMeasure();
	}

	//when you create a new Task, IOS will have a error - maybe using scorllview instead of listview
	void DeleteThenCreateTask(int id, int intType, string topic, string content, string dueDate, int intSymbol){

		var task = Tasks.FirstOrDefault(x => x.Id == 0);
		task.isTopicVisible = true;
		task.isContentVisible = true;

		scrollview.ForceLayout();
		(scrollview as IView).InvalidateMeasure();
	}


	async void ShowOrHideContent(object sender, TappedEventArgs e)
    {
		var frame = (Frame)sender;
    	var selectedTask = (TaskModel)frame.BindingContext;

		// Use LINQ to find the task
		var task = Tasks.FirstOrDefault(x => x.Id == selectedTask.Id);

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
		var stack = (VerticalStackLayout)sender;
    	var selectedTask = (TaskModel)stack.BindingContext;

		await MopupService.Instance.PushAsync(new EditTaskView(selectedTask, Connection));
    }
}
