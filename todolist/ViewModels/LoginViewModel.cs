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

            var response = await WebServer.Login(email.Text, password.Text);
            if (response == null)
            {
                await ToastBar.DisplayToast("Cannot connect to server");
                return;
            }

            if (response.Item2 == HttpStatusCode.OK)
            {
                await ToastBar.DisplayToast("Logging in");
                await AccountDatabase.UpdateItemAsync(new AccountModel() { Id = 1, JwtToken = response.Item1 });
                var tasks = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, response.Item1);

                if (tasks != null)
                {
                    HubConnection? connection = await SignalR.BuildHubConnection(response.Item1);
                    Application.Current!.MainPage = new AppShell(tasks, response.Item1, connection);
                }
            }
            else if (response.Item2 == HttpStatusCode.BadRequest)
            {
                await ToastBar.DisplayToast(response.Item1);
            }
        }
    }
}

