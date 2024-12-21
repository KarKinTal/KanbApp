using KanbApp.Pages;
using KanbApp.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KanbApp.ViewModels;

public partial class ChangePasswordViewModel : BaseViewModel
{
    private readonly UserService _userService;

    public ChangePasswordViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string newPassword;

    [ObservableProperty]
    private string confirmPassword;

    [RelayCommand]
    public async Task ChangePasswordAsync()
    {
        try
        {
            bool isSuccesed = await _userService.ChangePasswordAsync(Email, NewPassword, ConfirmPassword);
            if (isSuccesed)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Passwords are not the same.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
