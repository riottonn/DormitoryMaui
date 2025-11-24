using DormitoryMaui.Models;

namespace DormitoryMaui;

public partial class StudentPage : ContentPage
{
    private Student _student;
    public bool Saved { get; private set; }

    public StudentPage(Student student)
    {
        InitializeComponent();
        _student = student;
        LoadStudent();
    }

    private void LoadStudent()
    {
        PibEntry.Text = _student.PIB;
        FacultyEntry.Text = _student.Faculty;
        DepartmentEntry.Text = _student.Department;

        CourseEntry.Text = _student.Course.ToString();
        RoomEntry.Text = _student.Room.ToString();
        BlockEntry.Text = _student.Block.ToString();

        CheckInPicker.Date = _student.CheckInDate;
        CheckOutPicker.Date = _student.CheckOutDate;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PibEntry.Text) ||
            string.IsNullOrWhiteSpace(FacultyEntry.Text) ||
            string.IsNullOrWhiteSpace(DepartmentEntry.Text))
        {
            await DisplayAlert("Помилка", "Заповніть всі поля!", "OK");
            return;
        }

        _student.PIB = PibEntry.Text;
        _student.Faculty = FacultyEntry.Text;
        _student.Department = DepartmentEntry.Text;

        int.TryParse(CourseEntry.Text, out int course);
        int.TryParse(RoomEntry.Text, out int room);
        int.TryParse(BlockEntry.Text, out int block);

        _student.Course = course;
        _student.Room = room;
        _student.Block = block;

        _student.CheckInDate = CheckInPicker.Date;
        _student.CheckOutDate = CheckOutPicker.Date;

        Saved = true;
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        Saved = false;
        await Navigation.PopAsync();
    }
}
