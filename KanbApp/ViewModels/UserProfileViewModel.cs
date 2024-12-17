using CommunityToolkit.Mvvm.Input;
using KanbApp.ViewModels;
using KanbApp.Pages;
using Mopups.Services;

namespace KanbApp.ViewModels;

public partial class UserProfileViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenLogin()
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}
