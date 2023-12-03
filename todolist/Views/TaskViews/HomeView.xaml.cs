namespace todolist.Views;

public partial class HomeView : FlyoutPage
{
	public HomeView()
	{
		InitializeComponent();

        Flyout = new ContentPage
        {
            Title = "Menu",
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Menu item 1" },
                    new Label { Text = "Menu item 2" }
                }
            }
        };

        // Create the detail page
        Detail = new NavigationPage(new ContentPage
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Detail page content" }
                }
            }
        });
    }
}