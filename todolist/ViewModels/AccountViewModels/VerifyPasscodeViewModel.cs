using System.Net;
using System.Text.RegularExpressions;
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


        public async Task ConfirmPasscode(Entry invisibleEntry)
        {
            if (!String.IsNullOrEmpty(invisibleEntry.Text) && invisibleEntry.Text.Length == 6)
            {           
                await invisibleEntry.HideKeyboardAsync();

                await ToastBar.DisplayToast("Verifying Passcode");
                await VerifyPasscode(invisibleEntry.Text);
            }
        }


        public async Task InvisibleEntryTextChanged(object sender, Label[] lablels, TextChangedEventArgs args)
        {
            var invisibleEntry = sender as Entry;
            if(invisibleEntry.Text == null)
            {
                return;
            }

            const string pattern = @"^\d*$";
            if (!Regex.IsMatch(args.NewTextValue, pattern))
            {
                invisibleEntry.Text = args.OldTextValue;
            }

            for (var i = 0; i < 6; i++)
            {
                if (invisibleEntry.Text.Length > i)
                {
                    lablels[i].Text = invisibleEntry.Text[i].ToString();
                }
                else 
                {
                    lablels[i].Text = "";
                }
            }

            if (invisibleEntry.Text.Length == 6)
            {           
                await invisibleEntry.HideKeyboardAsync();
                invisibleEntry.Text = "";

                await ToastBar.DisplayToast("Verifying Passcode");
                await VerifyPasscode(invisibleEntry.Text);
            }
        }
    }
}

