using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using todolist.ViewModels.TaskViewModels;


namespace todolist.Views.TaskViews;

public partial class MenuView : PopupPage
{
	public string JwtToken { get; set; }
	public MenuViewModel ViewModel { get; }

	public MenuView(double width, double height, double marginTop,
		string jwtToken)
	{
		InitializeComponent();	
		
		JwtToken = jwtToken;
		ViewModel = new MenuViewModel();

		viewFrame.WidthRequest = width * 0.65;
		viewFrame.HeightRequest = height - marginTop;
		viewFrame.Margin = new Thickness() { Top = marginTop };

		search.ReturnType = ReturnType.Go;
		search.ReturnCommand = new Command(async () => {			
			await ViewModel.SearchTask(search.Text, JwtToken);
		});

		creditLabel.Text = "By Fiona C & John W";
	}

	async void Logout(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.Logout());
	}

	async void ResetPassword(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ResetPassword(JwtToken));
	}

	async void ReadPrivacyPolicy(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ReadPrivacyPolicy());
	}

	async void ReadTermsAndConditions(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ReadTermsAndConditions());
	}

	async void DeleteAccount(object sender, TappedEventArgs args)
	{
		await IsLoading.RunMethod(() => ViewModel.DeleteAccount());
	}
}