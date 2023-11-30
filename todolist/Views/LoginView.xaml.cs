using CommunityToolkit.Maui.Core.Platform;

namespace todolist.Views;

public partial class LoginView : ContentPage
{
	public LoginViewModel ViewModel { get; }   
    public double DisplayHeight { get; set; }
    public double DisplayWidth { get; set; }
    public double IconWidth { get; set; }
    public bool AnimationStarted { get; set; } = false;
    public bool AnimationFinished { get; set; } = false;
    public bool IconPressed { get; set; } = true;


	public LoginView()
	{
        InitializeComponent();
        SetControlsProperties();

        NavigationPage.SetHasNavigationBar(this, false);

        ViewModel = new LoginViewModel();

        email.ReturnCommand = new Command(() => password.Focus());
        password.ReturnCommand = new Command(() => Login(null, null));
        password.ReturnType = ReturnType.Go;
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

        loginButton.WidthRequest = width * 0.8;

        
        //clickLabel.WidthRequest = width * 0.8;

        IconWidth = width * 0.8;
        iconWithShadow.WidthRequest = IconWidth;
        iconWithoutShadow.WidthRequest = IconWidth;
        shadow.WidthRequest = IconWidth * 0.913;

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


    async void ClickIcon(object sender, TappedEventArgs args)
    {
        if (AnimationFinished == false)
        {
            return;
        }
        await IsLoading.RunMethod(async() => {
            if (IconPressed == true)
            {
                await iconWithoutShadow.TranslateTo(0, 0, 350);
                IconPressed = false;
            }
            else 
            {
                await iconWithoutShadow.TranslateTo(0, IconWidth * 0.1, 350);
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

        //clickLabel.IsVisible = false;
        var centerYPosition = DisplayHeight / 2 - IconWidth * 1.1159 / 2; 
        var heightDistance = IconWidth * 0.1;

        iconWithShadow.IsVisible = false;

        absoluteLayout.VerticalOptions = LayoutOptions.Start;
        iconWithoutShadow.Margin = new Thickness() { Top = centerYPosition };
        shadow.Margin = new Thickness() { Top = centerYPosition + IconWidth * 0.18 };
        
        iconWithoutShadow.IsVisible = true;
        shadow.IsVisible = true;

        await iconWithoutShadow.TranslateTo(0, IconWidth * 0.1, 350);

        await Task.Delay(200);

        await iconWithoutShadow.TranslateTo(0, 0, 350);

        await Task.Delay(200);
        
        var topYPosition = (DisplayHeight - IconWidth * 1.1159 - inputStack.Height - heightDistance - 30) / 2;

        iconWithoutShadow.Margin = new Thickness() { Top = topYPosition };
        shadow.Margin = new Thickness() { Top = topYPosition + IconWidth * 0.18 };
        inputStack.Margin = new Thickness() { Top = topYPosition + IconWidth * 1.1159 + 30 };  

        await Task.Delay(200);

        var task1 = iconWithoutShadow.TranslateTo(0, IconWidth * 0.1, 350);
        var task2 = inputStack.TranslateTo(0, IconWidth * 0.1, 350);

        await Task.WhenAll(task1, task2);


        //position of keyboard will not be auto adjusted after the Translation in android 
        //registeraccount button no reponse after translation in IOS
        await inputStack.TranslateTo(0, 0, 0);
        inputStack.Margin = new Thickness() { Top = topYPosition + IconWidth * 1.1159 + 30 + IconWidth * 0.1 }; 
        AnimationFinished  = true;
    }

    async void Login(object sender, EventArgs e)
    {
        await IsLoading.RunMethod(() => ViewModel.Login(email, password));
    }

    async void RegisterAccount(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new ForgetPasswordView());
    }

    async void ForgetPassword(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new ForgetPasswordView());
    }
}
