namespace todolist.Views;

public partial class ForgetPasswordView : ContentPage
{
    public ForgetPasswordViewModel ViewModel { get; }

	public ForgetPasswordView()
	{
		InitializeComponent();
		SetControlsProperties();

        ViewModel = new ForgetPasswordViewModel(Navigation);
	}

	public void SetControlsProperties()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

        email.WidthRequest = width * 0.8;
        emailBorder.WidthRequest = width * 0.8;
        emailBorder.Margin = new Thickness() { Bottom = height * 0.02 };

        stack.WidthRequest = width * 0.8;
    }


    public async void ForgetPassword(object sender, EventArgs args)
    {
        await IsLoading.RunMethod(() => ViewModel.ForgetPassword(email));
    }
}