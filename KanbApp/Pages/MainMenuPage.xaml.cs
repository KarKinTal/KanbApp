using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class MainMenuPage
{
    public MainMenuPage(MainMenuViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainMenuViewModel viewModel)
        {
            viewModel.RefreshTables();
        }
    }
}