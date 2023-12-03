namespace todolist.Views;

public partial class ResetPasswordView : ContentPage
{
	public ResetPasswordViewModel ViewModel { get; }

	public ResetPasswordView(string email, string resetToken)
	{
		InitializeComponent();
		SetControlsProperties();

		ViewModel = new ResetPasswordViewModel(resetToken, email, Navigation);	
	}


	public void SetControlsProperties()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

		password.WidthRequest = width * 0.8;
        passwordBorder.WidthRequest = width * 0.8;
        passwordBorder.Margin = new Thickness() { Bottom = height * 0.02 };

		confirmPassword.WidthRequest = width * 0.8;
        confirmPasswordBorder.WidthRequest = width * 0.8;
        confirmPasswordBorder.Margin = new Thickness() { Bottom = height * 0.02 };

        stack.WidthRequest = width * 0.8;
    }


	public async void ResetPassword(object sender, EventArgs args)
	{
		await IsLoading.RunMethod(() => ViewModel.ResetPassword(password, confirmPassword));	
	}
}