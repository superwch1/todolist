using CommunityToolkit.Maui.Core.Platform;

namespace todolist.Views;

public partial class LoginView : ContentPage
{
	public LoginViewModel ViewModel { get; }
    public bool AnimationStarted { get; set; } = false;
    public double DisplayHeight { get; set; }
    public double DisplayWidth { get; set; }


	public LoginView()
	{
        InitializeComponent();
        SetControlsProperties();

        NavigationPage.SetHasNavigationBar(this, false);

        ViewModel = new LoginViewModel();
        password.ReturnCommand = new Command(() => Login(null, null));
    }


    public void SetControlsProperties()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

        absoluteLayout.MaximumWidthRequest = width * 0.8;

        email.WidthRequest = width * 0.8;
        emailBorder.WidthRequest = width * 0.8;
        emailBorder.Margin = new Thickness() { Bottom = 20 };

        password.WidthRequest = width * 0.8;
        passwordBorder.WidthRequest = width * 0.8;
        passwordBorder.Margin = new Thickness() { Bottom = 20 };

        button.WidthRequest = width * 0.8;
        //clickLabel.WidthRequest = width * 0.8;

        buttonWithShadow.WidthRequest = width * 0.8;
        buttonWithoutShadow.WidthRequest = width * 0.8;

        //input stack needs to be visible in order to get its height
        inputStack.Margin = new Thickness() { Left = 4000 };

        //it needs to be true otherwise the option will be placed down a little bit in IOS
        absoluteLayout.IgnoreSafeArea = true;
    }


    //height obtained from DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density
    //is different with the height of page in android (no difference in width)
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        
        DisplayHeight = height;
        DisplayWidth = width;
    }


    async void StartAnimation(object sender, TappedEventArgs args)
    {
        if (AnimationStarted == true)
        {
            return;
        }
        AnimationStarted = true;

        //clickLabel.IsVisible = false;
        var firstbuttonWithShadowY = DisplayHeight / 2 - (DisplayWidth * 0.8) * 1.1159 / 2; 
        var heightDistance = buttonInCenter.Height * (1 - 0.8961);

        buttonInCenter.IsVisible = false;
        buttonWithShadow.IsVisible = false;

        absoluteLayout.VerticalOptions = LayoutOptions.Start;

        buttonWithoutShadow.Margin = new Thickness() { Top = firstbuttonWithShadowY + heightDistance };
        buttonWithoutShadow.IsVisible = true;

        await Task.Delay(400);

        buttonWithoutShadow.IsVisible = false;

        buttonWithShadow.Margin = new Thickness() { Top = firstbuttonWithShadowY };
        buttonWithShadow.IsVisible = true;

        await Task.Delay(400);

        buttonWithShadow.IsVisible = false;

        var secondButtonWithShadowY = (DisplayHeight - (DisplayWidth * 0.8) * 1.1159 - inputStack.Height - heightDistance - 30) / 2;
        buttonWithShadow.Margin = new Thickness() { Top = secondButtonWithShadowY };
        buttonWithShadow.IsVisible = true;

        inputStack.Margin = new Thickness() { Top = secondButtonWithShadowY + (DisplayWidth * 0.8) * 1.1159 + 30 };  

        await Task.Delay(750);

        buttonWithShadow.IsVisible = false;

        buttonWithoutShadow.Margin = new Thickness() { Top = secondButtonWithShadowY + heightDistance };
        inputStack.Margin = new Thickness() { Top = secondButtonWithShadowY + (DisplayWidth * 0.8) * 1.1159 + heightDistance + 30 };
        buttonWithoutShadow.IsVisible = true;
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
