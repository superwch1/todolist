using Mopups.Pages;


namespace todolist.Views;

public partial class MenuView : PopupPage
{

	public double DeviceWidth { get; set; }
	public double DeviceHeight { get; set; }

	public MenuView(double width, double height, double marginTop)
	{
		InitializeComponent();	
		DeviceWidth = width;
		DeviceHeight = height;

		viewFrame.WidthRequest = width / 2;
		viewFrame.HeightRequest = height - marginTop;
		viewFrame.Margin = new Thickness() { Top = marginTop };
	}
}