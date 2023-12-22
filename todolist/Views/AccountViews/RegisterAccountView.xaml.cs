namespace todolist.Views.AccountViews;

public partial class RegisterAccountView : ContentPage
{
	public RegisterAccountViewModel ViewModel { get; }

	public RegisterAccountView()
	{
		InitializeComponent();
		ViewModel = new RegisterAccountViewModel();

		Shell.SetNavBarIsVisible(this, false);
	}


	public async void RegisterAccount(object sender, EventArgs args)
	{
		await IsLoading.RunMethod(() => ViewModel.RegisterAccount(email, password, confirmPassword));
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

	public async void GoBack(object sender, TappedEventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync(".."));
    }

	public async void ReadPrivacyPolicy(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ReadPrivacyPolicy());
	}

	public async void ReadTermsAndConditions(object sender, TappedEventArgs args)
	{	
		await IsLoading.RunMethod(() => ViewModel.ReadTermsAndConditions());
	}
}