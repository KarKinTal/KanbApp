using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage(CreateAccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}