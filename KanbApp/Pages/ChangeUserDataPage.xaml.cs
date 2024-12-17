using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class ChangeUserDataPage
{
	public ChangeUserDataPage(ChangeUserDataViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}