using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;

namespace todolist.ViewModels
{
	public class PopUpViewModel
	{
        public async Task CreateTask(HubConnection connection,
            int intType, string topic, string content, DateTime dueDate, int intSymbol)
        {
            if (String.IsNullOrEmpty(topic))
            {
                await ToastBar.DisplayToast("Please enter topic");
                return;
            }

            try 
            {
                await connection.InvokeAsync("CreateTask", 
                    intType, topic, content, dueDate.Date.ToString("dd-MM-yyyy"), intSymbol);
                await MopupService.Instance.PopAsync();
            }
            catch
            {
                await ToastBar.DisplayToast("Cannont connect to server");
            }
        }


        public async Task DeleteTask(HubConnection connection, int id)
        {
            try 
            {
                await connection.InvokeAsync("DeleteTask", id);
                await MopupService.Instance.PopAsync();
            }
            catch
            {
                await ToastBar.DisplayToast("Cannont connect to server");
            }
        }


        public async Task UpdateTask(HubConnection connection,
            int id, int intType, string topic, string content, DateTime dueDate, int intSymbol)
        {
            if (String.IsNullOrEmpty(topic))
            {
                await ToastBar.DisplayToast("Please enter topic");
                return;
            }

            try 
            {
                await connection.InvokeAsync("UpdateTask", 
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

