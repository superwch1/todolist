using System.Net;

namespace todolist.ViewModels
{
	public class VerifyPasscodeViewModel
	{
        public INavigation Navigation { get; }
        public string Email { get; }

        public VerifyPasscodeViewModel(INavigation navigation, string email)
        {
            Navigation = navigation;
            Email = email;
        }

        public async Task VerifyPasscode(Entry passcode)
        {
            if (passcode.Text == null || passcode.Text.Length != 6)
            {
                await ToastBar.DisplayToast("Please enter passcode");
                return;
            }

            var registerResponse = await WebServer.VerifyPasscode(Email, passcode.Text);           
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
                await Navigation.PushAsync(new ResetPasswordView(Email, resetToken));
            }
        }
    }
}

