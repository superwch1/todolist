using System.Net;
using Microsoft.AspNetCore.SignalR.Client;

namespace todolist.ViewModels
{
	public class LoginViewModel
	{
        public async Task Login(Entry email, Entry password)
        {
            if (email.Text == null){
                await ToastBar.DisplayToast("Please enter email");
                return;
            }
            if (password.Text == null){
                await ToastBar.DisplayToast("Please enter password");
                return;
            }

            var loginResponse = await WebServer.Login(email.Text, password.Text);
            if (loginResponse.Item2 == HttpStatusCode.ExpectationFailed)
            {
                await ToastBar.DisplayToast("Cannot connect to server");
                return;
            }

            if (loginResponse.Item2 == HttpStatusCode.BadRequest)
            {
                await ToastBar.DisplayToast(loginResponse.Item1);
                return;
            }

            if (loginResponse.Item2 == HttpStatusCode.OK)
            {
                await ToastBar.DisplayToast("Logging in");
                await UserDatabase.UpdateItemAsync(new UserModel() { Id = 1, JwtToken = loginResponse.Item1 });

                HubConnection? connection = await SignalR.BuildHubConnection(loginResponse.Item1);
                var taskReponse = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, loginResponse.Item1);     

                if (taskReponse.Item2 == HttpStatusCode.OK && connection != null)
                {
                    Application.Current!.MainPage = new TaskShell(taskReponse.Item1, loginResponse.Item1, connection);
                }
            }
        }
    }
}

