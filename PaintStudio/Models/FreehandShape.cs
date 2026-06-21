using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class FreehandShape : Shape
    {
        public void AddPoint(PointD p)
        {
            Vertices.Add(p);
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 2) return;
            Color c = Selected ? Color.Red : LineColor;
            
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                var v1 = Vertices[i];
                var v2 = Vertices[i + 1];
                rasterizer.DrawLine((int)Math.Round(v1.X), (int)Math.Round(v1.Y), 
                                    (int)Math.Round(v2.X), (int)Math.Round(v2.Y), c, Thickness);
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 2) return false;
            return GeometryUtils.PathContains(Vertices, p, Math.Max(Thickness, 5));
        }
    }
}

