using KanbApp.Services;

namespace KanbApp.Pages;

public partial class CheckingLoginPage : ContentPage
{
    private readonly IAuthService _authService;
    public CheckingLoginPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (await _authService.IsUserAuthenticated())
        {
            await Shell.Current.GoToAsync($"//{nameof(TablePage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}