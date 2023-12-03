using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace todolist.Views.TaskViews;

public partial class HomeView : Microsoft.Maui.Controls.TabbedPage
{
	public HomeView(List<TaskModel> tasks, string jwtToken, HubConnection connection)
	{
		NavigationPage.SetHasNavigationBar(this, false);

		InitializeComponent();

		var myTaskView = new TaskView(tasks, 0, jwtToken, connection) 
		{ 
			Title = "My Task", 
			IconImageSource = "mytask"
		};

		var followUpTaskView = new TaskView(tasks, 1, jwtToken, connection) 
		{ 
			Title = "FollowUp Task", 
			IconImageSource = "followuptask"
		};

		Children.Add(myTaskView);
		Children.Add(followUpTaskView);

		#if ANDROID
			//set SelectedTabColor will lead to crash
			BarBackgroundColor = Color.FromRgba(254, 255, 245, 255);
			BarTextColor = Color.FromRgba(87, 71, 42, 255);
			UnselectedTabColor = Color.FromRgba(87, 71, 42, 255);

			On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
				.SetToolbarPlacement(ToolbarPlacement.Bottom);
		#elif IOS
			SelectedTabColor = Color.FromRgba(87, 71, 42, 255);
		#endif	
    }
}

