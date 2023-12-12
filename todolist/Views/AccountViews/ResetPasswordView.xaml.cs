using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class ResetPasswordView : ContentPage, IQueryAttributable
{
	public ResetPasswordViewModel ViewModel { get; set; }

	public ResetPasswordView()
	{
		InitializeComponent();
		Shell.SetTabBarIsVisible(this, false);
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		string email = query["email"] as string;	
		string resetToken = query["resetToken"] as string;
		ViewModel = new ResetPasswordViewModel(resetToken, email);	
    }

	public async void ResetPassword(object sender, EventArgs args)
	{
		await IsLoading.RunMethod(() => ViewModel.ResetPassword(password, confirmPassword));	
	}

	public void PasswordTextChanged(object sender, TextChangedEventArgs e)
	{
		if (e.NewTextValue.Length >= 6 && e.NewTextValue.Length <= 20)
		{
			passwordIcon.Source = "tick";
		}
		else
		{
			passwordIcon.Source = "cross";
		}

		if (e.NewTextValue == confirmPassword.Text)
		{
			confirmPasswordIcon.Source = "tick";
		}
		else
		{
			confirmPasswordIcon.Source = "cross";
		}
	}

	public void ConfirmPasswordTextChanged(object sender, TextChangedEventArgs e)
	{
		if (e.NewTextValue == password.Text)
		{
			confirmPasswordIcon.Source = "tick";
		}
		else
		{
			confirmPasswordIcon.Source = "cross";
		}
	}


	async void GoBack(object sender, TappedEventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync(".."));
    }
}