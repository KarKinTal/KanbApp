using KanbApp.ViewModels;

namespace KanbApp.Pages;

public partial class TaskEditPage
{
    public TaskEditPage(TaskEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}