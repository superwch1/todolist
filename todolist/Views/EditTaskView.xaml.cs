using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views;

public partial class EditTaskView : PopupPage
{
	public HubConnection Connection { get; set; }
	public EditTaskViewModel ViewModel { get; set; }
	TaskModel? Model { get; set; }

	public EditTaskView(TaskModel model, HubConnection connection, int intType)
	{
		InitializeComponent();

		Connection = connection;
		ViewModel = new EditTaskViewModel();
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
#if IOS
		await viewFrame.TranslateTo(0, -100);
#endif
	}


	public async void EditorUnfocused(object sender, FocusEventArgs args)
	{
#if IOS
		await viewFrame.TranslateTo(0, 0);
#endif
	}


	public async void CreateTask(object sender, EventArgs args)
	{
		int intType = intTypePicker.SelectedIndex;
		int intSymbol = intSymbolPicker.SelectedIndex;

		await ViewModel.CreateTask(Connection, intType, topic.Text, content.Text,
			dueDate.Date, intSymbol);
	}

	public async void DeleteTask(object sender, EventArgs args)
	{
		await ViewModel.DeleteTask(Connection, Model.Id);
	}


	public async void UpdateTask(object sender, EventArgs args)
	{
		int intType = intTypePicker.SelectedIndex;
		int intSymbol = intSymbolPicker.SelectedIndex;

		await ViewModel.UpdateTask(Connection, Model.Id, intType, topic.Text, 
			content.Text, dueDate.Date, intSymbol);
	}


	public async void Cancel(object sender, EventArgs args)
	{
		await MopupService.Instance.PopAsync();
	}
}