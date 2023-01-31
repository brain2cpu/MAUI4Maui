using System.Collections.ObjectModel;
using Brain2CPU.MvvmEssence;
using MAUI4Maui.Models;
using MAUI4Maui.Models.DB;
using MAUI4Maui.Services;
using Microsoft.EntityFrameworkCore;

namespace MAUI4Maui.ViewModels;

public class GraphicsViewModel : ViewModelBase
{
    private readonly DataContext _data;
    private readonly ApiClient _apiClient;

    public ObservableCollection<DateValue> LastMonthValues { get; } = new();

    public ObservableCollection<NameId> FavoriteItems { get; } = new();

    public NameId SelectedItem
    {
        get => Get<NameId>();
        set => Set(value, FetchValuesCommand);
    }

    public GraphicsViewModel(DataContext data, ApiClient apiClient)
    {
        _data = data;
        _apiClient = apiClient;

        LoadFavorites();
    }

    public RelayCommandAsync FetchValuesCommand => Get(FetchValuesAsync, () => !IsBusy && SelectedItem != null);

    private async Task FetchValuesAsync()
    {
        IsBusy = true;

        try
        {
            LastMonthValues.Clear();

            var r = await _apiClient.GetAggregatesAsync(SelectedItem.Id, DateTime.Now.AddMonths(-1), DateTime.Now);

            foreach (var item in r)
            {
                LastMonthValues.Add(item);
                
                if(_data.Prices.AsNoTracking().Any(x => string.Equals(x.StockId, SelectedItem.Id) && x.Time.Date == item.Date.Date))
                   continue;

                await _data.Prices.AddAsync(new TimeValue
                    { StockId = SelectedItem.Id, Time = item.Date, Value = item.Value });
            }

            await _data.SaveChangesAsync();
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
            FavoriteItems.Add(new NameId {Name = stock.Name, Id = stock.Id});
        }
    }
}