using System;
using System.Drawing;
using System.Collections.Generic;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class BezierShape : Shape
    {
        public int ControlPointsCount => Vertices.Count;

        public BezierShape()
        {
        }

        public void AddControlPoint(PointD p)
        {
            if (Vertices.Count < 4)
            {
                Vertices.Add(p);
            }
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count < 2) return;
            Color c = Selected ? Color.Red : LineColor;

            if (Vertices.Count == 2)
            {
                rasterizer.DrawLine((int)Math.Round(Vertices[0].X), (int)Math.Round(Vertices[0].Y), 
                                    (int)Math.Round(Vertices[1].X), (int)Math.Round(Vertices[1].Y), c, Thickness);
                return;
            }

            // Algoritmo de Bézier Cúbica o Cuadrática
            double step = 0.01;
            PointD prevPoint = Vertices[0];

            for (double t = step; t <= 1.0; t += step)
            {
                PointD p = CalculateBezierPoint(t);
                rasterizer.DrawLine((int)Math.Round(prevPoint.X), (int)Math.Round(prevPoint.Y), 
                                    (int)Math.Round(p.X), (int)Math.Round(p.Y), c, Thickness);
                prevPoint = p;
            }
        }

        private PointD CalculateBezierPoint(double t)
        {
            double u = 1 - t;
            if (Vertices.Count == 3) // Cuadrática
            {
                double tt = t * t;
                double uu = u * u;
                double x = uu * Vertices[0].X + 2 * u * t * Vertices[1].X + tt * Vertices[2].X;
                double y = uu * Vertices[0].Y + 2 * u * t * Vertices[1].Y + tt * Vertices[2].Y;
                return new PointD(x, y);
            }
            else // Cúbica
            {
                double tt = t * t;
                double uu = u * u;
                double uuu = uu * u;
                double ttt = tt * t;

                double x = uuu * Vertices[0].X + 3 * uu * t * Vertices[1].X + 3 * u * tt * Vertices[2].X + ttt * Vertices[3].X;
                double y = uuu * Vertices[0].Y + 3 * uu * t * Vertices[1].Y + 3 * u * tt * Vertices[2].Y + ttt * Vertices[3].Y;
                return new PointD(x, y);
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 2) return false;
            // Aproximación midiendo la curva
            double step = 0.05;
            PointD prevPoint = Vertices[0];
            for (double t = step; t <= 1.0; t += step)
            {
                PointD curr = CalculateBezierPoint(t);
                if (GeometryUtils.DistanceToSegment(p, prevPoint, curr) <= Math.Max(Thickness, 5)) return true;
                prevPoint = curr;
            }
            return false;
        }
    }
}

