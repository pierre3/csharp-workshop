namespace InterfaceGuide.Common;

using WinDrawing;
using XDrawing;

public record struct Rectangle(double X, double Y, double Width, double Height);


public static class RectangleExtensions
{
    public static WinRect ToWRect(this Rectangle rect) => new WinRect(rect.X, rect.Y, rect.Width, rect.Height);
    public static XRect ToXRect(this Rectangle rect) => new XRect(rect.X, rect.Y, rect.Width, rect.Height);
}



