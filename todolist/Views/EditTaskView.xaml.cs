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
		
		intTypePicker.SelectedIndex = intType == 0 ? 0 : 1;

		//need to be further adjust in the future
		//the editor did not auto fill up the available space in frame
		content.HeightRequest = 220;
		contentFrame.HeightRequest = 220;

		if (model != null)
		{
			createOrUpdate.Clicked += UpdateTask;
			createOrUpdate.Text = "Update";
			cancelOrDelete.Clicked += DeleteTask;
			cancelOrDelete.Text = "Delete";

			topic.Text = model.Topic;
			dueDate.Date = model.DueDate;
			content.Text = model.Content;
			intSymbolPicker.SelectedIndex = model.IntSymbol == 0 ? 0 : 1;
		}
		else 
		{
			createOrUpdate.Clicked += CreateTask;
			createOrUpdate.Text = "Create";
			cancelOrDelete.Clicked += Cancel;
			cancelOrDelete.Text = "Cancel";
			intSymbolPicker.SelectedIndex = 0;
			dueDate.Date = DateTime.Now;
	
		}
	}

	public async void EditorFocused(object sender, FocusEventArgs args)
	{
		await viewFrame.TranslateTo(0, -100);
	}

	public async void EditorUnfocused(object sender, FocusEventArgs args)
	{
		await viewFrame.TranslateTo(0, 0);
	}

	public async void CreateTask(object sender, EventArgs args)
	{
		int intType = intTypePicker.SelectedIndex;
		int intSymbol = intSymbolPicker.SelectedIndex;

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
		int intType = intTypePicker.SelectedIndex;
		int intSymbol = intSymbolPicker.SelectedIndex;

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