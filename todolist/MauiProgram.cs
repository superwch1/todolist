using CommunityToolkit.Maui;
using Maui.FixesAndWorkarounds;
using Microsoft.Extensions.Logging;

namespace todolist;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        RemoveBorderAndUndeline();

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.RegisterServices()
			.ConfigureKeyboardAutoScroll()
            .ConfigureFonts(fonts =>
			{
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}


    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
		//mauiAppBuilder.Services.AddSingleton<AccountDatabase>();
        return mauiAppBuilder;
    }


    public static void RemoveBorderAndUndeline(){
        //no underline for entry in android and IOS
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });

        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            //handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });
        Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });

    }
}

