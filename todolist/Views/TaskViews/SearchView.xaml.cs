using System.Collections.ObjectModel;
using System.Net;
using Mopups.Services;

namespace todolist.Views.TaskViews;


public partial class SearchView : ContentPage, IQueryAttributable
{
	public ObservableCollection<TaskModel> MyTasks { get; set;} = new ObservableCollection<TaskModel>();
	public ObservableCollection<TaskModel> FollowupTasks { get; set;} = new ObservableCollection<TaskModel>();
	public string JwtToken { get; set; }
	public string Keyword { get; set; }
	public TaskViewModel ViewModel { get; } = new TaskViewModel(DateTime.Now);

	
	public SearchView()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
#if IOS
		Shell.SetTabBarIsVisible(this, false);
#endif

		search.ReturnType = ReturnType.Go;
		search.ReturnCommand = new Command(async () => {			
			await IsLoading.RunMethod(async() => 
			{
				var code = await ViewModel.SearchTask(MyTasks, FollowupTasks, 
					myTaskStackLayout, followupTaskStackLayout, 
				    myTaskScrollView, followupTaskScrollView, search.Text, JwtToken);
				
				if (code == HttpStatusCode.OK)
				{
					Keyword = search.Text;
				}
			});
		});
    }

	//workaround for grey screen in android for setting TabBar invisible 
	protected override async void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

#if ANDROID
		Shell.SetTabBarIsVisible(this, false);
		/*
		var shellTabHeight = 56;
		workAroundScrollView.IsVisible = true;
		workAroundScrollView.Margin = new Thickness() { Top = grid.Height + shellTabHeight - 5 };
		*/
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

	public async void GoBack(object sender, TappedEventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync(".."));
    }


   	public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		JwtToken = query["jwtToken"] as string;
		List<TaskModel> tasks = query["tasks"] as List<TaskModel>;
		Keyword = query["keyword"] as string;

		var myTasks = tasks
			.Where(x => x.IntType == 0)
			.OrderBy(x => x.DueDate)
			.ToList();

		var followupTasks = tasks
			.Where(x => x.IntType == 1)
			.OrderBy(x => x.DueDate)
			.ToList();

		search.Text = Keyword;

		MyTasks = new ObservableCollection<TaskModel>(myTasks);
		FollowupTasks = new ObservableCollection<TaskModel>(followupTasks);

		BindableLayout.SetItemsSource(myTaskStackLayout, MyTasks);	
		BindableLayout.SetItemsSource(followupTaskStackLayout, FollowupTasks);	
    }

	public void ShowOrHideContent(object sender, TappedEventArgs e)
    {
		var frame = (Frame)sender;
    	var selectedTask = (TaskModel)frame.BindingContext;

		if (selectedTask.IntType == 0)
		{
			ViewModel.ShowOrHideContent(MyTasks, myTaskScrollView, selectedTask);
		}
		else 
		{
			ViewModel.ShowOrHideContent(FollowupTasks, followupTaskScrollView, selectedTask);
		}	
    }

	
	public async void EditTask(object sender, TappedEventArgs e)
    {
		var frame = (Frame)sender;
    	var selectedTask = (TaskModel)frame.BindingContext;

		var popUpView = new PopUpView(selectedTask, selectedTask.IntType);
		await MopupService.Instance.PushAsync(popUpView);

		//after the popup is disappeared, it runs the following function
		popUpView.Disappearing += async (sender, e) => {
			await ViewModel.CounterCheckTaskFromKeyword(
				MyTasks, FollowupTasks, myTaskScrollView, followupTaskScrollView, Keyword, JwtToken);
		};
    }
}
