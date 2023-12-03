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
			var task2 = MopupService.Instance.PopAsync();
			var task1 = Navigation.PushAsync(new ForgetPasswordView());

			await Task.WhenAll(task1, task2);
		});
	}

	async void Logout(object sender, TappedEventArgs args)
	{	
		await ViewModel.Logout(Connection);
	}
}