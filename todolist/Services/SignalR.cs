using Microsoft.AspNetCore.SignalR.Client;

namespace todolist.Services 
{
    public class SignalR 
    {
        public static HubConnection? Connection { get; set; }

        public async static Task<HubConnection?> BuildHubConnection(string jwtToken){
            try 
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl(WebServer.ChatHubUrl, 
                        options => 
                            options.AccessTokenProvider = async () => await Task.FromResult(jwtToken))
                    .WithAutomaticReconnect(new MyRetryPolicy())
                    .Build();
                await connection.StartAsync();

                connection.KeepAliveInterval = new TimeSpan(0, 0, 10);
                connection.ServerTimeout = new TimeSpan(0, 0, 20);

                return connection;
            }
            catch {
                return null;
            }
        }

        public class MyRetryPolicy : IRetryPolicy
        {
            public TimeSpan? NextRetryDelay(RetryContext retryContext)
            {
                return TimeSpan.FromSeconds(5);
            }
        }
    }
}
