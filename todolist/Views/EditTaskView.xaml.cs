using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views;

public partial class EditTaskView : PopupPage
{
	public HubConnection Connection { get; set; }
	TaskModel? Model { get; set; }

	public EditTaskView(TaskModel model, HubConnection connection, int intType)
	{
		InitializeComponent();

		Connection = connection;
		Model = model;
		
		if(intType == 0){
			myTask.IsChecked = true;
		}
		else {
			followUpTask.IsChecked = true;
		}

		if (model != null)
		{
			createOrUpdate.Clicked += UpdateTask;
			createOrUpdate.Text = "Update";
			cancelOrDelete.Clicked += DeleteTask;
			cancelOrDelete.Text = "Delete";

			topic.Text = model.Topic;
			dueDate.Date = model.DueDate;
			content.Text = model.Content;

			if(model.IntSymbol == 0){
				notYet.IsChecked = true;
			}
			else {
				done.IsChecked = true;
			}
		}
		else 
		{
			createOrUpdate.Clicked += CreateTask;
			createOrUpdate.Text = "Create";
			cancelOrDelete.Clicked += Cancel;
			cancelOrDelete.Text = "Cancel";
			notYet.IsChecked = true;
	
		}
	}

	public async void CreateTask(object sender, EventArgs args)
	{
		int intType = myTask.IsChecked == true ? 0 : 1;
		int intSymbol = notYet.IsChecked == true ? 0 : 1;

		if (String.IsNullOrEmpty(topic.Text))
		{
			await ToastBar.DisplayToast("Please enter topic");
			return;
		}

		try 
		{
			await Connection.InvokeAsync("CreateTask", 
				intType, topic.Text, content.Text, dueDate.Date.ToString("dd-MM-yyyy"), intSymbol);
			await MopupService.Instance.PopAsync();
		}
		catch
		{
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}

	public async void DeleteTask(object sender, EventArgs args)
	{
		try 
		{
			await Connection.InvokeAsync("DeleteTask", Model.Id);
			await MopupService.Instance.PopAsync();
		}
		catch
		{
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}


	public async void UpdateTask(object sender, EventArgs args)
	{
		int intType = myTask.IsChecked == true ? 0 : 1;
		int intSymbol = notYet.IsChecked == true ? 0 : 1;

		if (String.IsNullOrEmpty(topic.Text))
		{
			await ToastBar.DisplayToast("Please enter topic");
			return;
		}

		try 
		{
			await Connection.InvokeAsync("UpdateTask", 
				Model.Id, intType, topic.Text, content.Text, dueDate.Date.ToString("dd-MM-yyyy"), intSymbol);
			await MopupService.Instance.PopAsync();
		}
		catch
		{
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}

	public async void Cancel(object sender, EventArgs args)
	{
		await MopupService.Instance.PopAsync();
	}
}