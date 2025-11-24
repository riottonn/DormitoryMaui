using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DormitoryMaui.Models;

public class Student : INotifyPropertyChanged
{
    private int _id;
    private string _pib = string.Empty;
    private string _faculty = string.Empty;
    private string _department = string.Empty;
    private int _course;
    private int _room;
    private int _block;
    private DateTime _checkInDate;
    private DateTime _checkOutDate;

    public int Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string PIB
    {
        get => _pib;
        set => SetField(ref _pib, value);
    }

    public string Faculty
    {
        get => _faculty;
        set => SetField(ref _faculty, value);
    }

    public string Department
    {
        get => _department;
        set => SetField(ref _department, value);
    }

    public int Course
    {
        get => _course;
        set => SetField(ref _course, value);
    }

    public int Room
    {
        get => _room;
        set => SetField(ref _room, value);
    }

    public int Block
    {
        get => _block;
        set => SetField(ref _block, value);
    }

    public DateTime CheckInDate
    {
        get => _checkInDate;
        set => SetField(ref _checkInDate, value);
    }

    public DateTime CheckOutDate
    {
        get => _checkOutDate;
        set => SetField(ref _checkOutDate, value);
    }

    public override string ToString() => $"{Id} {PIB}";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}