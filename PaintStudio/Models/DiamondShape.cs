using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class DiamondShape : Shape
    {
        public DiamondShape(PointD p1, PointD p2)
        {
            UpdateVertices(p1, p2);
        }

        public void UpdateVertices(PointD p1, PointD p2)
        {
            Vertices.Clear();
            // Rombo dentro del bounding box
            double mx = (p1.X + p2.X) / 2.0;
            double my = (p1.Y + p2.Y) / 2.0;
            Vertices.Add(new PointD(mx, p1.Y)); // Arriba
            Vertices.Add(new PointD(p2.X, my)); // Derecha
            Vertices.Add(new PointD(mx, p2.Y)); // Abajo
            Vertices.Add(new PointD(p1.X, my)); // Izquierda
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 4) return;
            Color c = Selected ? Color.Red : LineColor;

            if (IsFilled)
            {
                rasterizer.FillPolygon(Vertices, FillColor);
            }

            for (int i = 0; i < 4; i++)
            {
                var v1 = Vertices[i];
                var v2 = Vertices[(i + 1) % 4];
                rasterizer.DrawLine((int)Math.Round(v1.X), (int)Math.Round(v1.Y),
                                    (int)Math.Round(v2.X), (int)Math.Round(v2.Y), c, Thickness);
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 4) return false;
            if (IsFilled && GeometryUtils.PolygonContains(Vertices, p)) return true;
            for (int i = 0; i < 4; i++)
                if (GeometryUtils.DistanceToSegment(p, Vertices[i], Vertices[(i + 1) % 4]) <= Math.Max(Thickness, 5)) return true;
            return false;
        }
    }
}
