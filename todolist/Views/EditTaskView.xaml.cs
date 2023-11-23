using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views;

public partial class EditTaskView : PopupPage
{
	public HubConnection Connection { get; set; }
	TaskModel? Model { get; set; }

	public EditTaskView(TaskModel model, HubConnection connection)
	{
		InitializeComponent();

		Connection = connection;
		
		Model = model;
		topic.Text = model.Topic;
		dueDate.Date = model.DueDate;
		content.Text = model.Content;

		if(model.IntType == 0){
			myTask.IsChecked = true;
		}
		else {
			followUpTask.IsChecked = true;
		}

		if(model.IntSymbol == 0){
			notYet.IsChecked = true;
		}
		else {
			done.IsChecked = true;
		}
	}

	public async void CreateTask(Object sender, EventArgs args)
	{
		int intType = myTask.IsChecked == true ? 0 : 1;
		int intSymbol = notYet.IsChecked == true ? 0 : 1;

		try 
		{
			await Connection.InvokeAsync("CreateTask", 
				intType, topic.Text, content.Text, DateTime.Now.ToString("dd-MM-yyyy"), intSymbol);
			await MopupService.Instance.PopAsync();
		}
		catch
		{
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}

	public async void DeleteTask(Object sender, EventArgs args)
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


	public async void UpdateTask(Object sender, EventArgs args)
	{
		int intType = myTask.IsChecked == true ? 0 : 1;
		int intSymbol = notYet.IsChecked == true ? 0 : 1;
	}

	
}