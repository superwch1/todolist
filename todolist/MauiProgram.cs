using CommunityToolkit.Maui;
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
			"NoUnderline", 
			(h, v) => 
			{ h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent); 
			});
#endif
        return builder.Build();
	}


    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
		mauiAppBuilder.Services.AddSingleton<AccountDatabase>();
        return mauiAppBuilder;
    }
}

