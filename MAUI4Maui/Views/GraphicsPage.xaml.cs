using MAUI4Maui.ViewModels;

namespace MAUI4Maui.Views;

public partial class GraphicsPage
{
	public GraphicsPage(GraphicsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}

