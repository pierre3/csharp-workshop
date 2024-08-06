record Student(string Name, string Class, int Score)
{
    public static IEnumerable<Student> GetSampleData()
    {
        var student = new Student("John Smith", "A", 70);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student("Alice Johnson", "B", 85);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student("Bob Brown", "A", 90);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student("Charlie Davis", "C", 78);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student("Diana Evans", "B", 92);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student("Evan Harris", "C", 88);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student("Fiona Green", "A", 75);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;

        student = new Student("George White", "C", 80);
        ConsoleEx.WriteLine($"@ yield return > {student}", ConsoleColor.DarkYellow);
        yield return student;
    }
}

