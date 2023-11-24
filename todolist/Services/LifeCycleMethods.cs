using Microsoft.AspNetCore.SignalR.Client;

namespace todolist.Services 
{
    public static class LifeCycleMethods
    {
        public static int EnterForegroundCount = 0;
        public static List<Func<Task>> ActivatedActions = new List<Func<Task>> () 
        {
            new Func<Task>(async () => { await Task.Run(() => EnterForegroundCount++);})
        };
        public static List<Func<Task>> DeactivatedActions = new List<Func<Task>> ();
    }
}
