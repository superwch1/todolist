using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class ForgetPasswordView : ContentPage
{
    public ForgetPasswordViewModel ViewModel { get; }

	public ForgetPasswordView()
	{
		InitializeComponent();
        ViewModel = new ForgetPasswordViewModel();  
	}


    public async void ForgetPassword(object sender, EventArgs args)
    {
        await IsLoading.RunMethod(() => ViewModel.ForgetPassword(email));
    }
}