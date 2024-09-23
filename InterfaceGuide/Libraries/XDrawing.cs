namespace InterfaceGuide.XDrawing;

public record struct XRect(double X, double Y, double Width, double Height);

public class XGraphics
{
    public void DrawRectangle(XRect rect) => Console.WriteLine($"{nameof(XGraphics)}.{nameof(DrawRectangle)}");
    public void DrawEllipse(XRect rect) => Console.WriteLine($"{nameof(XGraphics)}.{nameof(DrawEllipse)}");
}

public class XRegion
{
    public void AddRect(XRect rect) { }
    public void AddOval(XRect rect) { }
    public bool Contains(double X, double Y) => true;
}