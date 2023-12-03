using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views;

public partial class MenuView : PopupPage
{
	public double DeviceWidth { get; set; }
	public double DeviceHeight { get; set; }
	public HubConnection Connection { get; set; }
	public MenuViewModel ViewModel { get; }

	public MenuView(double width, double height, double marginTop, HubConnection connection)
	{
		InitializeComponent();	
		DeviceWidth = width;
		DeviceHeight = height;
		Connection = connection;
		ViewModel = new MenuViewModel();

		viewFrame.WidthRequest = width / 2;
		viewFrame.HeightRequest = height - marginTop;
		viewFrame.Margin = new Thickness() { Top = marginTop };

		search.ReturnType = ReturnType.Go;
		search.ReturnCommand = new Command(async () => {			
			await MopupService.Instance.PopAsync();
			await Shell.Current.GoToAsync("login"); 

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