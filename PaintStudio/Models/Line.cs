using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class Line : Shape
    {
        public Line(PointD p1, PointD p2)
        {
            Vertices.Add(p1);
            Vertices.Add(p2);
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 2) return;
            Color c = Selected ? Color.Red : LineColor;
            rasterizer.DrawLine((int)Math.Round(Vertices[0].X), (int)Math.Round(Vertices[0].Y), (int)Math.Round(Vertices[1].X), (int)Math.Round(Vertices[1].Y), c, Thickness);
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 2) return false;
            return GeometryUtils.DistanceToSegment(p, Vertices[0], Vertices[1]) <= Math.Max(Thickness, 5);
        }
    }
}


