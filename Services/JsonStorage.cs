using System.Text.Json;
using DormitoryMaui.Models;

namespace DormitoryMaui.Services;

public static class JsonStorage
{
    public static List<Student> Load(string path)
    {
        if (!File.Exists(path))
            return new List<Student>();

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<Student>>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Student>();
    }

    public static void Save(string path, List<Student> students)
    {
        var json = JsonSerializer.Serialize(students,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(path, json);
    }
}