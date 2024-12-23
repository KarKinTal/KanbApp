using KanbApp.ViewModels;
using KanbApp.Models;

namespace KanbApp.Pages;

public partial class TableEditPage
{
    public TableEditPage(TableEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnColumnNameChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Column column)
        {
            await (BindingContext as TableEditViewModel)?.UpdateColumnNameAsync(column);
        }
    }
}