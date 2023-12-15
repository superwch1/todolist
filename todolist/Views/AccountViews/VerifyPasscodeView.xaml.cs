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


	public async void VerifyPasscode(object sender, EventArgs args)
	{
		
	}

	public async void EntryTextChanged(object sender, TextChangedEventArgs args)
	{
		Entry currentEntry = sender as Entry;
		var entries = new Entry[] { firstDigit, secondDigit, thirdDigit, fourthDigit, fifthDigit, sixthDigit };
		int currentIndex = Array.IndexOf(entries, currentEntry);

		//move to next entry
		if (currentEntry.Text.Length == 2)
		{
			if (currentIndex < 5) //0, 1, 2, 3, 4
			{
				currentEntry.Text = currentEntry.Text.Remove(currentEntry.Text.Length - 1, 1);
				Entry nextEntry = entries[currentIndex + 1];
				nextEntry.Text = currentEntry.Text[currentEntry.Text.Length - 1].ToString();
				nextEntry.Focus();
			}
			else if (currentIndex == 5)
			{
				currentEntry.Text = currentEntry.Text.Remove(currentEntry.Text.Length - 1, 1);
			}
		}

		//return to previous entry
		else if (currentEntry.Text.Length == 0)
		{
			if (currentIndex == 0)
			{
				Entry previousEntry = entries[0];
				previousEntry.Focus();
			}
			else 
			{
				Entry previousEntry = entries[currentIndex - 1];
				previousEntry.Focus();
			}
		}


		var passcode = "";
		foreach(var entry in entries)
		{
			if (entry != null && !String.IsNullOrEmpty(entry.Text))
			{
				passcode += entry.Text;
			}
		}
		
		if (passcode.Length == 6 && sixthDigit.IsFocused == true)
		{
			await IsLoading.RunMethod(() => ViewModel.VerifyPasscode(passcode));
		}
	}


	async void GoBack(object sender, TappedEventArgs args)
    {
        await IsLoading.RunMethod(() => Shell.Current.GoToAsync(".."));
    }
}