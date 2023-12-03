namespace todolist.Views;

public partial class VerifyPasscodeView : ContentPage
{
	public VerifyPasscodeViewModel ViewModel { get; }

	public VerifyPasscodeView(string email)
	{
		InitializeComponent();
		SetControlsProperties();

		ViewModel = new VerifyPasscodeViewModel(Navigation, email);
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