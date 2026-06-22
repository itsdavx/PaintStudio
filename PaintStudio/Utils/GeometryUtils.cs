using System;
using System.Collections.Generic;
using PaintStudio.Models;

namespace PaintStudio.Utils
{
    public static class GeometryUtils
    {
        // -------------------- VERIFICAR POLÍGONO --------------------
        public static bool PolygonContains(List<PointD> poly, PointD p)
        {
            bool result = false;
            int j = poly.Count - 1;
            for (int i = 0; i < poly.Count; i++)
            {
                if ((poly[i].Y < p.Y && poly[j].Y >= p.Y) || (poly[j].Y < p.Y && poly[i].Y >= p.Y))
                {
                    if (poly[i].X + (p.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) * (poly[j].X - poly[i].X) < p.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        // -------------------- DISTANCIA A SEGMENTO --------------------
        public static double DistanceToSegment(PointD p, PointD v, PointD w)
        {
            double l2 = Math.Pow(v.X - w.X, 2) + Math.Pow(v.Y - w.Y, 2);
            if (l2 == 0) return Math.Sqrt(Math.Pow(p.X - v.X, 2) + Math.Pow(p.Y - v.Y, 2));
            double t = Math.Max(0, Math.Min(1, ((p.X - v.X) * (w.X - v.X) + (p.Y - v.Y) * (w.Y - v.Y)) / l2));
            PointD proj = new PointD(v.X + t * (w.X - v.X), v.Y + t * (w.Y - v.Y));
            return Math.Sqrt(Math.Pow(p.X - proj.X, 2) + Math.Pow(p.Y - proj.Y, 2));
        }

        // -------------------- VERIFICAR TRAYECTORIA --------------------
        public static bool PathContains(List<PointD> path, PointD p, double tolerance)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                if (DistanceToSegment(p, path[i], path[i + 1]) <= tolerance) return true;
            }
            return false;
        }
    }
}