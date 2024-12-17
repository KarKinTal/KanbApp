using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class LoginPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}