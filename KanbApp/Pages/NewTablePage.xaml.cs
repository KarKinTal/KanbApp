using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class NewTablePage
{
	public NewTablePage(NewTableViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}