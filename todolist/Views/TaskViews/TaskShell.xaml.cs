﻿using Microsoft.AspNetCore.SignalR.Client;
using todolist.Views.AccountViews;

namespace todolist.Views.TaskViews;

public partial class TaskShell : Shell
{
	public TaskShell(List<TaskModel> tasks, string jwtToken, HubConnection connection)
	{
		Routing.RegisterRoute("searchview", typeof(SearchView));
		Routing.RegisterRoute("resetpassword", typeof(ResetPasswordView));
		Routing.RegisterRoute("policy", typeof(PolicyView));

		InitializeComponent();
		myTask.Content = new TaskView(tasks, 0, jwtToken, connection);
		followUpTask.Content = new TaskView(tasks, 1, jwtToken, connection);
    }
}

