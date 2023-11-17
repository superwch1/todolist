using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace todolist.Services
{
	public class ToastBar
	{
		public static async Task DisplayToast(string message)
		{
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(message, duration, fontSize);
            await toast.Show();
        }
	}
}

