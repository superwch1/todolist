using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace todolist.Views;

public partial class TaskView : ContentPage
{
	public ObservableCollection<TaskModel> Tasks { get; set;} = new ObservableCollection<TaskModel>();
	public int IntType { get; }
	public HubConnection Connection { get; set; }
	public string JwtToken { get; }
	public TaskViewModel ViewModel { get; }
	public DateTime SelectedDateTime { get; set; }

	
	public TaskView(List<TaskModel> tasks, int intType, string jwtToken, 
		HubConnection connection)
	{
		InitializeComponent();

		IntType = intType;
		JwtToken = jwtToken;
		Connection = connection;
		SelectedDateTime = DateTime.Now;
		ViewModel = new TaskViewModel(SelectedDateTime);
		selectedPeriod.Text = SelectedDateTime.ToString("MMM yyyy");

		tasks = tasks
			.Where(x => x.IntType == IntType)
			.OrderBy(x => x.IntSymbol)
			.ThenBy(x => x.DueDate)
			.ToList();

		Tasks = new ObservableCollection<TaskModel>(tasks);

		BindableLayout.SetItemsSource(collectionView, Tasks);	

		LifeCycleMethods.ActivatedActions.Add(ActivatedFunction);
		LifeCycleMethods.DeactivatedActions.Add(DeactivatedFunction);	
		InitializeSignalR();
    }

	public async void AddMonth(object seneder, EventArgs args)
	{
        await IsLoading.RunMethod(async () =>
		{
			SelectedDateTime = SelectedDateTime.AddMonths(-1);
			selectedPeriod.Text = SelectedDateTime.ToString("MMM yyyy");
			ViewModel.SelectedDateTime = SelectedDateTime;

			ViewModel.DeleteAllTask(Tasks, scrollview);
			await ViewModel.ReadTaskFromSelectedPeriod(Tasks, scrollview, SelectedDateTime, JwtToken, IntType);
		});
	}

	public async void MinusMonth(object seneder, EventArgs args)
	{
		await IsLoading.RunMethod(async () =>
		{
			SelectedDateTime = SelectedDateTime.AddMonths(1);
			selectedPeriod.Text = SelectedDateTime.ToString("MMM yyyy");
			ViewModel.SelectedDateTime = SelectedDateTime;

			ViewModel.DeleteAllTask(Tasks, scrollview);
			await ViewModel.ReadTaskFromSelectedPeriod(Tasks, scrollview, SelectedDateTime, JwtToken, IntType);
		});
		
	}


	void DeleteTask(int id)
	{
		ViewModel.DeleteTask(Tasks, scrollview, id);
	}


	void DeleteThenCreateTask(int id, int intType, string topic, string content, string dueDate, int intSymbol)
	{
		ViewModel.DeleteTask(Tasks, scrollview, id);
		ViewModel.CreateTask(Tasks, scrollview, false, IntType, id, intType, topic, content, dueDate, intSymbol);
	}


	void ShowOrHideContent(object sender, TappedEventArgs e)
    {
		var frame = (Frame)sender;
    	var selectedTask = (TaskModel)frame.BindingContext;

		ViewModel.ShowOrHideContent(Tasks, scrollview, selectedTask);
    }

	async void OpenMenu(object seender, TappedEventArgs e)
	{
		var width = grid.Width;
		var height = grid.Height;

#if ANDROID
		height += 56; //height of shell bar
#elif IOS
		height -= On<iOS>().SafeAreaInsets().Top;
#endif

		//80 - Margin top 
		await MopupService.Instance.PushAsync(new MenuView(width, height, 80, Connection, JwtToken));	
	}


	async void EditTask(object sender, TappedEventArgs e)
    {
		var stack = (VerticalStackLayout)sender;
    	var selectedTask = (TaskModel)stack.BindingContext;

		await MopupService.Instance.PushAsync(new PopUpView(selectedTask, Connection, IntType));
    }


	async void CreateTask(object sender, TappedEventArgs e)
    {
		await MopupService.Instance.PushAsync(new PopUpView(null, Connection, IntType));
    }


	public void InitializeSignalR()
	{
		Connection.On<int>("DeleteTask", DeleteTask);
		Connection.On<int, int, string, string, string, int>("DeleteThenCreateTask", DeleteThenCreateTask);
		Connection.Reconnected += async (connectionId) => 
		{
			await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
		};
	}

	public async Task TerminateSignalR()
	{
		Connection.Remove("DeleteTask");
		Connection.Remove("DeleteThenCreateTask");
		await Connection.StopAsync();
	}


	public async Task DeactivatedFunction()
	{
		await TerminateSignalR();
	}


	public async Task ActivatedFunction()
	{
		if (Connection.State == HubConnectionState.Disconnected){
			var connection = await SignalR.BuildHubConnection(JwtToken);

			if (connection != null && connection.State == HubConnectionState.Connected)
			{
				Connection = connection;
				InitializeSignalR();
				await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
			}
		}  

		var count = LifeCycleMethods.EnterForegroundCount;

		while(Connection.State == HubConnectionState.Disconnected 
			&& count == LifeCycleMethods.EnterForegroundCount){
	
			await Task.Delay(5000);
			if (Connection.State == HubConnectionState.Disconnected)
			{
				var connection = await SignalR.BuildHubConnection(JwtToken);

				if (connection != null && connection.State == HubConnectionState.Connected){
					Connection = connection;
					InitializeSignalR();
					await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
				}
			} 
        }   
	}
}
