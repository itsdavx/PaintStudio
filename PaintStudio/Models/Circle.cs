using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class Circle : Shape
    {
        public Circle(PointD center, PointD edge)
        {
            Vertices.Add(center);
            Vertices.Add(edge);
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 2) return;

            PointD v1 = Vertices[0];
            PointD v2 = Vertices[1];
            Color c = Selected ? Color.Red : LineColor;

            double r = Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2));

            if (IsFilled)
            {
                rasterizer.FillCircle((int)Math.Round(v1.X), (int)Math.Round(v1.Y), (int)Math.Round(r), FillColor);
            }

            rasterizer.DrawCircle((int)Math.Round(v1.X), (int)Math.Round(v1.Y), (int)Math.Round(r), c, Thickness);
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 2) return false;
            double r = Math.Sqrt(Math.Pow(Vertices[1].X - Vertices[0].X, 2) + Math.Pow(Vertices[1].Y - Vertices[0].Y, 2));
            double dist = Math.Sqrt(Math.Pow(p.X - Vertices[0].X, 2) + Math.Pow(p.Y - Vertices[0].Y, 2));
            if (IsFilled) return dist <= r;
            return Math.Abs(dist - r) <= Math.Max(Thickness, 5);
        }
    }
}
