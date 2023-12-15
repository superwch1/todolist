using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Pages;
using Mopups.Services;


namespace todolist.Views.TaskViews;

public partial class DeleteAlertView : PopupPage
{
	int? TaskId { get; set; }
	string Type { get; set; }

	public DeleteAlertView(string labelText, string type, int? taskId)
	{
		InitializeComponent();
		TaskId = taskId;
		Type = type;
		label.Text = labelText;
	}


	public async void Yes(object sender, EventArgs args)
	{
		try
		{
			if (Type == "DeleteTask")
			{
				using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
				{
					await SignalR.Connection.InvokeAsync("DeleteTask", TaskId, cancellationTokenSource.Token);
				}
				await MopupService.Instance.PopAsync();
			}
			else if (Type == "DeleteAccount")
			{
				await MopupService.Instance.PopAllAsync();
				Application.Current.MainPage = new AccountShell();

				await ToastBar.DisplayToast("Account Deleted");
			}
		}
		catch 
		{ 
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}

	public async void No(object sender, EventArgs args)
	{
		try
		{
			await MopupService.Instance.PopAsync();
		}
		catch 
		{ 
			await ToastBar.DisplayToast("Cannont connect to server");
		}
	}
}