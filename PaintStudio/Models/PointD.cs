using System;
using System.Drawing;

namespace PaintStudio.Models
{
    [System.Serializable]
    public struct PointD
    {
        // -------------------- PROPIEDADES --------------------
        public double X { get; set; }
        public double Y { get; set; }

        // -------------------- CONSTRUCTOR --------------------
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        // -------------------- MÉTODOS PÚBLICOS --------------------
        public Point ToPoint()
        {
            return new System.Drawing.Point((int)Math.Round(X), (int)Math.Round(Y));
        }
    }
}