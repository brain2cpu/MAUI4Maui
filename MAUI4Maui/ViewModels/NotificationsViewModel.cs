using Brain2CPU.MvvmEssence;
using Plugin.LocalNotification;

namespace MAUI4Maui.ViewModels;

// https://github.com/thudugala/Plugin.LocalNotification/wiki

public class NotificationsViewModel : ViewModelBase
{
    public string Title
    {
        get => Get("");
        set => Set(value);
    }
    
    public string Description
    {
        get => Get("");
        set => Set(value);
    }

    public bool EnableRepeat
    {
        get => Get(true);
        set => Set(value);
    }

    public DateTime NotificationDate
    {
        get => Get(DateTime.Today);
        set => Set(value.Date);
    }

    public TimeSpan NotificationTime
    {                                         // obviously incorrect after 22:59 :)
        get => Get(TimeSpan.FromHours(DateTime.Now.Hour + 1));
        set => Set(value);
    }

    public int RepeatInterval
    {
        get => Get(5);
        set => Set(value);
    }

    public bool IsAnyNotificationSet
    {
        get => Get(false);
        set => Set(value, ClearCommand);
    }

    public async Task LoadExitingNotificationAsync()
    {
        IsBusy = true;

        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            await LocalNotificationCenter.Current.RequestNotificationPermission();

        var ex = await LocalNotificationCenter.Current.GetPendingNotificationList();
        var notification = ex.FirstOrDefault();
        if (notification != null)
        {
            Title = notification.Title;
            Description = notification.Description;
            NotificationDate = notification.Schedule.NotifyTime?.Date ?? DateTime.Today;
            NotificationTime = notification.Schedule.NotifyTime?.TimeOfDay ?? new TimeSpan(DateTime.Now.Hour + 1, 0, 0);  // see above
            EnableRepeat = notification.Schedule.RepeatType == NotificationRepeat.TimeInterval;
            RepeatInterval = notification.Schedule.NotifyRepeatInterval?.Minutes ?? 5;
        }
        
        IsAnyNotificationSet = notification != null;

        IsBusy = false;
    }

    public RelayCommand SetCommand => Get(Set, () => !IsBusy);

    private void Set()
    {
        LocalNotificationCenter.Current.ClearAll();
        LocalNotificationCenter.Current.CancelAll();

        var notification = new NotificationRequest
        {
            NotificationId = 100,
            Title = Title,
            Description = Description,
            Schedule =
            {
                NotifyTime = NotificationDate.Add(NotificationTime),
                RepeatType = EnableRepeat ? NotificationRepeat.TimeInterval : NotificationRepeat.No,
                NotifyRepeatInterval = TimeSpan.FromMinutes(RepeatInterval)
            }
        };
        LocalNotificationCenter.Current.Show(notification);

        IsAnyNotificationSet = true;
    }

    public RelayCommand ClearCommand => Get(Clear, () => !IsBusy && IsAnyNotificationSet);

    private void Clear()
    {
        LocalNotificationCenter.Current.ClearAll();
        LocalNotificationCenter.Current.CancelAll();

        IsAnyNotificationSet = false;
    }
}