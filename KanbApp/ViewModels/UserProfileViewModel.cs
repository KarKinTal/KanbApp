using CommunityToolkit.Mvvm.Input;
using KanbApp.Services;
using KanbApp.Pages;
using Mopups.Services;
using Mopups.PreBaked.PopupPages.Login;
using CommunityToolkit.Mvvm.ComponentModel;
using KanbApp.Models;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels;

public partial class UserProfileViewModel : BaseViewModel, IQueryAttributable
{
    private readonly UserService _userService;

    [ObservableProperty]
    private string userName;

    public UserProfileViewModel(UserService userService)
    {
        _userService = userService;
        _ = LoadUserNameAsync();
    }

    private async Task LoadUserNameAsync()
    {
        var user = await _userService.GetLoggedInUserAsync(); // Pobierz aktualnie zalogowanego użytkownika
        if (user != null)
        {
            UserName = user.Name; // Przypisz nazwę użytkownika do właściwości
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Refresh", out var refresh)
            && bool.TryParse(refresh.ToString(), out var shouldRefresh)
            && shouldRefresh)
        {
            _ = LoadUserNameAsync(); // Odśwież dane użytkownika
        }
    }

    [RelayCommand]
    public async Task OpenChangingUserData()
    {
        await Shell.Current.GoToAsync(nameof(ChangeUserDataPage));
    }

    [RelayCommand]
    public async Task OpenTable()
    {
        await Shell.Current.GoToAsync($"///{nameof(TablePage)}");
    }


    [RelayCommand]
    public async Task Logout()
    {
        try
        {
            await _userService.LogoutAsync();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
