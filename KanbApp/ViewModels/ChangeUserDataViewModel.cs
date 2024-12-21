using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using KanbApp.Services;
using KanbApp.ViewModels;

namespace KanbApp.ViewModels;

public partial class ChangeUserDataViewModel : BaseViewModel
{
    private readonly UserService _userService;

    public ChangeUserDataViewModel(UserService userService)
    {
        _userService = userService;
        LoadUserDataAsync();
    }

    [ObservableProperty]
    private string email;// Musi pobierać maila z aktualnie zalogowanego użytkownika

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string newName;

    private async void LoadUserDataAsync()
    {
        try
        {
            var user = await _userService.GetLoggedInUserAsync();
            if (user != null)
            {
                Email = user.Email; // Wypełnij Email
                Name = user.Name;   // Opcjonalnie wypełnij Name
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Unable to load user data: " + ex.Message, "OK");
        }
    }

    [RelayCommand]
    public async Task ChangeUserDataAsync()
    {
        var user = await _userService.GetLoggedInUserAsync();
        Email = user.Email;
        try
        {
            bool isSuccess = await _userService.UpdateUserDataAsync(Email, NewName, Password);
            if (isSuccess)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Invalid name or password.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    public async Task OpenUserProfile()
    {
        await Shell.Current.GoToAsync(nameof(UserProfilePage));
    }
}
