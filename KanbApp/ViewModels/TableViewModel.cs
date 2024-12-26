using CommunityToolkit.Mvvm.Input;
using KanbApp.ViewModels;
using KanbApp.Pages;
using KanbApp.Services;
using KanbApp.Models;
using Mopups.Services;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KanbApp.ViewModels;

public partial class TableViewModel : BaseViewModel, IQueryAttributable
{
    private readonly UserService _userService;
    private readonly TableService _tableService;
    private readonly TaskService _taskService;

    private int _currentColumnIndex = 0;

    [ObservableProperty]
    private Table currentTable;

    [ObservableProperty]
    private List<Column> columns;

    [ObservableProperty]
    private ObservableCollection<Models.Task> tasks;

    [ObservableProperty]
    private Column currentColumn;

    [ObservableProperty]
    private bool isFirstColumn;

    [ObservableProperty]
    private bool isLastColumn;

    [ObservableProperty]
    private bool showPreviousColumnButton;

    [ObservableProperty]
    private bool showNextColumnButton;

    public TableViewModel(UserService userService, TableService tableService, TaskService taskService)
    {

        _userService = userService;
        _tableService = tableService;
        _taskService = taskService;
        Columns = new List<Column>();
        Tasks = new ObservableCollection<Models.Task>();
        _ = InitializeTable();
    }

    private async Task InitializeTable(int? tableId = null)
    {
        await CheckUserTablesAsync();

        if (tableId.HasValue)
        {
            await LoadTableByIdAsync(tableId.Value);
        }
        else
        {
            await LoadDefaultTableForUserAsync();
        }
        UpdateColumnNavigation();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TableId", out var tableIdObj) && int.TryParse(tableIdObj.ToString(), out var tableId))
        {
            // Jeśli TableId jest podane, załaduj tę tabelę
            await LoadTableByIdAsync(tableId);
        }
        else
        {
            // Jeśli nie podano TableId, załaduj domyślną tabelę użytkownika
            await LoadDefaultTableForUserAsync();
        }
        UpdateColumnNavigation();
    }

    public async Task LoadTableByIdAsync(int tableId)
    {
        CurrentTable = await _tableService.GetTableByIdAsync(tableId);
        if (CurrentTable != null)
        {
            Columns = await _tableService.GetColumnsForTableAsync(tableId);

            // Wybierz pierwszą kolumnę
            if (Columns.Any())
            {
                var user = await _userService.GetLoggedInUserAsync();
                _currentColumnIndex = await FindColumnWithUserTasksAsync(user.Id) ?? 0; // Domyślnie pierwsza kolumna
                UpdateColumnNavigation();
                await LoadTasksForCurrentColumnAsync();
            }
        }
    }

    public async Task LoadDefaultTableForUserAsync()
    {
        var user = await _userService.GetLoggedInUserAsync();
        if (user == null)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }

        var tables = await _tableService.GetTablesForUserAsync(user.Id);
        if (tables.Any())
        {
            var defaultTable = tables.OrderBy(t => t.Id).First();
            await LoadTableByIdAsync(defaultTable.Id);
        }
        else
        {
            await Shell.Current.GoToAsync("//NewTablePage");
        }
    }

    private async Task<int?> FindColumnWithUserTasksAsync(int userId)
    {
        for (int i = 0; i < Columns.Count; i++)
        {
            var column = Columns[i];
            if (await _taskService.DoesColumnContainTasksForUserAsync(column.Id, userId))
            {
                return i;
            }
        }
        return null; // No column with user tasks found
    }

    private void UpdateColumnNavigation()
    {
        IsFirstColumn = _currentColumnIndex == 0;
        IsLastColumn = _currentColumnIndex == Columns.Count - 1;
        CurrentColumn = Columns.ElementAtOrDefault(_currentColumnIndex);

        ShowPreviousColumnButton = !IsFirstColumn;
        ShowNextColumnButton = !IsLastColumn;
    }

    private async Task LoadTasksForCurrentColumnAsync()
    {
        if (CurrentColumn != null)
        {
            var tasks = await _taskService.GetTasksByColumnIdWithUsersAsync(CurrentColumn.Id);

            // Ustaw domyślną wartość IsExpanded na false dla każdego zadania
            foreach (var task in tasks)
            {
                task.IsExpanded = false;
            }

            Tasks = new ObservableCollection<Models.Task>(tasks);
        }
    }

    public bool CanMoveTaskToPreviousColumn(Models.Task task)
    {
        if (task == null || Columns == null) return false;
        var currentColumn = Columns.FirstOrDefault(c => c.Id == task.ColumnId);
        return currentColumn != null && Columns.IndexOf(currentColumn) > 0;
    }

    public bool CanMoveTaskToNextColumn(Models.Task task)
    {
        if (task == null || Columns == null) return false;
        var currentColumn = Columns.FirstOrDefault(c => c.Id == task.ColumnId);
        return currentColumn != null && Columns.IndexOf(currentColumn) < Columns.Count - 1;
    }

    public async Task RefreshTasksAsync()
    {
        if (CurrentColumn != null)
        {
            await LoadTasksForCurrentColumnAsync();
        }
    }

    private async Task CheckUserTablesAsync()
    {
        var loggedInUser = await _userService.GetLoggedInUserAsync();
        if (loggedInUser == null)
        {
            await Shell.Current.GoToAsync("//LoginPage"); // Wylogowanie, jeśli brak zalogowanego użytkownika
            return;
        }

        // Pobierz tabele przypisane do użytkownika
        var userTables = await _tableService.GetTablesForUserAsync(loggedInUser.Id);

        // Jeśli użytkownik nie ma żadnych tabel, przekieruj na stronę NewTablePage
        if (!userTables.Any())
        {
            await Shell.Current.GoToAsync("//NewTablePage"); // Ustawienie NewTablePage jako początek stosu
        }
    }


    [RelayCommand]
    public void ToggleTaskExpansion(Models.Task task)
    {
        if (task == null) return;

        // Zmień stan widoczności zadania
        task.IsExpanded = !task.IsExpanded;

        // Odśwież widok
        var index = Tasks.IndexOf(task);
        if (index >= 0)
        {
            Tasks[index] = task;
        }
    }

    [RelayCommand]
    public void NextColumn()
    {
        if (_currentColumnIndex < Columns.Count - 1)
        {
            _currentColumnIndex++;
            UpdateColumnNavigation();
            _ = LoadTasksForCurrentColumnAsync();
        }
    }

    [RelayCommand]
    public void PreviousColumn()
    {
        if (_currentColumnIndex > 0)
        {
            _currentColumnIndex--;
            UpdateColumnNavigation();
            _ = LoadTasksForCurrentColumnAsync();
        }
    }

    [RelayCommand]
    public async Task OpenTaskCreate()
    {
        if (CurrentColumn == null || CurrentTable == null) return;

        var columnId = CurrentColumn.Id;
        var tableId = CurrentTable.Id;

        await Shell.Current.GoToAsync($"TaskCreatePage?ColumnId={columnId}&TableId={tableId}");
    }

    [RelayCommand]
    public async Task MoveTaskToNextColumn(Models.Task task)
    {
        if (task == null || CurrentTable == null || Columns == null) return;

        var currentColumn = Columns.FirstOrDefault(c => c.Id == task.ColumnId);
        if (currentColumn == null) return;

        var nextColumnIndex = Columns.IndexOf(currentColumn) + 1;
        if (nextColumnIndex >= Columns.Count) return; // Ostatnia kolumna, brak przesunięcia

        var nextColumn = Columns[nextColumnIndex];
        task.ColumnId = nextColumn.Id;

        await _taskService.UpdateTaskAsync(task); // Zaktualizuj zadanie w bazie danych
        await LoadTasksForCurrentColumnAsync();   // Odśwież widok
    }

    [RelayCommand]
    public async Task MoveTaskToPreviousColumn(Models.Task task)
    {
        if (task == null || CurrentTable == null || Columns == null) return;

        var currentColumn = Columns.FirstOrDefault(c => c.Id == task.ColumnId);
        if (currentColumn == null) return;

        var previousColumnIndex = Columns.IndexOf(currentColumn) - 1;
        if (previousColumnIndex < 0) return; // Pierwsza kolumna, brak przesunięcia

        var previousColumn = Columns[previousColumnIndex];
        task.ColumnId = previousColumn.Id;

        await _taskService.UpdateTaskAsync(task); // Zaktualizuj zadanie w bazie danych
        await LoadTasksForCurrentColumnAsync();   // Odśwież widok
    }

    [RelayCommand]
    public async Task OpenTaskEdit(Models.Task task)
    {
        if (task == null) return;

        var navigationParameters = new Dictionary<string, object>
    {
        { "TaskId", task.Id },
        { "TableViewModel", this }
    };

        await Shell.Current.GoToAsync("TaskEditPage", navigationParameters);
    }

    [RelayCommand]
    public async Task OpenMainMenu()
    {
        await Shell.Current.GoToAsync(nameof(MainMenuPage));
    }

    [RelayCommand]
    public async Task OpenTableMenu()
    {
        if (CurrentTable != null)
        {
            await Shell.Current.GoToAsync($"{nameof(TableMenuPage)}?TableId={CurrentTable.Id}");
        }
    }
}
