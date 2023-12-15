using System.Net;

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
    }
}

