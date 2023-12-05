using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views;

public partial class MenuView : PopupPage
{
	public double DeviceWidth { get; set; }
	public double DeviceHeight { get; set; }
	public HubConnection Connection { get; set; }
	public string JwtToken { get; set; }
	public MenuViewModel ViewModel { get; }

	public MenuView(double width, double height, double marginTop, HubConnection connection,
		string jwtToken)
	{
		InitializeComponent();	
		DeviceWidth = width;
		DeviceHeight = height;
		Connection = connection;
		JwtToken = jwtToken;
		ViewModel = new MenuViewModel();

		viewFrame.WidthRequest = width * 0.6;
		viewFrame.HeightRequest = height - marginTop;
		viewFrame.Margin = new Thickness() { Top = marginTop };

		search.ReturnType = ReturnType.Go;
		search.ReturnCommand = new Command(async () => {			
			await ViewModel.SearchTask(Connection, search.Text, JwtToken, DeviceHeight);

			//workaround for grey screen in android for setting TabBar invisible 
			/*
				protected override void OnSizeAllocated(double width, double height)
				{
					base.OnSizeAllocated(width, height);
					Shell.SetTabBarIsVisible(this, false);
				}
			*/
		});

		creditLabel.Text = "By Fiona C & John W";
	}

	async void Logout(object sender, TappedEventArgs args)
	{	
		await ViewModel.Logout(Connection);	
	}
}