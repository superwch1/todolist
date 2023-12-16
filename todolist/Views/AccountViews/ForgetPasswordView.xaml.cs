using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Platform;
using todolist.ViewModels.AccountViewModels;

namespace todolist.Views.AccountViews;

public partial class ForgetPasswordView : ContentPage
{
    public ForgetPasswordViewModel ViewModel { get; }

	public ForgetPasswordView()
	{
		InitializeComponent();
        ViewModel = new ForgetPasswordViewModel();  

        Shell.SetNavBarIsVisible(this, false);
	}


    public async void ForgetPassword(object sender, EventArgs args)
    {
        await email.HideKeyboardAsync();
        await IsLoading.RunMethod(() => ViewModel.ForgetPassword(email));
    }

    async void GoBack(object sender, TappedEventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync(".."));
    }
}