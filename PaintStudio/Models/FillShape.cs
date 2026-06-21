using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    /// <summary>
    /// Representa un comando de relleno en el lienzo.
    /// Al redibujar, ejecuta FloodFill en la coordenada almacenada.
    /// </summary>
    [System.Serializable]
    public class FillShape : Shape
    {
        public FillShape(PointD startPoint, Color fillColor)
        {
            Vertices.Add(startPoint);
            FillColor = fillColor;
        }

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


