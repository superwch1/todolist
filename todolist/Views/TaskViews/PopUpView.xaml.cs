using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views.TaskViews;

public partial class PopUpView : PopupPage
{
	public PopUpViewModel ViewModel { get; set; }
	TaskModel? Model { get; set; }

	public PopUpView(TaskModel model, int intType)
	{
		InitializeComponent();

		ViewModel = new PopUpViewModel();
		Model = model;
		
		typeLabel.Text = intType == 0 ? "My Task" : "FollowUp Task";
		typeImage.Source = intType == 0 ? "mytask" : "followuptask";
		typeFrame.BorderColor = intType == 0 ? Color.FromRgba(124, 148, 113, 255) : Color.FromRgba(99, 113, 125, 255);
		typeFrame.BackgroundColor = intType == 0 ? Color.FromRgba(230, 241, 225, 255) : Color.FromRgba(213, 222, 230, 255);

		cancel.Clicked += Cancel;
		cancel.Text = "Cancel";

		//need to be further adjust in the future
		//the editor did not auto fill up the available space in frame
		content.HeightRequest = 200;
		contentFrame.HeightRequest = 200;

		if (model != null)
		{
			createOrUpdate.Clicked += UpdateTask;
			createOrUpdate.Text = "Update";
			
			topic.Text = model.Topic;
			dueDate.Date = model.DueDate;
			content.Text = model.Content;
		}
		else 
		{
			createOrUpdate.Clicked += CreateTask;
			createOrUpdate.Text = "Create";
			dueDate.Date = DateTime.Now;
		}
	}


	public async void EditorFocused(object sender, FocusEventArgs args)
	{
#if IOS
		await viewFrame.TranslateTo(0, -110);
#endif
	}


	public async void EditorUnfocused(object sender, FocusEventArgs args)
	{
#if IOS
		await viewFrame.TranslateTo(0, 0);
#endif
	}

	public async void ChangeType(object sender, TappedEventArgs args)
	{
		if (typeLabel.Text == "My Task")
		{
			typeLabel.Text = "FollowUp Task";
			typeImage.Source = "followuptask";
			typeFrame.BorderColor = Color.FromRgba(99, 113, 125, 255);
			typeFrame.BackgroundColor = Color.FromRgba(213, 222, 230, 255);
		}
		else 
		{
			typeLabel.Text = "My Task";
			typeImage.Source = "mytask";
			typeFrame.BorderColor = Color.FromRgba(124, 148, 113, 255);
			typeFrame.BackgroundColor = Color.FromRgba(230, 241, 225, 255);
		}
	}


	public async void CreateTask(object sender, EventArgs args)
	{
		int intType = typeLabel.Text == "My Task" ? 0 : 1;
		int intSymbol = Model == null ? 0 : Model.IntSymbol;

		await IsLoading.RunMethod(() => ViewModel.CreateTask(intType, topic.Text, content.Text,
			dueDate.Date, intSymbol));	
	}


	public async void UpdateTask(object sender, EventArgs args)
	{
		int intType = typeLabel.Text == "My Task" ? 0 : 1;
		int intSymbol = Model == null ? 0 : Model.IntSymbol;

		await IsLoading.RunMethod(() => ViewModel.UpdateTask(Model.Id, intType, topic.Text, 
			content.Text, dueDate.Date, intSymbol));	
	}


	public async void Cancel(object sender, EventArgs args)
	{
		try
		{
			await MopupService.Instance.PopAsync();
		}
		catch { }
	}
}