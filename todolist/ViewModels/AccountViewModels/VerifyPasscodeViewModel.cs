using System.Net;
using CommunityToolkit.Maui.Core.Platform;

namespace todolist.ViewModels.AccountViewModels
{
	public class VerifyPasscodeViewModel
	{
        public string Email { get; }

        public VerifyPasscodeViewModel(string email)
        {
            Email = email;
        }

        public async Task VerifyPasscode(string passcode)
        {
            var registerResponse = await WebServer.VerifyPasscode(Email, passcode);           
            if (registerResponse.Item2 == HttpStatusCode.ExpectationFailed)
            {
                await ToastBar.DisplayToast("Cannot connect to server");
            }

            if (registerResponse.Item2 == HttpStatusCode.BadRequest)
            {
                await ToastBar.DisplayToast(registerResponse.Item1);
            }

            if (registerResponse.Item2 == HttpStatusCode.OK)
            {
                string resetToken = registerResponse.Item1;
                
                await Shell.Current.GoToAsync("resetpassword",
                    new Dictionary<string, object>{
                        { "email", Email },
                        { "resetToken", resetToken }
                    });
            }
        }


        public async Task ConfirmPasscode(Entry[] entries)
        {
            if (!String.IsNullOrEmpty(entries[0].Text) && !String.IsNullOrEmpty(entries[1].Text) && !String.IsNullOrEmpty(entries[2].Text) &&
                !String.IsNullOrEmpty(entries[3].Text) && !String.IsNullOrEmpty(entries[4].Text) && !String.IsNullOrEmpty(entries[5].Text))
            {           
                string passcode = entries[0].Text + entries[1].Text + entries[2].Text + entries[3].Text + 
                    entries[4].Text + entries[5].Text;

                if (passcode.Length == 6)
                {
                    await entries[0].HideKeyboardAsync();
                    await entries[1].HideKeyboardAsync();
                    await entries[2].HideKeyboardAsync();
                    await entries[3].HideKeyboardAsync();
                    await entries[4].HideKeyboardAsync();
                    await entries[5].HideKeyboardAsync();

                    await ToastBar.DisplayToast("Verifying Passcode");

                    await VerifyPasscode(passcode);
                }
            }
        }


        public async Task EntryTextChanged(object sender, Entry[] entries)
        {
            Entry currentEntry = sender as Entry;
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
        }
    }
}

