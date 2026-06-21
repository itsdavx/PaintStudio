using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class StarShape : Shape
    {
        public StarShape(PointD p1, PointD p2)
        {
            UpdateVertices(p1, p2);
        }

        public void UpdateVertices(PointD p1, PointD p2)
        {
            Vertices.Clear();
            double cx = (p1.X + p2.X) / 2.0;
            double cy = (p1.Y + p2.Y) / 2.0;
            double w = Math.Abs(p2.X - p1.X);
            double h = Math.Abs(p2.Y - p1.Y);
            double rOuter = Math.Min(w, h) / 2.0;
            double rInner = rOuter * 0.4; // Estrella de 5 puntas estándar

            for (int i = 0; i < 10; i++)
            {
                double r = (i % 2 == 0) ? rOuter : rInner;
                double a = Math.PI / 2.0 - i * Math.PI / 5.0; // Inicia arriba y gira a la derecha
                Vertices.Add(new PointD(cx + Math.Cos(a) * r, cy - Math.Sin(a) * r));
            }
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 10) return;
            Color c = Selected ? Color.Red : LineColor;
            for (int i = 0; i < 10; i++)
            {
                var v1 = Vertices[i];
                var v2 = Vertices[(i + 1) % 10];
                rasterizer.DrawLine((int)Math.Round(v1.X), (int)Math.Round(v1.Y), 
                                    (int)Math.Round(v2.X), (int)Math.Round(v2.Y), c, Thickness);
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 10) return false;
            if (IsFilled && GeometryUtils.PolygonContains(Vertices, p)) return true;
            for (int i = 0; i < 10; i++)
                if (GeometryUtils.DistanceToSegment(p, Vertices[i], Vertices[(i+1)%10]) <= Math.Max(Thickness, 5)) return true;
            return false;
        }
    }
}

