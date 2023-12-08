using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class VerifyPasscodeView : ContentPage, IQueryAttributable
{
	public VerifyPasscodeViewModel ViewModel { get; set; }

	public VerifyPasscodeView()
	{
		InitializeComponent();
		SetControlsProperties();
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		string email = query["email"] as string;	
		ViewModel = new VerifyPasscodeViewModel(email);
    }

	public void SetControlsProperties()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

        passcode.WidthRequest = width * 0.8;
        passcodeBorder.WidthRequest = width * 0.8;
        passcodeBorder.Margin = new Thickness() { Bottom = height * 0.02 };

        stack.WidthRequest = width * 0.8;
    }


	public async void VerifyPasscode(object sender, EventArgs args)
	{
		await IsLoading.RunMethod(() => ViewModel.VerifyPasscode(passcode));
	}
}