using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class RegisterAccountView : ContentPage
{
	public RegisterAccountViewModel ViewModel { get; }

	public RegisterAccountView()
	{
		InitializeComponent();
		SetControlsProperties();

		ViewModel = new RegisterAccountViewModel();
	}


	public void SetControlsProperties()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        var height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

        email.WidthRequest = width * 0.8;
        emailBorder.WidthRequest = width * 0.8;
        emailBorder.Margin = new Thickness() { Bottom = height * 0.02 };

		password.WidthRequest = width * 0.8;
        passwordBorder.WidthRequest = width * 0.8;
        passwordBorder.Margin = new Thickness() { Bottom = height * 0.02 };

		confirmPassword.WidthRequest = width * 0.8;
        confirmPasswordBorder.WidthRequest = width * 0.8;
        confirmPasswordBorder.Margin = new Thickness() { Bottom = height * 0.02 };

        stack.WidthRequest = width * 0.8;
    }

	public async void RegisterAccount(object sender, EventArgs args)
	{
		await IsLoading.RunMethod(() => ViewModel.RegisterAccount(email, password, confirmPassword));
	}
}