using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace todolist.Services
{
	public class WebServer
	{
		public static string ServerDomain = "https://todolist.superwch1.com";
        public static string ChatHubUrl => $"{ServerDomain}/chatHub";


        public static async Task<Tuple<string, HttpStatusCode>> Login(string email, string password)
		{
			try
			{
                var url = $"{ServerDomain}/Mobile/Login?email={email}&password={password}";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };
                var response = await http.GetAsync(url);

                var data = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                return Tuple.Create(data, statusCode);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }


        public static async Task<Tuple<string, HttpStatusCode>> RegisterAccount(AccountModel model)
		{
			try
			{
                var url = $"{ServerDomain}/Mobile/RegisterAccount";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await http.PostAsync(url, content);

                var data = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                return Tuple.Create(data, statusCode);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }


        public static async Task<Tuple<string, HttpStatusCode>> ForgetPassword(string email)
		{
			try
			{
                var url = $"{ServerDomain}/Mobile/ForgetPassword?email={email}";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };              
                var response = await http.PostAsync(url, null);

                var data = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                return Tuple.Create(data, statusCode);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }


        public static async Task<Tuple<string, HttpStatusCode>> ForgetPasswordWithJwtToken(string jwtToken)
		{
			try
			{
                var url = $"{ServerDomain}/Mobile/ForgetPasswordWithJwtToken";

                var http = new HttpClient();   
                http.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);             
                var response = await http.PostAsync(url, null);

                var data = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                return Tuple.Create(data, statusCode);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }




        public static async Task<Tuple<string, HttpStatusCode>> VerifyPasscode(string email, string passcode)
		{
			try
			{
                var url = $"{ServerDomain}/Mobile/VerifyPasscode?email={email}&passcode={passcode}";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };            
                var response = await http.PostAsync(url, null);

                var data = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                return Tuple.Create(data, statusCode);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }


        public static async Task<Tuple<string, HttpStatusCode>> ResetPassword(ResetPasswordModel model)
		{
			try
			{
                var url = $"{ServerDomain}/Mobile/ResetPassword";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };               
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await http.PostAsync(url, content);

                var data = await response.Content.ReadAsStringAsync();
                var statusCode = response.StatusCode;
                return Tuple.Create(data, statusCode);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }


  
		public static async Task<Tuple<List<TaskModel>, HttpStatusCode>> ReadTaskFromKeyword(string keyword, string jwtToken)
		{
			try
			{
                var url = $"{ServerDomain}/mobile/ReadTaskFromKeyword?keyword={keyword}";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };
                http.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);

                var response = await http.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(jsonString);
                    return Tuple.Create(tasks!, response.StatusCode);
                }

                return Tuple.Create(new List<TaskModel>(), HttpStatusCode.ExpectationFailed);
            }
            catch
            {
                return Tuple.Create(new List<TaskModel>(), HttpStatusCode.ExpectationFailed);
            }
        }


        public static async Task<Tuple<List<TaskModel>, HttpStatusCode>> ReadTaskFromTime(int year, int month, string jwtToken)
		{
			try
			{
                var url = $"{ServerDomain}/mobile/ReadTaskFromTime?year={year}&month={month}";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };
                http.DefaultRequestHeaders.Add("Authorization", "Bearer " + jwtToken);

                var response = await http.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<List<TaskModel>>(jsonString);
                    return Tuple.Create(tasks!, response.StatusCode);
                }

                return Tuple.Create(new List<TaskModel>(), HttpStatusCode.ExpectationFailed);
            }
            catch
            {
                return Tuple.Create(new List<TaskModel>(), HttpStatusCode.ExpectationFailed);
            }
        }


        public static async Task<Tuple<string, HttpStatusCode>> ReadPrivacyPolicy()
		{
			try
			{
                var url = $"{ServerDomain}/mobile/ReadPrivacyPolicy";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };

                var response = await http.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var policyContent = await response.Content.ReadAsStringAsync();
                    return Tuple.Create(policyContent, response.StatusCode);
                }

                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }


        public static async Task<Tuple<string, HttpStatusCode>> ReadTermsAndConditions()
		{
			try
			{
                var url = $"{ServerDomain}/mobile/ReadTermsAndConditions";

                var http = new HttpClient{ Timeout = TimeSpan.FromSeconds(5) };

                var response = await http.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var policyContent = await response.Content.ReadAsStringAsync();
                    return Tuple.Create(policyContent, response.StatusCode);
                }

                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
            catch
            {
                return Tuple.Create("", HttpStatusCode.ExpectationFailed);
            }
        }
	}
}

