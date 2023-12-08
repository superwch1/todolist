namespace todolist.Views.TaskViews;

public partial class PolicyView : ContentPage, IQueryAttributable
{
	public PolicyView()
	{
		InitializeComponent();
	}

    protected override async void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		Shell.SetTabBarIsVisible(this, false);
	}


    //workaround for the add button hidden under tab after navigate back from serach view in IOS
	//it happens when you expand all of the task in search view then go backwards
	protected override void OnDisappearing()
	{
		base.OnDisappearing();

#if IOS
		Shell.SetTabBarIsVisible(this, true);
#endif
	}


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		string policyType = query["policyType"] as string;
		string policyContent = query["policyContent"] as string;

        policyLabel.Text = policyContent;
		Title = policyType;
    }
}