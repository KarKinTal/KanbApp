using Mopups.Services;
using KanbApp.Pages;
using CommunityToolkit.Mvvm.Input;

namespace KanbApp.ViewModels;

public partial class TableMenuViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenTableEdit()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(TableEditPage));
    }
}
