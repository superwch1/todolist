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
			await ToastBar.DisplayToast("Logging in");
			await UserDatabase.UpdateItemAsync(new UserModel() { Id = 1, JwtToken = registerResponse.Item1 });

			HubConnection? connection = await SignalR.BuildHubConnection(registerResponse.Item1);
			var taskReponse = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, registerResponse.Item1);     

			if (taskReponse.Item2 == HttpStatusCode.OK && connection != null)
			{
				Application.Current!.MainPage = new HomeView(taskReponse.Item1, registerResponse.Item1, connection);
			}
		}
        }
    }
}

