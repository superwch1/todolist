namespace todolist.Views;

public partial class LoginView : ContentPage
{
	LoginViewModel _viewModel;

	public LoginView(AccountDatabase accountDatabase)
	{
        InitializeComponent();
        SetControlsProperties();

        _viewModel = new LoginViewModel(accountDatabase);
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

        if (email.Text == null){
            await ToastBar.DisplayToast("Please enter email");
        }
        else if (password.Text == null){
            await ToastBar.DisplayToast("Please enter password");
        }
        else if (email.Text != null && password.Text != null)
		{
			await _viewModel.Login(email.Text, password.Text);
        }
    }
}
