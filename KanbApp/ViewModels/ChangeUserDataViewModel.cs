using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using KanbApp.ViewModels;
using Mopups.Services;

namespace KanbApp.ViewModels;

public partial class ChangeUserDataViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenUserProfile()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(UserProfilePage));
    }
}
