using MAUI4Maui.ViewModels;

namespace MAUI4Maui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}

