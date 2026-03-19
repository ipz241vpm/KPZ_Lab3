using System;

// РЕНДЕРЕР (реалізація) 
interface IRenderer
{
    void Render(string shapeName);
}

// Векторний рендер
class VectorRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as vectors");
    }
}

// Растровий рендер
class RasterRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as pixels");
    }
}

// SHAPE (абстракція) 
abstract class Shape
{
    protected IRenderer renderer;

    public Shape(IRenderer renderer)
    {
        this.renderer = renderer;
    }

    public abstract void Draw();
}

// КОНКРЕТНІ ФІГУРИ 
class Circle : Shape
{
    public Circle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Circle");
    }
}

class Square : Shape
{
    public Square(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Square");
    }
}

class Triangle : Shape
{
    public Triangle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Triangle");
    }
}

class Program
{
    static void Main()
    {
        // Різні рендери
        IRenderer vector = new VectorRenderer();
        IRenderer raster = new RasterRenderer();

        // Комбінації
        Shape circle = new Circle(vector);
        Shape square = new Square(raster);
        Shape triangle = new Triangle(raster);

        circle.Draw();
        square.Draw();
        triangle.Draw();
    }
}