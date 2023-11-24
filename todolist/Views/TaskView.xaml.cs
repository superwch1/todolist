﻿using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;

namespace todolist.Views;

public partial class TaskView : ContentPage
{
	public ObservableCollection<TaskModel> Tasks { get; set;} = new ObservableCollection<TaskModel>();
	public int IntType { get; }
	public HubConnection Connection { get; set; }
	public string JwtToken { get; }
	public TaskViewModel ViewModel { get; }
	public DateTime SelectedDateTime { get; set; }

	//remember to remove twice for functions in lifecycle
	public TaskView(List<TaskModel> tasks, int intType, string jwtToken, 
		HubConnection connection)
	{
		InitializeComponent();

		IntType = intType;
		JwtToken = jwtToken;
		Connection = connection;
		ViewModel = new TaskViewModel();
		SelectedDateTime = DateTime.Now;

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


	void DeleteTask(int id)
	{
		ViewModel.DeleteTask(Tasks, scrollview, id);
	}


	void DeleteThenCreateTask(int id, int intType, string topic, string content, string dueDate, int intSymbol)
	{
		ViewModel.DeleteTask(Tasks, scrollview, id);
		ViewModel.CreateTask(Tasks, scrollview, IntType, id, intType, topic, content, dueDate, intSymbol);
	}


	void ShowOrHideContent(object sender, TappedEventArgs e)
    {
		var frame = (Frame)sender;
    	var selectedTask = (TaskModel)frame.BindingContext;

		ViewModel.ShowOrHideContent(Tasks, scrollview, selectedTask);
    }


	async void EditTask(object sender, TappedEventArgs e)
    {
		var stack = (VerticalStackLayout)sender;
    	var selectedTask = (TaskModel)stack.BindingContext;

		await MopupService.Instance.PushAsync(new EditTaskView(selectedTask, Connection));
    }


	public void InitializeSignalR()
	{
		Connection.On<int>("DeleteTask", DeleteTask);
		Connection.On<int, int, string, string, string, int>("DeleteThenCreateTask", DeleteThenCreateTask);
		Connection.Reconnected += async (connectionId) => 
		{
			await Task.Run(() => ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken));
		};
	}


	public async Task DeactivatedFunction()
	{
		Connection.Remove("DeleteTask");
		Connection.Remove("DeleteThenCreateTask");
		await Connection.StopAsync();
	}


	public async Task ActivatedFunction()
	{
		if (Connection.State == HubConnectionState.Disconnected){
			var connection = await SignalR.BuildHubConnection(JwtToken);

			if (connection != null && connection.State == HubConnectionState.Connected)
			{
				Connection = connection;
				InitializeSignalR();
				ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
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
					ViewModel.CounterCheckTask(Tasks, scrollview, SelectedDateTime, IntType, JwtToken);
				}
			} 
        }   
	}
}
