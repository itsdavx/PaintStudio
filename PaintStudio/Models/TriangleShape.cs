using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class TriangleShape : Shape
    {
        public TriangleShape(PointD p1, PointD p2)
        {
            UpdateVertices(p1, p2);
        }

        public void UpdateVertices(PointD p1, PointD p2)
        {
            Vertices.Clear();
            // Triángulo isósceles dentro del bounding box
            Vertices.Add(new PointD((p1.X + p2.X) / 2.0, p1.Y)); // Punta superior
            Vertices.Add(new PointD(p1.X, p2.Y)); // Esquina inferior izquierda
            Vertices.Add(new PointD(p2.X, p2.Y)); // Esquina inferior derecha
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 3) return;
            Color c = Selected ? Color.Red : LineColor;
            for (int i = 0; i < 3; i++)
            {
                var v1 = Vertices[i];
                var v2 = Vertices[(i + 1) % 3];
                rasterizer.DrawLine((int)Math.Round(v1.X), (int)Math.Round(v1.Y), 
                                    (int)Math.Round(v2.X), (int)Math.Round(v2.Y), c, Thickness);
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 3) return false;
            if (IsFilled && GeometryUtils.PolygonContains(Vertices, p)) return true;
            for (int i = 0; i < 3; i++)
                if (GeometryUtils.DistanceToSegment(p, Vertices[i], Vertices[(i+1)%3]) <= Math.Max(Thickness, 5)) return true;
            return false;
        }
    }
}

