﻿using System.Net;

namespace todolist.ViewModels.AccountViewModels
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

            await ToastBar.DisplayToast("Logging in");

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
                await UserDatabase.UpdateItemAsync(new UserModel() { Id = 1, JwtToken = loginResponse.Item1 });

                SignalR.Connection = await SignalR.BuildHubConnection(loginResponse.Item1);
                var taskReponse = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, loginResponse.Item1);     

                if (taskReponse.Item2 == HttpStatusCode.OK && SignalR.Connection != null)
                {
                    Application.Current!.MainPage = new TaskShell(taskReponse.Item1, loginResponse.Item1, false);
                }
            }
        }
    }
}

