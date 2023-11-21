using CommunityToolkit.Maui;
using Maui.FixesAndWorkarounds;
using Microsoft.Extensions.Logging;

namespace todolist;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.RegisterServices()
			.ConfigureKeyboardAutoScroll()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

//no underline for entry in android
#if ANDROID
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
			"EntryNoUnderline", 
			(h, v) => 
				{ h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent); }
		);

		Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(
			"EditorNoUnderline", 
			(h, v) => 
				{ h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent); }
		);
		
#endif
        return builder.Build();
	}


    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
		mauiAppBuilder.Services.AddSingleton<AccountDatabase>();
        return mauiAppBuilder;
    }
}

