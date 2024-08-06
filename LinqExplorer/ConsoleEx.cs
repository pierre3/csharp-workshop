static class ConsoleEx
{
    public static void WriteLine(object? o, ConsoleColor color)
    {
        var saveColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            Console.WriteLine(o);
        }
        finally
        {
            Console.ForegroundColor = saveColor;
        }
    }

    public static void WriteLine(string s, ConsoleColor color)
    {
        var saveColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            Console.WriteLine(s);
        }
        finally
        {
            Console.ForegroundColor = saveColor;
        }
    }

}


