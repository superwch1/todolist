using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;

namespace todolist.ViewModels
{
	public class MenuViewModel
	{
       	public async Task Logout(HubConnection connection)
		{
			try 
			{
				await MopupService.Instance.PopAsync();
			}
			catch { }

			connection.Remove("DeleteTask");
			connection.Remove("DeleteThenCreateTask");
			await connection.StopAsync();

			//loop for two times for two page in shell
			for(var i = 0; i < 2; i++)
			{
				LifeCycleMethods.ActivatedActions.RemoveAt(LifeCycleMethods.ActivatedActions.Count - 1);
				LifeCycleMethods.DeactivatedActions.RemoveAt(LifeCycleMethods.DeactivatedActions.Count - 1);
			}
			await UserDatabase.UpdateItemAsync(new UserModel() { Id = 1, JwtToken = "" });
			Application.Current!.MainPage = new NavigationPage(new LoginView());
		}


		public async Task SearchTask(HubConnection connection, string keyword, string jwtToken)
		{
			var taskReponse = await WebServer.ReadTaskFromKeyword(keyword, jwtToken);
            if (taskReponse.Item2 != HttpStatusCode.OK)
            {
                await ToastBar.DisplayToast("Cannot connect to server");
                return;
            }
			List<TaskModel> tasks = taskReponse.Item1;

			if (tasks.Count > 0)
			{
				try 
				{
					await MopupService.Instance.PopAsync();
					await Shell.Current.GoToAsync($"searchview",
						new Dictionary<string, object>{
							{ "connection", connection },
							{ "jwtToken", jwtToken },
							{ "tasks", tasks },
							{ "keyword", keyword }
						});
				}
				catch { }
			}
			else 
			{
				await ToastBar.DisplayToast("No search results");
			}		
		}
    }
}