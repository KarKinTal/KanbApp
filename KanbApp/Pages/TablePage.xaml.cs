using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TablePage
{
    public TablePage(TableViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Pobierz ViewModel przypisany do TablePage
        if (BindingContext is TableViewModel viewModel)
        {
            await viewModel.InitializeTable();
        }
    }
}