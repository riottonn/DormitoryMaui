using System.Collections.ObjectModel;
using DormitoryMaui.Models;
using DormitoryMaui.Services;

namespace DormitoryMaui;

public partial class MainPage : ContentPage
{
    private ObservableCollection<Student> _students = new();
    private List<Student> _allStudents = new();
    private string? _currentFilePath;

    public MainPage()
    {
        InitializeComponent();

        BindingContext = _students;

        StudentsCollection.ItemsSource = _students;
        SearchPicker.SelectedIndex = 0;
    }

    private async void OnOpenJsonClicked(object sender, EventArgs e)
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Оберіть JSON",
                FileTypes = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".json" } }
                    })
            };

            var result = await FilePicker.PickAsync(options);
            if (result == null)
                return;

            _currentFilePath = result.FullPath;

            _allStudents = JsonStorage.Load(_currentFilePath);

            _students.Clear();
            foreach (var st in _allStudents)
                _students.Add(st);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async void OnSaveJsonClicked(object sender, EventArgs e)
    {
        if (_currentFilePath == null)
            _currentFilePath = Path.Combine(FileSystem.AppDataDirectory, "students.json");

        JsonStorage.Save(_currentFilePath, _students.ToList());
        await DisplayAlert("OK", "Файл збережено.", "OK");
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var student = new Student
        {
            Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1,
            CheckInDate = DateTime.Today,
            CheckOutDate = DateTime.Today.AddMonths(6)
        };

        var page = new StudentPage(student);
        await Navigation.PushAsync(page);

        if (page.Saved)
        {
            _students.Add(student);
            _allStudents = _students.ToList();
        }
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (StudentsCollection.SelectedItem is not Student student)
        {
            await DisplayAlert("Увага", "Оберіть студента.", "OK");
            return;
        }

        var page = new StudentPage(student);
        await Navigation.PushAsync(page);

        if (page.Saved)
        {
            // ВАЖЛИВО: Перемалювати список
            StudentsCollection.ItemsSource = null;
            StudentsCollection.ItemsSource = _students;

            _allStudents = _students.ToList();
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (StudentsCollection.SelectedItem is not Student student)
        {
            await DisplayAlert("Увага", "Оберіть студента.", "OK");
            return;
        }

        bool yes = await DisplayAlert("Підтвердження",
            $"Видалити {student.PIB}?", "Так", "Ні");

        if (yes)
        {
            _students.Remove(student);
            _allStudents = _students.ToList();
        }
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        var value = SearchEntry.Text?.Trim() ?? "";

        if (string.IsNullOrEmpty(value))
        {
            await DisplayAlert("Помилка", "Введіть значення.", "OK");
            return;
        }

        IEnumerable<Student> q = _allStudents;

        switch (SearchPicker.SelectedItem)
        {
            case "ПІБ":
                q = q.Where(s => s.PIB.Contains(value, StringComparison.OrdinalIgnoreCase));
                break;

            case "Факультет":
                q = q.Where(s => s.Faculty.Contains(value, StringComparison.OrdinalIgnoreCase));
                break;

            case "Курс":
                if (int.TryParse(value, out int course))
                    q = q.Where(s => s.Course == course);
                break;

            case "Кімната":
                if (int.TryParse(value, out int room))
                    q = q.Where(s => s.Room == room);
                break;
        }

        _students.Clear();
        foreach (var st in q)
            _students.Add(st);
    }

    private void OnResetSearchClicked(object sender, EventArgs e)
    {
        _students.Clear();
        foreach (var st in _allStudents)
            _students.Add(st);

        SearchEntry.Text = "";
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
    }
}