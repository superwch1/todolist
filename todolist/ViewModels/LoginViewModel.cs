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
                await _accountDatabase.UpdateItemAsync(new AccountInfo() { Id = 1, JwtToken = jwtToken });

                var abc = new HttpClient();
                abc.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);
                var re = await abc.GetAsync("https://todolist.superwch1.com/Mobile/ReadTaskFromTime?" +
                    "year=2023&month=11");
                var abcde = await re.Content.ReadAsStringAsync();
                Console.WriteLine(abcde);


                Application.Current.MainPage = new AppShell();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                await ToastBar.DisplayToast(responseContent);
            }
        }
    }
}

