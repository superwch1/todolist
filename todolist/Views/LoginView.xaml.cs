namespace todolist.Views;

public partial class LoginView : ContentPage
{
	public LoginViewModel ViewModel { get; }

	public LoginView()
	{
        InitializeComponent();
        SetControlsProperties();

        NavigationPage.SetHasNavigationBar(this, false);

        ViewModel = new LoginViewModel();
        password.ReturnCommand = new Command(() => Login(null, null));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        icon.RotateTo(360, 2000);
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

        button.WidthRequest = width * 0.8;

        icon.WidthRequest = height * 0.4;
        icon.Margin = new Thickness() { Bottom = height * 0.05 };

        stack.WidthRequest = width * 0.8;
    }

    async void Login(object sender, EventArgs e)
    {
        await IsLoading.RunMethod(() => ViewModel.Login(email, password));
    }

    async void RegisterAccount(object sender, TappedEventArgs args)
    {
        await Navigation.PushAsync(new RegisterAccountView());
    }


    async void ForgetPassword(object sender, TappedEventArgs args)
    {
        await Navigation.PushAsync(new ForgetPasswordView());
    }
}
