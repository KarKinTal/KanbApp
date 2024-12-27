using KanbApp.Pages;
using KanbApp.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Mopups.Services;

namespace KanbApp.ViewModels;

public partial class CreateAccountViewModel : BaseViewModel
{
    private readonly UserService _userService;

    public CreateAccountViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string confirmPassword;

    [RelayCommand]
    public async Task RegisterAsync()
    {
        try
        {
            bool isSuccess = await _userService.RegisterUserAsync(Email, Name, Password, ConfirmPassword);
            if (isSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "Your account has been successfully created.", "OK");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Registration failed.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
