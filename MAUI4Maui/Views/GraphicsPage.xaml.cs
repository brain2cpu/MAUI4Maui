using MAUI4Maui.ViewModels;

namespace MAUI4Maui.Views;

public partial class GraphicsPage : ContentPage
{
	public GraphicsPage(GraphicsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}

