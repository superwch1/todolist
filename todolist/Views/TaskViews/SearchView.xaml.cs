﻿using System.Collections.ObjectModel;
using Mopups.Services;
using todolist.ViewModels.TaskViewModels;

namespace todolist.Views.TaskViews;


public partial class SearchView : ContentPage, IQueryAttributable
{
	public ObservableCollection<TaskModel> Tasks { get; set;} = new ObservableCollection<TaskModel>();
	public string JwtToken { get; set; }
	public string Keyword { get; set; }
	public double OffSet { get; set; }
	public TaskViewModel ViewModel { get; } = new TaskViewModel(DateTime.Now);

	
	public SearchView()
	{
		InitializeComponent();
    }

	//workaround for grey screen in android for setting TabBar invisible 
	protected override async void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		Shell.SetTabBarIsVisible(this, false);

#if ANDROID
		var shellTabHeight = 56;
		workAroundScrollView.IsVisible = true;
		workAroundScrollView.Margin = new Thickness() { Top = grid.Height + shellTabHeight - 5 };
#endif
	}


	//workaround for the add button hidden under tab after navigate back from serach view in IOS
	//it happens when you expand all of the task in search view then go backwards
	protected override void OnDisappearing()
	{
		base.OnDisappearing();

#if IOS
		Shell.SetTabBarIsVisible(this, true);
#endif
	}


   	public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		JwtToken = query["jwtToken"] as string;
		List<TaskModel> tasks = query["tasks"] as List<TaskModel>;
		Keyword = query["keyword"] as string;

		tasks = tasks
			.OrderBy(x => x.DueDate)
			.ToList();

		Tasks = new ObservableCollection<TaskModel>(tasks);
		BindableLayout.SetItemsSource(collectionView, Tasks);	

		Title = $"Search Result: {Keyword}";
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

		var popUpView = new PopUpView(selectedTask, selectedTask.IntType);
		await MopupService.Instance.PushAsync(popUpView);

		//after the popup is disappeared, it runs the following function
		popUpView.Disappearing += async (sender, e) => {
			await ViewModel.CounterCheckTaskFromKeyword(Tasks, scrollview, Keyword, JwtToken);
		};
    }
}
