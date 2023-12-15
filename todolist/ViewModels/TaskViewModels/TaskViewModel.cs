using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Mopups.Services;
using todolist.Views.AccountViews;
using todolist.Views.TaskViews;
namespace todolist.ViewModels.TaskViewModels
{
	public class TaskViewModel
	{
		public DateTime SelectedDateTime { get; set; }
		public TaskViewModel(DateTime selectedDateTime)
		{
			SelectedDateTime = selectedDateTime;
		}

		public async Task UpdateTask(TaskModel model)
        {
            try 
            {
				using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
					await SignalR.Connection.InvokeAsync("UpdateTask", model.Id, model.IntType, model.Topic, 
						model.Content, model.DueDate.Date.ToString("dd-MM-yyyy"), model.IntSymbol,
						cancellationTokenSource.Token);
                }
            }
            catch
            {
                await ToastBar.DisplayToast("Cannont connect to server");
            }
        }


		public void ShowOrHideContent(ObservableCollection<TaskModel> tasks, 
			ScrollView scrollview, TaskModel selectedTask)
		{
			// Use LINQ to find the task
			var task = tasks.FirstOrDefault(x => x.Id == selectedTask.Id);

			if (task != null)
			{
				task.IsContentVisible = !selectedTask.IsContentVisible;
				task.ArrowImageSource = task.IsContentVisible == true ? "uparrow" : "downarrow";

				//reduce the height of scrollview after change back to invisible
				(scrollview as IView).InvalidateMeasure();
			}
		}

		public void DeleteTask(ObservableCollection<TaskModel> tasks, ScrollView scrollview, int id)
		{
			//since the method is invoked in SignalR, it need to use MainThread for browsing UI
			MainThread.BeginInvokeOnMainThread(async () =>
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


		public void DeleteAllTask(ObservableCollection<TaskModel> tasks, ScrollView scrollview)
		{
			tasks.Clear();
			(scrollview as IView).InvalidateMeasure();
		}


		public async Task ReadTaskFromSelectedPeriod(ObservableCollection<TaskModel> tasks, ScrollView scrollview,
			DateTime selectedDateTime, string jwtToken, int intType)
		{
			var taskResponse = await WebServer.ReadTaskFromTime(selectedDateTime.Year, selectedDateTime.Month, jwtToken);

			if (taskResponse.Item2 != HttpStatusCode.OK)
			{
				await ToastBar.DisplayToast("Cannot connect to server");
				return;
			}
			
			var tasksFromServer = taskResponse.Item1
				.Where(x => x.IntType == intType)
				.OrderBy(x => x.IntSymbol)
				.ThenBy(x => x.DueDate)
				.ToList();

			foreach(var t in tasksFromServer)
			{
				tasks.Add(t);
			}			
		}


		public async Task Logout()
		{
			//loop for two times for two page in shell
			for(var i = 0; i < 2; i++)
			{
				LifeCycleMethods.ActivatedActions.RemoveAt(LifeCycleMethods.ActivatedActions.Count - 1);
				LifeCycleMethods.DeactivatedActions.RemoveAt(LifeCycleMethods.DeactivatedActions.Count - 1);
			}
			await UserDatabase.UpdateItemAsync(new UserModel() { Id = 1, JwtToken = "" });
			Application.Current!.MainPage = new NavigationPage(new LoginView());
		}


		public void CreateTask(ObservableCollection<TaskModel> tasks,
			ScrollView scrollView, bool ingoreTime, int viewIntType,
			int id, int intType, string topic, string content, string dueDate, int intSymbol)
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{		
				var newTask = new TaskModel() { Id = id, IntType = intType, Topic = topic, 
					Content = content, DueDate = Convert.ToDateTime(dueDate), IntSymbol = intSymbol };

				if (intType == viewIntType && newTask.DueDate.Year == SelectedDateTime.Year &&
						newTask.DueDate.Month == SelectedDateTime.Month)
				{				
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

				else if (ingoreTime == true)
				{
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


		public async Task CounterCheckTask(ObservableCollection<TaskModel> tasks,
			ScrollView scrollview, DateTime selectedDateTime, int intType, string jwtToken)
		{
			var taskResponse = await WebServer.ReadTaskFromTime(selectedDateTime.Year, selectedDateTime.Month, jwtToken);

			if (taskResponse.Item2 != HttpStatusCode.OK)
			{
				await ToastBar.DisplayToast("Cannot connect to server");
				return;
			}
			var tasksFromServer = taskResponse.Item1.Where(x => x.IntType == intType).ToList();
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
						CreateTask(tasks, scrollview, false, intType, t.Id, t.IntType, t.Topic, t.Content,
							t.DueDate.ToString(), t.IntSymbol);
					}
					tasksId.Remove(task.Id);
				}
				
				else
				{
					CreateTask(tasks, scrollview, false, intType, t.Id, t.IntType, t.Topic, t.Content,
						t.DueDate.ToString(), t.IntSymbol);
				}	
			}

			foreach (var id in tasksId)
			{
				DeleteTask(tasks, scrollview, id); 
			}
		}


		public async Task CounterCheckTaskFromKeyword(ObservableCollection<TaskModel> tasks,
			ScrollView scrollview, string keyword, string jwtToken)
		{
			var taskResponse = await WebServer.ReadTaskFromKeyword(keyword, jwtToken);

			if (taskResponse.Item2 != HttpStatusCode.OK)
			{
				await ToastBar.DisplayToast("Cannot connect to server");
				return;
			}
			var tasksFromServer = taskResponse.Item1.ToList();

			foreach(var t in tasksFromServer) 
			{
				var task = tasks.Where(x => x.Id == t.Id).FirstOrDefault();
				if (task != null)
				{
					if (t.Topic != task.Topic || t.Content != task.Content || t.DueDate != task.DueDate ||
						t.IntSymbol != task.IntSymbol) 
					{
						DeleteTask(tasks, scrollview, task.Id);
						CreateTask(tasks, scrollview, true, t.IntType, t.Id, t.IntType, t.Topic, t.Content,
							t.DueDate.ToString(), t.IntSymbol);
					}
				}
			}
		}


		public async Task SwipeChanging(SwipeView swipeView, SwipeChangingEventArgs args, 
			double offSet)
		{
			if (args.SwipeDirection == SwipeDirection.Left)
			{
				var frame = (StackLayout)swipeView.Parent;
				double alpha = 1 + Math.Max(-1, offSet / 50);
				frame.Opacity = alpha;

				if (offSet < -50)
				{
					var selectedTask = (TaskModel)swipeView.BindingContext;
					var deleteAlertView = new DeleteAlertView("Do you want to delete the task?", "DeleteTask", selectedTask.Id);
					deleteAlertView.Disappearing += (sender, args) => {
						frame.Opacity = 1;	
						swipeView.IsEnabled = true;
						swipeView.Close();		
					};

					//stop user from further swiping and open duplicated alert
					swipeView.IsEnabled = false;
					await IsLoading.RunMethod(() => MopupService.Instance.PushAsync(deleteAlertView));
				}
			}
			else if (args.SwipeDirection == SwipeDirection.Right && offSet > 50)
			{
				var selectedTask = (TaskModel)swipeView.BindingContext;
				selectedTask.IntSymbol = selectedTask.IntSymbol == 0 ? 1 : 0;

				//still figuring how to stop using sending repeating invoke when no connection
				await IsLoading.RunMethod(() => UpdateTask(selectedTask));
			}
		}


		public void SwipeEnded(object sender, SwipeEndedEventArgs args, double offSet,
			ObservableCollection<TaskModel> tasks, ScrollView scrollView)
		{
			var swipeView = (SwipeView)sender;
			var frame = (Frame)swipeView.Content;
    		var selectedTask = (TaskModel)frame.BindingContext;

			if (offSet < 5 && offSet > -5)
			{
				ShowOrHideContent(tasks, scrollView, selectedTask);
			}

			if (args.SwipeDirection == SwipeDirection.Left && offSet >= -50)
			{
				var stackLayout = (StackLayout)swipeView.Parent;
				stackLayout.Opacity = 1;
			}
			swipeView.Close();
		}
	}
}

