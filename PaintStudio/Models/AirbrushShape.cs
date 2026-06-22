using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class AirbrushShape : Shape
    {
        // -------------------- CAMPOS --------------------
        private Random rnd = new Random();

        // -------------------- MÉTODOS PÚBLICOS --------------------
        public void AddPoint(PointD p)
        {
            Vertices.Add(p);
        }

        public override void Draw(Rasterizer rasterizer)
        {
            Color c = Selected ? Color.Red : LineColor;
            int radius = Thickness * 2;
            int density = Thickness * 5;

            foreach (var p in Vertices)
            {
                int cx = (int)p.X;
                int cy = (int)p.Y;

                for (int i = 0; i < density; i++)
                {
                    double angle = rnd.NextDouble() * Math.PI * 2;
                    double r = rnd.NextDouble() * radius;

                    int px = cx + (int)(Math.Cos(angle) * r);
                    int py = cy + (int)(Math.Sin(angle) * r);

                    rasterizer.SetPixel(px, py, c);
                }
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count == 0) return false;
            foreach (var v in Vertices)
            {
                if (Math.Sqrt(Math.Pow(p.X - v.X, 2) + Math.Pow(p.Y - v.Y, 2)) <= Thickness * 2) return true;
            }
            return false;
        }
    }
}