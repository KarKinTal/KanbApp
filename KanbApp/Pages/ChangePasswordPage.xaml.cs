using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class ChangePasswordPage
{
	public ChangePasswordPage(ChangePasswordViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}