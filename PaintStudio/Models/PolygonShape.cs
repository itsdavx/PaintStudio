using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class PolygonShape : Shape
    {
        // -------------------- AGREGAR VÉRTICES --------------------
        public void AddVertex(PointD p)
        {
            Vertices.Add(p);
        }

        // -------------------- DIBUJAR POLÍGONO --------------------
        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 2) return;
            Color c = Selected ? Color.Red : LineColor;

            if (IsFilled && Vertices.Count >= 3)
            {
                rasterizer.FillPolygon(Vertices, FillColor);
            }

            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                var v1 = Vertices[i];
                var v2 = Vertices[i + 1];
                rasterizer.DrawLine((int)Math.Round(v1.X), (int)Math.Round(v1.Y),
                                    (int)Math.Round(v2.X), (int)Math.Round(v2.Y), c, Thickness);
            }

            // Cerrar el polígono si tiene más de 2 vértices
            if (Vertices.Count > 2)
            {
                var v1 = Vertices[Vertices.Count - 1];
                var v2 = Vertices[0];
                rasterizer.DrawLine((int)Math.Round(v1.X), (int)Math.Round(v1.Y),
                                    (int)Math.Round(v2.X), (int)Math.Round(v2.Y), c, Thickness);
            }
        }

        // -------------------- VERIFICAR COLISIÓN --------------------
        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 3) return false;
            if (IsFilled && GeometryUtils.PolygonContains(Vertices, p)) return true;

            for (int i = 0; i < Vertices.Count; i++)
            {
                if (GeometryUtils.DistanceToSegment(p, Vertices[i], Vertices[(i + 1) % Vertices.Count]) <= Math.Max(Thickness, 5)) return true;
            }
            return false;
        }
    }
}