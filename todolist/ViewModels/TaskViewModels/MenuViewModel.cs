using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;

namespace todolist.ViewModels
{
	public class MenuViewModel
	{
       public async Task Logout(HubConnection connection)
		{
            await MopupService.Instance.PopAsync();

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
    }
}