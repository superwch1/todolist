using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views.TaskViews;

public partial class DeleteAlertView : PopupPage
{
	TaskModel Model { get; set; }

	public DeleteAlertView(TaskModel model)
	{
		InitializeComponent();

		Model = model;
	}


	public async void Yes(object sender, EventArgs args)
	{
		try
		{
			await MopupService.Instance.PopAsync();
			await SignalR.Connection.InvokeAsync("DeleteTask", Model.Id);
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