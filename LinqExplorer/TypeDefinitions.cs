record Student(int Id, string Name, string Class, int Score)
{
    public static IEnumerable<Student> EnumerateData()
    {
        var student = new Student(1, "John Smith", "A", 70);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student(2, "Alice Johnson", "B", 85);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student(3, "Bob Brown", "A", 90);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student(4, "Charlie Davis", "C", 78);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student(5, "Diana Evans", "B", 92);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student(6, "Evan Harris", "C", 88);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student(7, "Fiona Green", "A", 75);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student(8, "George White", "C", 80);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;
    }
}

delegate int SqrtDelegate(int n);
