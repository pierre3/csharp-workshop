namespace InterfaceGuide.Level4;
using Common;

public interface IHitTestStrategy
{
    bool Contains(Rectangle bounds, double x, double y);
}

public class RectangleHitTestStrategy : IHitTestStrategy
{
    public bool Contains(Rectangle bounds, double x, double y)
    {
        Console.WriteLine($"{nameof(RectangleHitTestStrategy)}.{nameof(Contains)}");
        return bounds.X < x && x < bounds.X + bounds.Width
           && bounds.Y < y && y < bounds.Y + bounds.Height;
    }
}

public class OvalHitTestStrategy : IHitTestStrategy
{
    public bool Contains(Rectangle bounds, double x, double y)
    {
        Console.WriteLine($"{nameof(OvalHitTestStrategy)}.{nameof(Contains)}");
        var ar = bounds.Width / 2;
        var br = bounds.Height / 2;
        var px = x - bounds.X - ar;
        var py = y - bounds.Y - br;
        return (px * px) / (ar * ar) + (py * py) / (br * br) < 1;
    }
}

public interface IGraphics
{
    void DrawRectangle(Rectangle bounds);
    void DrawOval(Rectangle bounds);
}

interface IShape
{
    void Draw(IGraphics g);
    bool Contains(double X, double Y);
}

class RectangleShape(Rectangle bounds, IHitTestStrategy? hittestStrategy = null) : IShape
{
    private IHitTestStrategy HitTestStrategy { get; }
        = hittestStrategy ?? new RectangleHitTestStrategy();

    public Rectangle Bounds { get; } = bounds;
    public void Draw(IGraphics g)
    {
        g.DrawRectangle(Bounds);
    }

    public bool Contains(double X, double Y)
    {
        return HitTestStrategy.Contains(Bounds, X, Y);
    }
}

class OvalShape(Rectangle bounds, IHitTestStrategy? hittestStrategy = null) : IShape
{
    private IHitTestStrategy HitTestStrategy { get; }
        = hittestStrategy ?? new RectangleHitTestStrategy();

    public Rectangle Bounds { get; } = bounds;
    public void Draw(IGraphics g)
    {
        g.DrawOval(Bounds);
    }

    public bool Contains(double X, double Y)
    {
        return HitTestStrategy.Contains(Bounds, X, Y);
    }
}

