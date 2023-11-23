﻿using System.Net;
using Microsoft.AspNetCore.SignalR.Client;

namespace todolist.ViewModels
{
	public class LoginViewModel
	{
        AccountDatabase _accountDatabase;

        public LoginViewModel(AccountDatabase accountDatabase)
        {
            _accountDatabase = accountDatabase;
        }

        public async Task Login(string email, string password)
        {
            var response = await WebServer.Login(email, password);
            if (response == null)
            {
                return;
            }

            if (response.Item2 == HttpStatusCode.OK)
            {
                await ToastBar.DisplayToast("Logging in");
                await _accountDatabase.UpdateItemAsync(new AccountModel() { Id = 1, JwtToken = response.Item1 });
                var tasks = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, response.Item1);

                if (tasks != null)
                {
                    HubConnection? connection = await SignalR.BuildHubConnection(response.Item1);
                    Application.Current!.MainPage = new AppShell(tasks, response.Item1, connection, _accountDatabase);
                }
            }
            else if (response.Item2 == HttpStatusCode.BadRequest)
            {
                await ToastBar.DisplayToast(response.Item1);
            }
        }
    }
}

