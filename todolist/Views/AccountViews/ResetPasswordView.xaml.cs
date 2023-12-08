using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class ResetPasswordView : ContentPage, IQueryAttributable
{
	public ResetPasswordViewModel ViewModel { get; set; }

	public ResetPasswordView()
	{
		InitializeComponent();
		SetControlsProperties();	

		Shell.SetTabBarIsVisible(this, false);
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		string email = query["email"] as string;	
		string resetToken = query["resetToken"] as string;
		ViewModel = new ResetPasswordViewModel(resetToken, email);	
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