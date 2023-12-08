using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class LoginView : ContentPage
{
	public LoginViewModel ViewModel { get; }   
    public double DisplayHeight { get; set; }
    public double DisplayWidth { get; set; }
    public double IconWidth { get; set; }
    public bool AnimationStarted { get; set; } = false;
    public bool AnimationFinished { get; set; } = false;
    public bool IconPressed { get; set; } = true;
    public double TopYPosition { get; set; }
    public double MiddleYPosition { get; set; }
    public double DistanceBetweenIconAndStack { get; } = 10;


	public LoginView()
	{
        InitializeComponent();
        SetControlsProperties();

        NavigationPage.SetHasNavigationBar(this, false);

        ViewModel = new LoginViewModel();
        Shell.SetNavBarIsVisible(this, false); 
    }


    public void SetControlsProperties()
    {
        var width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

        absoluteLayout.MaximumWidthRequest = width * 0.8;

        email.WidthRequest = width * 0.8 - 40; //for padding
        emailBorder.WidthRequest = width * 0.8;
        emailBorder.Margin = new Thickness() { Bottom = 20 };

        password.WidthRequest = width * 0.8 - 40; //for padding
        passwordBorder.WidthRequest = width * 0.8;
        passwordBorder.Margin = new Thickness() { Bottom = 20 };

        loginButton.WidthRequest = width * 0.6;


        IconWidth = width * 0.8;
        iconWithShadow.WidthRequest = IconWidth;
        iconWithoutShadow.WidthRequest = IconWidth;
        shadow.WidthRequest = IconWidth * 0.913;

        clickLabel.Margin = new Thickness() { Bottom = IconWidth + 100 };

        //input stack needs to be visible in order to get its height
        inputStack.Opacity = 0; //even though the user can't see the button, it is clickable
        inputStack.Margin = new Thickness() { Left = 3000 };

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


    async void ClickIcon(object sender, TappedEventArgs args)
    {
        if (AnimationFinished == false)
        {
            return;
        }
        await IsLoading.RunMethod(async() => {
            if (IconPressed == true)
            {
                await iconWithoutShadow.TranslateTo(0, TopYPosition - MiddleYPosition, 350);
                IconPressed = false;
            }
            else 
            {
                await iconWithoutShadow.TranslateTo(0, TopYPosition - MiddleYPosition + IconWidth * 0.1, 350);
                IconPressed = true;
            }
        });
    }


    async void StartAnimation(object sender, TappedEventArgs args)
    {
        if (AnimationStarted == true)
        {
            return;
        }
        AnimationStarted = true;

        clickLabel.IsVisible = false;
        MiddleYPosition = DisplayHeight / 2 - IconWidth * 1.1159 / 2; 
        var heightDistance = IconWidth * 0.1;

        iconWithShadow.IsVisible = false;

        absoluteLayout.VerticalOptions = LayoutOptions.Start;
        iconWithoutShadow.Margin = new Thickness() { Top = MiddleYPosition };
        shadow.Margin = new Thickness() { Top = MiddleYPosition + IconWidth * 0.18 };
        
        iconWithoutShadow.IsVisible = true;
        shadow.IsVisible = true;

        await iconWithoutShadow.TranslateTo(0, IconWidth * 0.1, 350);

        await Task.Delay(200);

        TopYPosition = (DisplayHeight - IconWidth * 1.1159 - inputStack.Height - heightDistance - DistanceBetweenIconAndStack) / 2;

        inputStack.Margin = new Thickness() { Top = TopYPosition + IconWidth * 1.1159 + DistanceBetweenIconAndStack }; 

        var task1 = iconWithoutShadow.TranslateTo(0, TopYPosition - MiddleYPosition, 550);
        var task2 = shadow.TranslateTo(0, TopYPosition - MiddleYPosition, 550);
        var task3 = inputStack.FadeTo(1, 550);

        await Task.WhenAll(task1, task2, task3); 

        await Task.Delay(200);

        var task4 = iconWithoutShadow.TranslateTo(0, TopYPosition - MiddleYPosition + IconWidth * 0.1, 350);
        var task5 = inputStack.TranslateTo(0, IconWidth * 0.1, 350);

        await Task.WhenAll(task4, task5);

        //position of keyboard will not be auto adjusted after the Translation in android 
        //registeraccount button no reponse after translation in IOS
        await inputStack.TranslateTo(0, 0, 0);
        inputStack.Margin = new Thickness() { Top = TopYPosition + IconWidth * 1.1159 + DistanceBetweenIconAndStack + IconWidth * 0.1 }; 
        AnimationFinished  = true;
    }

    async void Login(object sender, EventArgs e)
    {
        await IsLoading.RunMethod(() => ViewModel.Login(email, password));
    }

    async void RegisterAccount(object sender, EventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync("registeraccount"));
    }

    async void ForgetPassword(object sender, EventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync("forgetpassword"));
    }
}
