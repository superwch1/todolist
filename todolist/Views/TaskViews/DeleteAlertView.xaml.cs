using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views.TaskViews;

public partial class DeleteAlertView : PopupPage
{
	public HubConnection Connection { get; set; }
	TaskModel Model { get; set; }

	public DeleteAlertView(HubConnection connection, TaskModel model)
	{
		InitializeComponent();

		Model = model;
		Connection = connection;
	}


	public async void Yes(object sender, EventArgs args)
	{
		try
		{
			await MopupService.Instance.PopAsync();
			await Connection.InvokeAsync("DeleteTask", Model.Id);
		}
		catch 
		{ 
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}

	public async void No(object sender, EventArgs args)
	{
		try
		{
			await MopupService.Instance.PopAsync();
		}
		catch 
		{ 
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}
}