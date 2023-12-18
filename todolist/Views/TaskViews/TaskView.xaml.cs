using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;


namespace todolist.Views.TaskViews;

public partial class TaskView : ContentPage
{
	public ObservableCollection<TaskModel> Tasks { get; set;} = new ObservableCollection<TaskModel>();
	public int IntType { get; }
	public string JwtToken { get; }
	public bool IsNavigatedFromLaunch { get; set; } //prevent running activated function when it navigates to task view in app launch
	public TaskViewModel ViewModel { get; }
	public DateTime SelectedDateTime { get; set; }
	public double OffSet { get; set; }

	
	public TaskView(List<TaskModel> tasks, int intType, string jwtToken, bool isNavigatedFromLaunch)
	{
		InitializeComponent();

		IntType = intType;
		JwtToken = jwtToken;
		IsNavigatedFromLaunch = isNavigatedFromLaunch;
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


	public async void MinusMonth(object seneder, EventArgs args)
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

	public async void AddMonth(object seneder, EventArgs args)
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


	async void SwipeChanging(object sender, SwipeChangingEventArgs args)
	{
		OffSet = args.Offset;
		var swipeView = (Microsoft.Maui.Controls.SwipeView)sender;

		await ViewModel.SwipeChanging(swipeView, args, OffSet);
	}


	async void SwipeEnded(object sender, SwipeEndedEventArgs args)
	{
		ViewModel.SwipeEnded(sender, args, OffSet, Tasks, scrollview);
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
		await IsLoading.RunMethod(() => MopupService.Instance.PushAsync(new MenuView(width, height, 80, JwtToken)));
	}


	async void EditTask(object sender, TappedEventArgs e)
    {
		var stack = (VerticalStackLayout)sender;
    	var selectedTask = (TaskModel)stack.BindingContext;

		await IsLoading.RunMethod(() => MopupService.Instance.PushAsync(new PopUpView(selectedTask, IntType)));
    }


	async void CreateTask(object sender, TappedEventArgs e)
    {
		await IsLoading.RunMethod(() => MopupService.Instance.PushAsync(new PopUpView(null, IntType)));
    }


	public void InitializeSignalR()
	{
		SignalR.Connection.On<int>("DeleteTask", DeleteTask);
		SignalR.Connection.On<int, int, string, string, string, int>("DeleteThenCreateTask", DeleteThenCreateTask);
		SignalR.Connection.Reconnected += async (connectionId) => 
		{
			await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
		};
	}


	public async Task TerminateSignalR()
	{
		SignalR.Connection.Remove("DeleteTask");
		SignalR.Connection.Remove("DeleteThenCreateTask");
		await SignalR.Connection.StopAsync();
	}


	public async Task DeactivatedFunction()
	{
		await TerminateSignalR();
	}


	public async Task ActivatedFunction()
	{
		//When the app is launched and navigate to the task View,
		//it will run the ActivatedFunction() and the following if-condition prevent it
		if (IsNavigatedFromLaunch == true)
		{
			IsNavigatedFromLaunch = false;
			return;
		}

		if (SignalR.Connection.State == HubConnectionState.Connected) {
			InitializeSignalR();
			await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
			return;
		}

		if (SignalR.Connection.State == HubConnectionState.Disconnected){
			var connection = await SignalR.BuildHubConnection(JwtToken);

			if (connection != null && connection.State == HubConnectionState.Connected)
			{
				SignalR.Connection = connection;
				InitializeSignalR();
				await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
				return;
			}
		}  

		var count = LifeCycleMethods.EnterForegroundCount;

		while(SignalR.Connection.State == HubConnectionState.Disconnected 
			&& count == LifeCycleMethods.EnterForegroundCount){
	
			await Task.Delay(5000);

			if (SignalR.Connection.State == HubConnectionState.Connected) {
				InitializeSignalR();
				await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
				return;
			}

			if (SignalR.Connection.State == HubConnectionState.Disconnected)
			{
				var connection = await SignalR.BuildHubConnection(JwtToken);

				if (connection != null && connection.State == HubConnectionState.Connected){
					SignalR.Connection = connection;
					InitializeSignalR();
					await ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
					return;
				}
			} 
        }   
	}
}
