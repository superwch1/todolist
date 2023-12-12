using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class ResetPasswordView : ContentPage, IQueryAttributable
{
	public ResetPasswordViewModel ViewModel { get; set; }

	public ResetPasswordView()
	{
		InitializeComponent();

		Shell.SetNavBarIsVisible(this, false);
	}


	protected override async void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		Shell.SetTabBarIsVisible(this, false);
	}


    //workaround for the add button hidden under tab after navigate back from serach view in IOS
	//it happens when you expand all of the task in search view then go backwards
	protected override void OnDisappearing()
	{
		base.OnDisappearing();

#if IOS
		Shell.SetTabBarIsVisible(this, true);
#endif
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