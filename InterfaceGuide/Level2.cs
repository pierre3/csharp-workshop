namespace InterfaceGuide.Level2;
using WinDrawing;
using Common;


interface IShape
{
    void Draw();
    bool Contains(double X, double Y);
}

class RectangleShape(Rectangle bounds) : IShape
{
    private WinGraphics _g = new();
    public Rectangle Bounds { get; } = bounds;
    public void Draw()
    {
        _g.DrawRect(Bounds.ToWRect());
    }

    public bool Contains(double X, double Y)
    {
        return Bounds.X < X && X < Bounds.X + Bounds.Width
            && Bounds.Y < Y && Y < Bounds.Y + Bounds.Height;
    }
}

class OvalShape(Rectangle bounds) : IShape
{
    private WinGraphics _g = new();
    public Rectangle Bounds { get; } = bounds;
    public void Draw()
    {
        _g.DrawOval(Bounds.ToWRect());
    }
    public bool Contains(double X, double Y)
    {
        var ar = Bounds.Width / 2;
        var br = Bounds.Height / 2;
        return (Bounds.X * Bounds.X) / (ar * ar) + (Bounds.X * Bounds.X) / (br * br) < 1;
    }
}

