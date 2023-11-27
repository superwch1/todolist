namespace todolist.Services 
{
    public static class IsLoading
    {
        public static bool status = false;

        //prevents spamming button from having multiple HTTP request or page transition
        public static async Task RunMethod(Func<Task> action) 
        {
            if (status == true)
            {
                return;
            }
            status = true;
            await action();
            status = false;
        }
    }
}