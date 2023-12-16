using System.Net;
using System.Text.RegularExpressions;
using CommunityToolkit.Maui.Alerts;

namespace todolist.ViewModels.AccountViewModels
{
	public class RegisterAccountViewModel
	{
        public async Task RegisterAccount(Entry email, Entry password, Entry confirmPassword)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
			if (email.Text == null || !Regex.IsMatch(email.Text, pattern))
			{
				await ToastBar.DisplayToast("Please enter email");
				return;
			}

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

			await ToastBar.DisplayToast("Doing the paper work...");

			var registerResponse = await WebServer.RegisterAccount(new AccountModel() 
				{ Email = email.Text, Password = password.Text, ConfirmPassword = confirmPassword.Text});
			
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
				await ToastBar.DisplayToast("Account created");
				await Shell.Current.Navigation.PopToRootAsync();
			}
        }


		public async Task ReadPrivacyPolicy()
		{
			var taskReponse = await WebServer.ReadPrivacyPolicy();
            if (taskReponse.Item2 != HttpStatusCode.OK)
            {
                await ToastBar.DisplayToast("Cannot connect to server");
                return;
            }

			string policyContent = taskReponse.Item1;

			try 
			{
				await Shell.Current.GoToAsync("policy",
                    new Dictionary<string, object>{
                        { "policyContent", policyContent },
                        { "policyType", "Privacy Policy" }
                    });
			}
			catch { }
		}


		public async Task ReadTermsAndConditions()
		{
			var taskReponse = await WebServer.ReadTermsAndConditions();
            if (taskReponse.Item2 != HttpStatusCode.OK)
            {
                await ToastBar.DisplayToast("Cannot connect to server");
                return;
            }

			string policyContent = taskReponse.Item1;

			try 
			{
				await Shell.Current.GoToAsync("policy",
                    new Dictionary<string, object>{
                        { "policyContent", policyContent },
                        { "policyType", "Terms And Conditions" }
                    });
			}
			catch { }
		}
    }
}

