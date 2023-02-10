using MAUI4Maui.Services;
using MAUI4Maui.ViewModels;
using MAUI4Maui.Views;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Syncfusion.Maui.Core.Hosting;

namespace MAUI4Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
#if ANDROID || IOS
            .UseLocalNotification()
#endif
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddDbContext<DataContext>()
            .AddSingleton<ApiClient>();

        builder.Services.AddTransient<MainPage>()
            .AddTransient<GraphicsPage>()
            .AddTransient<NotificationsPage>()
            .AddTransient<MainViewModel>()
            .AddTransient<GraphicsViewModel>()
            .AddTransient<NotificationsViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
