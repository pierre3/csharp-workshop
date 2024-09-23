namespace InterfaceGuide.App;

using InterfaceGuide.Common;
using InterfaceGuide.WinDrawing;
using InterfaceGuide.XDrawing;


class WinGraphicsWrapper : Level3.IGraphics, Level4.IGraphics
{
    private readonly WinGraphics g = new();

    public void DrawOval(Rectangle bounds)
    {
        g.DrawOval(bounds.ToWRect());
    }

    public void DrawRectangle(Rectangle bounds)
    {
        g.DrawRect(bounds.ToWRect());
    }
}

class XGraphicsWrapper : Level3.IGraphics, Level4.IGraphics
{
    private readonly XGraphics g = new();

    public void DrawOval(Rectangle bounds)
    {
        g.DrawEllipse(bounds.ToXRect());
    }

    public void DrawRectangle(Rectangle bounds)
    {
        g.DrawRectangle(bounds.ToXRect());
    }
}

class WinHitTestStrategy() : Level4.IHitTestStrategy
{
    public bool Contains(Rectangle bounds, double x, double y)
    {
        Console.WriteLine($"{nameof(WinHitTestStrategy)}.{nameof(Contains)}");
        var region = new WinRegion();
        region.AddOval(bounds.ToWRect());
        return region.Contains(x, y);
    }
}

class XHitTestStrategy() : Level4.IHitTestStrategy
{
    public bool Contains(Rectangle bounds, double x, double y)
    {
        Console.WriteLine($"{nameof(XHitTestStrategy)}.{nameof(Contains)}");
        var region = new XRegion();
        region.AddOval(bounds.ToXRect());
        return region.Contains(x, y);
    }
}