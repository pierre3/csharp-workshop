
namespace InterfaceGuide.Level1;
using Common;
using WinDrawing;

/// <summary>
/// 図形を表すクラス
/// </summary>
/// <param name="type">図形の種類</param>
/// <param name="bounds">図形に外接する四角形</param>
public class Shape(Shape.ShapeType type, Rectangle bounds)
{
    public enum ShapeType
    {
        Rectangle,
        Oval
    }
    private WinGraphics _g = new();

    public ShapeType Type { get; } = type;
    public Rectangle Bounds { get; } = bounds;

    /// <summary>
    /// 図形を描画します
    /// </summary>
    public void Draw()
    {
        switch (Type)
        {
            case ShapeType.Rectangle:
                _g.DrawRect(Bounds.ToWRect());
                break;
            case ShapeType.Oval:
                _g.DrawOval(Bounds.ToWRect());
                break;
        }
    }
    /// <summary>
    /// X,Yで指定した点が図形の内部にあるか否かを判定する 
    /// </summary>
    /// <param name="X">X座標</param>
    /// <param name="Y">Y座標</param>
    /// <returns>点が図形内に含まれる場合はTrue</returns>
    public bool Contains(double X, double Y)
    {
        switch (Type)
        {
            case ShapeType.Rectangle:
                return Bounds.X < X && X < Bounds.X + Bounds.Width
                    && Bounds.Y < Y && Y < Bounds.Y + Bounds.Height;
            case ShapeType.Oval:
                var ar = Bounds.Width / 2;
                var br = Bounds.Height / 2;
                var px = X - Bounds.X - ar;
                var py = X - Bounds.Y - br;
                return (px * px) / (ar * ar) + (py * py) / (br * br) < 1;
            default:
                return false;
        }
    }
}