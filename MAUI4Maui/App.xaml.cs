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
            // your code goes here
            return;
        }
        if (e.IsTapped)
        {
            // your code goes here
            return;
        }
        
        // if Notification Action are setup
        switch (e.ActionId)
        {
            // your code goes here
        }
    }
}
