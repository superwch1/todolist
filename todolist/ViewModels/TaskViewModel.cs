using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;
namespace todolist.ViewModels
{
	public class TaskViewModel
	{
		public void ShowOrHideContent(ObservableCollection<TaskModel> tasks, 
			ScrollView scrollview, TaskModel selectedTask)
		{
			// Use LINQ to find the task
			var task = tasks.FirstOrDefault(x => x.Id == selectedTask.Id);

			if (task != null)
			{
				task.IsContentVisible = !selectedTask.IsContentVisible;
				task.ArrowImageSource = task.IsContentVisible == true ? "downarrow" : "uparrow";

				//reduce the height of scrollview after change back to invisible
				(scrollview as IView).InvalidateMeasure();
			}
		}

		public void DeleteTask(ObservableCollection<TaskModel> tasks, ScrollView scrollview, int id)
		{
			//since the method is invoked in SignalR, it need to use MainThread for browsing UI
			MainThread.BeginInvokeOnMainThread(() =>
			{
				var task = tasks.FirstOrDefault(x => x.Id == id);
				if (task != null)
					{
						var index = tasks.IndexOf(task);
						tasks.RemoveAt(index);

						(scrollview as IView).InvalidateMeasure();
					}
				});	
			}

		public void CreateTask(ObservableCollection<TaskModel> tasks,
			ScrollView scrollView, int viewIntType,
			int id, int intType, string topic, string content, string dueDate, int intSymbol)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{				
				if (intType == viewIntType)
				{
					var newTask = new TaskModel() { Id = id, IntType = intType, Topic = topic, 
					Content = content, DueDate = Convert.ToDateTime(dueDate), IntSymbol = intSymbol };

					List<TaskModel> tempTasks = new List<TaskModel>();
					tempTasks = tasks.ToList();

					tempTasks.Add(newTask);

					tempTasks = tempTasks
						.OrderBy(x => x.IntSymbol)
						.ThenBy(x => x.DueDate)
						.ToList();

					var index = tempTasks.IndexOf(newTask);
					
					if (index >= tasks.Count)
					{
						tasks.Add(newTask); // Add the item to the end of the list
					}
					else
					{
						tasks.Insert(index, newTask); // Insert the item at the calculated index
					}
				}
			});	
		}


		public void CounterCheckTask(ObservableCollection<TaskModel> tasks,
			ScrollView scrollview, DateTime selectedDateTime, int intType, string jwtToken)
	{
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			var tasksFromServer = await WebServer.ReadTaskFromTime(selectedDateTime.Year, selectedDateTime.Month, jwtToken);

			if (tasksFromServer == null)
			{
				return;
			}
			tasksFromServer = tasksFromServer.Where(x => x.IntType == intType).ToList();
			List<int> tasksId = tasks.Select(x => x.Id).ToList();

			foreach(var t in tasksFromServer) 
			{
				var task = tasks.Where(x => x.Id == t.Id).FirstOrDefault();
				if (task != null)
				{
					if (t.Topic != task.Topic || t.Content != task.Content || t.DueDate != task.DueDate ||
						t.IntSymbol != task.IntSymbol) 
					{
						DeleteTask(tasks, scrollview, task.Id);
						CreateTask(tasks, scrollview, intType, t.Id, t.IntType, t.Topic, t.Content,
							t.DueDate.ToString("dd-MM-yyyy"), t.IntSymbol);
					}
					tasksId.Remove(task.Id);
				}
				else
				{
					CreateTask(tasks, scrollview, intType, t.Id, t.IntType, t.Topic, t.Content,
						t.DueDate.ToString("dd-MM-yyyy"), t.IntSymbol);
				}	
			}

			foreach (var id in tasksId)
			{
				DeleteTask(tasks, scrollview, id);
			}
		});
	}
	}
}

