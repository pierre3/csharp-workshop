namespace InterfaceGuide.WinDrawing;
public record struct WinRect(double X, double Y, double Width, double Height);

public class WinGraphics
{
    public void DrawRect(WinRect rect) => Console.WriteLine($"{nameof(WinGraphics)}.{nameof(DrawRect)}");
    public void DrawOval(WinRect rect) => Console.WriteLine($"{nameof(WinGraphics)}.{nameof(DrawOval)}");
}

public class WinRegion
{
    public void AddRect(WinRect rect) { }
    public void AddOval(WinRect rect) { }
    public bool Contains(double X, double Y) => true;   
}
