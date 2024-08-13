using ConsoleAppFramework;


partial class MyApp
{
    [Command("delegate")]
    public void DelegateSample()
    {
        var students = new Student[]
        {
            new (1, "John Smith", "A", 70),
            new (2, "Alice Johnson", "B", 85),
            new (3, "Bob Brown", "A", 90),
            new (4, "Charlie Davis", "C", 78),
            new (5, "Diana Evans", "B", 92),
            new (6, "Evan Harris", "C", 88),
            new (7, "Fiona Green", "A", 75),
            new (8, "George White", "C", 80)
        };
        //デリゲート
        var query = students
            .Where(new Func<Student, bool>(WhereId))
            .Select(new Func<Student, string>(SelectName));

        //デリゲート(2)（関数名を渡すだけでデリゲートに変換してくれる）
        var query1 = students
            .Where(WhereId)
            .Select(SelectName);

        //匿名メソッド式
        //関数の定義をそのまま書けるように
        var query2 = students
            .Where(delegate (Student s) { return s.Id < 5; })
            .Select(delegate (Student s) { return s.Name; });

        //ラムダ式
        //delegateが省略可能に
        var query3 = students
            .Where((Student s) => { return s.Id < 5; })
            .Select((Student s) => { return s.Name; });
        //引数の型名も省略可能
        var query4 = students
            .Where((s) => { return s.Id < 5; })
            .Select((s) => { return s.Name; });
        //引数が１つの場合()も省略可能
        //メソッドの本体が１行で書ける場合、 {} と return が省略可能
        var query5 = students
            .Where(s => s.Id < 5)
            .Select(s => s.Name);
    }

    private bool WhereId(Student s)
    {
        return s.Id < 5;
    }

    private string SelectName(Student s)
    {
        return s.Name;
    }

}