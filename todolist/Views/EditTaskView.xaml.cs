using System.Collections.ObjectModel;
using Mopups.Pages;

namespace todolist.Views;

public partial class EditTaskView : PopupPage
{
	public EditTaskView(TaskModel model)
	{
		InitializeComponent();
		
		topic.Text = model.Topic;
		dueDate.Date = model.DueDate;
		content.Text = model.Content;
	}

	
}