using System.Net;
using System.Text.RegularExpressions;

namespace todolist.ViewModels
{
	public class ForgetPasswordViewModel
	{
        public INavigation Navigation { get; }

        public ForgetPasswordViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async Task ForgetPassword(Entry email)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            if (email.Text == null || !Regex.IsMatch(email.Text, pattern))
            {
                await ToastBar.DisplayToast("Please enter email");
                return;
            }

            var registerResponse = await WebServer.ForgetPassword(email.Text);          
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
                await Navigation.PushAsync(new VerifyPasscodeView(email.Text));
            }
        }
    }
}
