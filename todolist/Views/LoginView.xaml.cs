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
        clickLabel.WidthRequest = width * 0.8;

        firstButtonWithShadow.WidthRequest = width * 0.8;
        firstButtonWithoutShadow.WidthRequest = width * 0.8;
        secondButtonWithShadow.WidthRequest = width * 0.8;
        secondButtonWithoutShadow.WidthRequest = width * 0.8;

        //input stack needs to be visible in order to get its height
        AbsoluteLayout.SetLayoutBounds(inputStack, new Rect(0, -1000, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)); 
        AbsoluteLayout.SetLayoutBounds(clickLabel, new Rect(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));  

        AbsoluteLayout.SetLayoutBounds(firstButtonWithShadow, new Rect(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)); 
        AbsoluteLayout.SetLayoutBounds(firstButtonWithoutShadow, new Rect(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)); 
        AbsoluteLayout.SetLayoutBounds(secondButtonWithShadow, new Rect(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)); 
        AbsoluteLayout.SetLayoutBounds(secondButtonWithoutShadow, new Rect(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize)); 

        //it needs to be true otherwise the option will be placed down a little bit in IOS
        absoluteLayout.IgnoreSafeArea = true;
    }

    //height obtained from DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density
    //is different with the height of page in android (no difference in width)
    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        
        DisplayHeight = height;
        DisplayWidth = width;

        var firstButtonWithShadowY = height / 2 - (width * 0.8) * 1.1159 / 2; 
        await firstButtonWithShadow.TranslateTo(0, firstButtonWithShadowY, 0);
        await clickLabel.TranslateTo(0, firstButtonWithShadowY - 50, 0);
    }


    async void StartAnimation(object sender, TappedEventArgs args)
    {
        if (AnimationStarted == true)
        {
            return;
        }
        AnimationStarted = true;

        clickLabel.IsVisible = false;

        var firstbuttonWithShadowY = DisplayHeight / 2 - (DisplayWidth * 0.8) * 1.1159 / 2; 
        var heightDistance = firstButtonWithShadow.Height * (1 - 0.8961);
        firstButtonWithShadow.IsVisible = false;

        await firstButtonWithoutShadow.TranslateTo(0, firstbuttonWithShadowY + heightDistance, 0);
        firstButtonWithoutShadow.IsVisible = true;

        await Task.Delay(750);

        firstButtonWithoutShadow.IsVisible = false;
        firstButtonWithShadow.IsVisible = true;

        await Task.Delay(750);

        firstButtonWithShadow.IsVisible = false;

        var secondButtonWithShadowY = (DisplayHeight - (DisplayWidth * 0.8) * 1.1159 - inputStack.Height - 40) / 2;
        await secondButtonWithShadow.TranslateTo(0, secondButtonWithShadowY, 0);
        secondButtonWithShadow.IsVisible = true;

        await inputStack.TranslateTo(0, 1000 + secondButtonWithShadowY + (DisplayWidth * 0.8) * 1.1159 + 40, 0);
        //inputackt is always visible for getting its height but place at -1,000 in y-axis    

        await Task.Delay(750);

        secondButtonWithShadow.IsVisible = false;

        await secondButtonWithoutShadow.TranslateTo(0, secondButtonWithShadowY + heightDistance, 0);
        secondButtonWithoutShadow.IsVisible = true;
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

    public async void EntryFocused(object sender, FocusEventArgs args)
	{
	}


	public async void EntryUnfocused(object sender, FocusEventArgs args)
	{
	}
}
