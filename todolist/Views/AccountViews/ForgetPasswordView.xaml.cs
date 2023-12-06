namespace todolist.Views;

public partial class ForgetPasswordView : ContentPage
{
    public ForgetPasswordViewModel ViewModel { get; }

	public ForgetPasswordView()
	{
		InitializeComponent();
        ViewModel = new ForgetPasswordViewModel(Navigation);  
	}


    public async void ForgetPassword(object sender, EventArgs args)
    {
        await IsLoading.RunMethod(() => ViewModel.ForgetPassword(email));
    }
}