using System.Net;
using Newtonsoft.Json;

namespace todolist.Services
{
	public class WebServer
	{
		public static string ServerDomain = "https://todolist.superwch1.com";



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

