using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;

namespace todolist.Views;

public partial class TaskView : ContentPage
{
	public ObservableCollection<TaskModel> Tasks { get; set;} = new ObservableCollection<TaskModel>();
	public int IntType { get; }
	public HubConnection Connection { get; set; }
	public string JwtToken { get; }
	public AccountDatabase Database { get;}

	public TaskView(List<TaskModel> tasks, int intType, string jwtToken, 
		HubConnection connection, AccountDatabase accountDatabase)
	{
		InitializeComponent();

		IntType = intType;
		JwtToken = jwtToken;
		Connection = connection;
		Database = accountDatabase;

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


	void DeleteTask(int id)
	{
		//since the method is invoked in SignalR, it need to use MainThread for browsing UI
		MainThread.BeginInvokeOnMainThread(() =>
		{
			var task = Tasks.FirstOrDefault(x => x.Id == id);
			if (task != null)
			{
				task.IsContentVisible = false;
				task.IsTopicVisible = false;

				var index = Tasks.IndexOf(task);
				Tasks.RemoveAt(index);
			}

			scrollview.ForceLayout();
			(scrollview as IView).InvalidateMeasure();
		});		
	}


	void DeleteThenCreateTask(int id, int intType, string topic, string content, string dueDate, int intSymbol)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			Tasks.Add(new TaskModel() { Id = id, IntType = intType, Topic = topic, Content = content,
				DueDate = Convert.ToDateTime(dueDate), IntSymbol = intSymbol});

		});		
	}


	void ShowOrHideContent(object sender, TappedEventArgs e)
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
