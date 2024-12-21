using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using KanbApp.Services;
using Mopups.Services;
using System.Diagnostics;

namespace KanbApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly UserService _userService;

    public LoginViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [RelayCommand]
    public async Task LoginAsync()
    {
        try
        {
            bool isSuccess = await _userService.LoginAsync(Email, Password);
            if (isSuccess)
            {
                await Shell.Current.GoToAsync($"//{nameof(TablePage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Invalid email or password.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    public async Task OpenCreatingAccount()
    {
        await Shell.Current.GoToAsync(nameof(CreateAccountPage));
    }

    [RelayCommand]
    public async Task OpenChangePassword()
    {
        await Shell.Current.GoToAsync(nameof(ChangePasswordPage));
    }

    public void ResetLoginData()
    {
        Email = string.Empty;
        Password = string.Empty;
    }
}
