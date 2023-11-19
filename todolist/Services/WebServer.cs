using System.Net;
using Newtonsoft.Json;

namespace todolist.Services
{
	public class WebServer
	{
		public static string ServerDomain = "https://todolist.superwch1.com";


        public static async Task<Tuple<string, HttpStatusCode>?> Login(string email, string password)
		{
			try
			{
                var url = $"https://todolist.superwch1.com/Mobile/Login?" +
                $"email={email}&password={password}";

                var http = new HttpClient();
                var response = await http.GetAsync(url);

                var data = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                return Tuple.Create(data, statusCode);
            }
            catch
            {
                return null;
            }
        }


		public static async Task<List<TaskModel>?> ReadTaskFromTime(int year, int month, string jwtToken)
		{
			try
			{
                var url = $"{ServerDomain}/mobile/ReadTaskFromTime?year={year}&month={month}";

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);

                var response = await httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(jsonString);
                    return tasks;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
	}
}

