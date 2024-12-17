using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using KanbApp.ViewModels;
using Mopups.Services;
using System.Diagnostics;

namespace KanbApp.ViewModels;

public partial class MainMenuViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenTableCreate()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(TableCreatePage));
    }

    [RelayCommand]
    public async Task OpenUserProfile()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(UserProfilePage));
    }
}
