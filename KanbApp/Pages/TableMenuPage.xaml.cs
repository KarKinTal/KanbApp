using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TableMenuPage
{
    public TableMenuPage(TableMenuViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}