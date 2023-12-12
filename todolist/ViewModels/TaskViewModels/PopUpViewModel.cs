using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;

namespace todolist.ViewModels.TaskViewModels
{
	public class PopUpViewModel
	{
        public async Task CreateTask(
            int intType, string topic, string content, DateTime dueDate, int intSymbol)
        {
            if (String.IsNullOrEmpty(topic))
            {
                await ToastBar.DisplayToast("Please enter topic");
                return;
            }

            try 
            {
                await SignalR.Connection.InvokeAsync("CreateTask", 
                    intType, topic, content, dueDate.Date.ToString("dd-MM-yyyy"), intSymbol);
                await MopupService.Instance.PopAsync();
            }
            catch
            {
                await ToastBar.DisplayToast("Cannont connect to server");
            }
        }


        public async Task UpdateTask(
            int id, int intType, string topic, string content, DateTime dueDate, int intSymbol)
        {
            if (String.IsNullOrEmpty(topic))
            {
                await ToastBar.DisplayToast("Please enter topic");
                return;
            }

            try 
            {
                await SignalR.Connection.InvokeAsync("UpdateTask", 
                    id, intType, topic, content, dueDate.Date.ToString("dd-MM-yyyy"), intSymbol);
                await MopupService.Instance.PopAsync();
            }
            catch
            {
                await ToastBar.DisplayToast("Cannont connect to server");
            }
        }
    }
}

