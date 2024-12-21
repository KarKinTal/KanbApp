using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class LoginPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is LoginViewModel loginViewModel)
        {
            loginViewModel.ResetLoginData();
        }
    }
}