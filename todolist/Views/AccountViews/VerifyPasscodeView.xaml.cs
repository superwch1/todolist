using CommunityToolkit.Maui.Core.Platform;

namespace todolist.Views.AccountViews;

public partial class VerifyPasscodeView : ContentPage, IQueryAttributable
{
	public VerifyPasscodeViewModel ViewModel { get; set; }

	public VerifyPasscodeView()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
	}


	public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		string email = query["email"] as string;	
		ViewModel = new VerifyPasscodeViewModel(email);
    }


	public async void InvisibleEntryTextChanged(object sender, TextChangedEventArgs args)
	{
		var lablels = new Label[] { firstDigit, secondDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit };
		await ViewModel.InvisibleEntryTextChanged(sender, lablels, args);
	}


	public async void EnterPasscode(object sender, TappedEventArgs args)
	{
		invisibleEntry.Focus();
		await invisibleEntry.ShowKeyboardAsync();
	}


	public async void GoBack(object sender, TappedEventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync(".."));
    }
}