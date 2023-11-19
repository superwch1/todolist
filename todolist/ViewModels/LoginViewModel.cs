using System.Net;

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
            var http = new HttpClient();
            var response = await http.GetAsync("https://todolist.superwch1.com/Mobile/Login?" +
                $"email={email}&password={password}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jwtToken = await response.Content.ReadAsStringAsync();
                await _accountDatabase.UpdateItemAsync(new AccountModel() { Id = 1, JwtToken = jwtToken });

                var tasks = await WebServer.ReadTaskFromTime(DateTime.Now.Year, DateTime.Now.Month, jwtToken);

                if (tasks != null)
                {
                    Application.Current!.MainPage = new AppShell(tasks, _accountDatabase);
                }
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                await ToastBar.DisplayToast(responseContent);
            }
        }
    }
}

