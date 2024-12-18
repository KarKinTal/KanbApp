using CommunityToolkit.Mvvm.Input;
using KanbApp.ViewModels;
using KanbApp.Pages;
using Mopups.Services;

namespace KanbApp.ViewModels;

public partial class UserProfileViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenChangingUserData()
    {
        await MopupService.Instance.PushAsync(new ChangeUserDataPage(new ChangeUserDataViewModel()));
    }

    [RelayCommand]
    public async Task OpenLogin()
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}
