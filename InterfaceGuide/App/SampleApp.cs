namespace InterfaceGuide.App;
using ConsoleAppFramework;
using InterfaceGuide.Common;

class SampleApp
{
    [Command("level1")]
    public void Level1()
    {
        var shapes = new[] {
            new Level1.Shape(InterfaceGuide.Level1.Shape.ShapeType.Rectangle, new Rectangle(100, 100, 150, 150)),
            new Level1.Shape(InterfaceGuide.Level1.Shape.ShapeType.Oval, new Rectangle(300, 100, 120, 180)),
        };

        var cursorX = 123;
        var cursorY = 144;
        foreach (var shape in shapes)
        {
            shape.Draw();
            Console.WriteLine($"Contains({cursorX},{cursorY}): " + shape.Contains(123, 123));
        }
    }

    [Command("level2")]
    public void Level2()
    {
        var shapes = new Level2.IShape[] {
            new Level2.RectangleShape(new Rectangle(100, 100, 150, 150)),
            new Level2.OvalShape(new Rectangle(300, 100, 120, 180)),
        };

        var cursorX = 123;
        var cursorY = 144;
        foreach (var shape in shapes)
        {
            shape.Draw();
            Console.WriteLine($"Contains({cursorX},{cursorY}): " + shape.Contains(123, 123));
        }
    }

    [Command("level3")]
    public void Level3()
    {

        var shapes = new Level3.IShape[] {
                new Level3.RectangleShape(new Rectangle(100, 100, 150, 150)),
                new Level3.OvalShape(new Rectangle(300, 100, 120, 180)),
            };

        var cursorX = 123;
        var cursorY = 144;
        var wg = new WinGraphicsWrapper();

        foreach (var shape in shapes)
        {
            shape.Draw(wg);
            Console.WriteLine($"Contains({cursorX},{cursorY}): " + shape.Contains(123, 123));
        }
        Console.WriteLine("---------------------------");
        var xg = new XGraphicsWrapper();
        foreach (var shape in shapes)
        {
            shape.Draw(xg);
            Console.WriteLine($"Contains({cursorX},{cursorY}): " + shape.Contains(123, 123));
        }
    }

    [Command("level4")]
    public void Level4()
    {
        var shapes = new Level4.IShape[] {
            new Level4.RectangleShape(new Rectangle(100, 100, 150, 150)),
            new Level4.OvalShape(new Rectangle(100, 100, 150, 150)),
            new Level4.OvalShape(new Rectangle(300, 100, 120, 180), new XHitTestStrategy()),
        };
        var cursorX = 123;
        var cursorY = 144;
        var xg = new XGraphicsWrapper();
        foreach (var shape in shapes)
        {
            shape.Draw(xg);
            Console.WriteLine($"Contains({cursorX},{cursorY}): " + shape.Contains(123, 123));
        }
    }

}