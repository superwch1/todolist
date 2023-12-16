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


	public async void EntryTextChanged(object sender, TextChangedEventArgs args)
	{
		var entries = new Entry[] { firstDigit, secondDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit };
		await IsLoading.RunMethod(() => ViewModel.EntryTextChanged(sender, entries));
	}


	public async void ConfirmPasscode(object sender, EventArgs args)
	{
		var entries = new Entry[] { firstDigit, secondDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit };
		await IsLoading.RunMethod(() => ViewModel.ConfirmPasscode(entries));
	}


	async void GoBack(object sender, TappedEventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync(".."));
    }



/*
	public void EntryFocused(object sender, FocusEventArgs args)
	{
		var entries = new Entry[] { firstDigit, secondDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit };
		foreach (var entry in entries)
		{
			if (String.IsNullOrEmpty(entry.Text))
			{
				entry.Focus();
				return;
			}
		}
		sixthDigit.Focus();
	}
*/
}