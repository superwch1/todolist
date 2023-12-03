using System.Net;

namespace todolist.ViewModels
{
	public class ResetPasswordViewModel
	{
        public string ResetToken { get; }
        public string Email { get; }
        public INavigation Navigation { get; }

        public ResetPasswordViewModel(string resetToken, string email, INavigation navigation)
        {
            ResetToken = resetToken;
            Email = email;
            Navigation = navigation;
        }

        public async Task ResetPassword(Entry password, Entry confirmPassword)
        {
            if (password.Text == null || confirmPassword.Text == null)
            {
                await ToastBar.DisplayToast("Please enter password");
                return;
            }

            if (password.Text.Length < 6 || password.Text.Length > 20)
            {
                await ToastBar.DisplayToast("Password length is between 6 - 20 characters");
                return;
            }

            if (password.Text != confirmPassword.Text)
            {
                await ToastBar.DisplayToast("Password does not match");
            }

            var registerResponse = await WebServer.ResetPassword(new ResetPasswordModel() 
                { Email = Email, Password = password.Text, ConfirmPassword = confirmPassword.Text,
                  ResetToken = ResetToken});
            
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
                await ToastBar.DisplayToast("Password Updated");
                await Navigation.PopToRootAsync();
            }
        }
    }
}

