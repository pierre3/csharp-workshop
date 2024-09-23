namespace InterfaceGuide.Level3;
using Common;

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


class RectangleShape(Rectangle bounds) : IShape
{
    public Rectangle Bounds { get; } = bounds;
    public void Draw(IGraphics g)
    {
        g.DrawRectangle(Bounds);
    }

    public bool Contains(double X, double Y)
    {
        return Bounds.X < X && X < Bounds.X + Bounds.Width
            && Bounds.Y < Y && Y < Bounds.Y + Bounds.Height;
    }
}

class OvalShape(Rectangle bounds) : IShape
{
    public Rectangle Bounds { get; } = bounds;
    public void Draw(IGraphics g)
    {
        g.DrawOval(Bounds);
    }

    public bool Contains(double X, double Y)
    {
        var ar = Bounds.Width / 2;
        var br = Bounds.Height / 2;
        var px = X - Bounds.X - ar;
        var py = X - Bounds.Y - br;
        return (px * px) / (ar * ar) + (py * py) / (br * br) < 1;
    }
}

