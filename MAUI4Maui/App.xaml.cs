using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace MAUI4Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;

        MainPage = new AppShell();
    }

    private void OnNotificationActionTapped(NotificationActionEventArgs e)
    {
        if (e.IsDismissed)
        {
            MainPage?.DisplayAlert("MAUI for Maui - dismissed", e.Request.Title, "ok");
            return;
        }
        if (e.IsTapped)
        {
            MainPage?.DisplayAlert("MAUI for Maui - tapped", e.Request.Title, "ok");
            // your code goes here
            return;
        }
        
        MainPage?.DisplayAlert("MAUI for Maui - any other case", e.Request.Title, "ok");
    }
}
