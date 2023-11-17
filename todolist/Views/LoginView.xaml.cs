namespace todolist.Views;

public partial class LoginView : ContentPage
{
	LoginViewModel _viewModel;
    AccountDatabase _accountDatabase;

	public LoginView(AccountDatabase accountDatabase)
	{
        InitializeComponent();
        SetControlsProperties();

        _viewModel = new LoginViewModel(accountDatabase);
        _accountDatabase = accountDatabase;
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
    }

    async void submitButton_Clicked(object sender, EventArgs e)
    {
        if (email.Text != null && password.Text != null)
		{
			await _viewModel.Login(email.Text, password.Text);
        }
    }
}
