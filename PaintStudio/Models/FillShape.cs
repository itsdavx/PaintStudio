using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class FillShape : Shape
    {
        // -------------------- CONSTRUCTOR --------------------
        public FillShape(PointD startPoint, Color fillColor)
        {
            Vertices.Add(startPoint);
            FillColor = fillColor;
        }

        // -------------------- MÉTODOS PÚBLICOS --------------------
        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count > 0)
            {
                rasterizer.FloodFill((int)Vertices[0].X, (int)Vertices[0].Y, FillColor);
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            return false; // Fill no se puede seleccionar ni mover
        }
    }
}