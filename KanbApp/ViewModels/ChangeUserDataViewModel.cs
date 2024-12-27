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
        if (user == null)
        {
            await Shell.Current.DisplayAlert("Error", "User not logged in.", "OK");
            return;
        }

        bool hasChanges = false;
        string errorMessage = "";

        // Zmiana nazwy
        if (!string.IsNullOrWhiteSpace(NewName) && NewName != user.Name)
        {
            bool nameUpdated = await _userService.UpdateUserDataAsync(user.Email, NewName, null);
            if (nameUpdated)
            {
                hasChanges = true;
            }
            else
            {
                errorMessage += "Failed to update name. ";
            }
        }

        // Zmiana hasła
        if (!string.IsNullOrWhiteSpace(Password))
        {
            bool passwordUpdated = await _userService.UpdateUserDataAsync(user.Email, user.Name, Password);
            if (passwordUpdated)
            {
                hasChanges = true;
            }
            else
            {
                errorMessage += "Failed to update password. ";
            }
        }

        // Wyświetl wynik
        if (hasChanges)
        {
            await Shell.Current.DisplayAlert("Success", "User data updated successfully.", "OK");
            await Shell.Current.GoToAsync("..?Refresh=true");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", errorMessage.Trim(), "OK");
        }
    }

    [RelayCommand]
    public async Task OpenUserProfile()
    {
        await Shell.Current.GoToAsync(nameof(UserProfilePage));
    }
}
