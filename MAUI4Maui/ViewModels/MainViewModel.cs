using System.Collections.ObjectModel;
using Brain2CPU.MvvmEssence;
using MAUI4Maui.Models;
using MAUI4Maui.Models.DB;
using MAUI4Maui.Services;

namespace MAUI4Maui.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DataContext _data;
    private readonly ApiClient _apiClient;
    
    private List<NameId> _availableItems;

    public ObservableCollection<NameId> AvailableItems { get; } = new();

    public ObservableCollection<NameId> FavoriteItems { get; } = new();

    public NameId SelectedAvailableItem
    {
        get => Get<NameId>();
        set => Set(value, AddItemCommand);
    }

    public NameId SelectedFavoriteItem
    {
        get => Get<NameId>();
        set => Set(value, RemoveItemCommand);
    }

    public string Filter
    {
        get => Get("");
        set
        {
            if (Set(value) && _availableItems != null)
            {
                AvailableItems.Clear();
                foreach (var it in _availableItems.Where(x => x.Name.Contains(value, StringComparison.CurrentCultureIgnoreCase)))
                {
                    AvailableItems.Add(it);
                }
            }
        }
    }

    public MainViewModel(DataContext data, ApiClient apiClient)
    {
        _data = data;
        _apiClient = apiClient;

        LoadFavorites();
    }

    public RelayCommandAsync FetchDataCommand => Get(FetchDataAsync, () => !IsBusy);

    private async Task FetchDataAsync()
    {
        IsBusy = true;

        try
        {
            AvailableItems.Clear();

            _availableItems = await _apiClient.GetTickersAsync();

            foreach (var it in _availableItems)
            {
                AvailableItems.Add(it);
            }
        }
        catch (Exception xcp)
        {
            await Shell.Current.DisplayAlert("Error", xcp.Message, "OK");
        }
        finally
        {
            IsBusy = false;

            NotifyPropertyChanged(nameof(AvailableItems));
        }

        /*
        // adding fake data here:
        if (!FavoriteItems.Any())
        {
            await _data.Stocks.AddAsync(new Stock { Id = "FakeID.01", Name = "Dâni" });
            await _data.Stocks.AddAsync(new Stock { Id = "FakeID.02", Name = "Däni" });
            await _data.Stocks.AddAsync(new Stock { Id = "FakeID.03", Name = "Dani" });
            await _data.Stocks.AddAsync(new Stock { Id = "FakeID.04", Name = "Dáni" });
            await _data.Stocks.AddAsync(new Stock { Id = "FakeID.05", Name = "Deni" });
            await _data.Stocks.AddAsync(new Stock { Id = "FakeID.06", Name = "Dåni" });
            await _data.Stocks.AddAsync(new Stock { Id = "FakeID.07", Name = "Dăni" });
            await _data.SaveChangesAsync();

            LoadFavorites();
        }
        */
    }

    public RelayCommandAsync AddItemCommand => Get(AddItemAsync, () => !IsBusy && SelectedAvailableItem != null);

    private async Task AddItemAsync()
    {
        IsBusy = true;

        try
        {
            if (FavoriteItems.Any(x => x == SelectedAvailableItem))
                return;

            await _data.Stocks.AddAsync(new Stock { Id = SelectedAvailableItem.Id, Name = SelectedAvailableItem.Name });
            await _data.SaveChangesAsync();

            LoadFavorites();
        }
        catch (Exception xcp)
        {
            await Shell.Current.DisplayAlert("Error", xcp.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public RelayCommandAsync RemoveItemCommand => Get(RemoveItemAsync, () => !IsBusy && SelectedFavoriteItem != null);

    private async Task RemoveItemAsync()
    {
        IsBusy = true;

        try
        {
            var s = _data.Stocks.SingleOrDefault(x => string.Equals(x.Id, SelectedFavoriteItem.Id));
            if(s == null) 
                return;

            _data.Stocks.Remove(s);
            await _data.SaveChangesAsync();

            SelectedFavoriteItem = null;
            LoadFavorites();
        }
        catch (Exception xcp)
        {
            await Shell.Current.DisplayAlert("Error", xcp.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LoadFavorites()
    {
        FavoriteItems.Clear();

        foreach (var stock in _data.Stocks.OrderBy(x => x.Name))
        {
            FavoriteItems.Add(new NameId(stock.Name, stock.Id));
        }
    }
}

/*
 public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            AddCommand.ChangeCanExecute();
        }
    }

    public MainViewModel()
    {
        _addCommand = new Command(Add, _ => !IsBusy && SelectedAvailableItem != null);

        AvailableItems.Add(new NameId("Bolek", "Z1"));
        AvailableItems.Add(new NameId("Lolek", "Z2"));
        AvailableItems.Add(new NameId("Marek", "Z3"));
    }

    public ObservableCollection<NameId> AvailableItems { get; } = new();

    private NameId _selectedAvailableItem;
    public NameId SelectedAvailableItem
    {
        get => _selectedAvailableItem;
        set
        {
            if (_selectedAvailableItem == value)
                return;

            _selectedAvailableItem = value;
            OnPropertyChanged();
            AddCommand.ChangeCanExecute();
        }
    }

    private readonly Command _addCommand;
    public Command AddCommand => _addCommand;

    private void Add(object obj)
    {
        Shell.Current.DisplayAlert("MAUI", SelectedAvailableItem.Name, "OK");
    }

}
 */ 