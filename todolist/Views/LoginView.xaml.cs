namespace todolist.Views;

public partial class LoginView : ContentPage
{
	public LoginViewModel ViewModel { get; }
    public bool LoggingIn { get; set; } = false;

	public LoginView()
	{
        InitializeComponent();
        SetControlsProperties();

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
    }

    async void Login(object sender, EventArgs e)
    {
        if (LoggingIn == false)
        {
            LoggingIn = true;
            await ViewModel.Login(email, password);
        }
        LoggingIn = false;
    }
}
