using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using KanbApp.Pages;
using Mopups.Services;

namespace KanbApp.ViewModels;

public partial class NewTableViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task OpenTableCreate()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync(nameof(TableCreatePage));
    }
}
