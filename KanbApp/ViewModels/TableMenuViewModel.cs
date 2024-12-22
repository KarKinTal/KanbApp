using KanbApp.Services;
using KanbApp.Pages;
using KanbApp.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Task = System.Threading.Tasks.Task;

namespace KanbApp.ViewModels;

public partial class TableMenuViewModel : BaseViewModel
{
    private readonly UserService _userService;
    private readonly TableService _tableService;

    [ObservableProperty]
    private Table currentTable;

    [RelayCommand]
    public async Task OpenTableEdit()
    {
        await Shell.Current.GoToAsync(nameof(TableEditPage));
    }
}
