using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TableEditPage
{
    public TableEditPage(TableEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}