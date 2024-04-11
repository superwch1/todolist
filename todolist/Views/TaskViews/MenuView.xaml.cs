using Mopups.Pages;


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

		buildLabel.Text = $"Version: {AppInfo.Current.VersionString}+{AppInfo.Current.BuildString}";
		creditLabel.Text = "By Fiona C & John W";
	}

	public async void Logout(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.Logout());
	}

	public async void ResetPassword(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ResetPassword(JwtToken));
	}

	public async void ReadPrivacyPolicy(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ReadPrivacyPolicy());
	}

	public async void ReadTermsAndConditions(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ReadTermsAndConditions());
	}

	public async void DeleteAccount(object sender, TappedEventArgs args)
	{
		await IsLoading.RunMethod(() => ViewModel.DeleteAccount(JwtToken));
	}
}