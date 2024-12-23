using KanbApp.ViewModels;
using KanbApp.Models;

namespace KanbApp.Pages;

public partial class TaskCreatePage
{
    public TaskCreatePage(TaskCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private void OnUserCheckChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is TaskCreateViewModel viewModel)
        {
            if (sender is CheckBox checkBox)
            {
                // SprawdŸ, czy BindingContext jest poprawnie ustawiony na User
                if (checkBox.BindingContext is User user)
                {
                    System.Diagnostics.Debug.WriteLine($"CheckedChanged triggered for UserId: {user.Id}, IsChecked: {e.Value}");

                    if (e.Value)
                    {
                        // Dodanie u¿ytkownika do listy zaznaczonych
                        viewModel.SelectedUserIds.Add(user.Id);
                        System.Diagnostics.Debug.WriteLine($"Added UserId to SelectedUserIds: {user.Id}");
                    }
                    else
                    {
                        // Usuniêcie u¿ytkownika z listy zaznaczonych
                        viewModel.SelectedUserIds.Remove(user.Id);
                        System.Diagnostics.Debug.WriteLine($"Removed UserId from SelectedUserIds: {user.Id}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("BindingContext of CheckBox is not a User.");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Sender is not a CheckBox.");
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("BindingContext is not TaskCreateViewModel.");
        }
    }
}