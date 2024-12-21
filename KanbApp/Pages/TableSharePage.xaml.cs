using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TableSharePage
{
	public TableSharePage(TableShareViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}