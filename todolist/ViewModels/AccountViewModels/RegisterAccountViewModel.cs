using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR.Client;
using todolist.Views.TaskViews;

namespace todolist.ViewModels
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
    }
}

