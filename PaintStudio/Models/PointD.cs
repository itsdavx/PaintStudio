using System;
using System.Drawing;

namespace PaintStudio.Models
{
    /// <summary>
    /// Representa un punto bidimensional con precisión de coma flotante,
    /// necesario para evitar pérdida de datos durante las transformaciones geométricas (Rotación, Escala).
    /// </summary>
    [System.Serializable]
    public struct PointD
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint()
        {
            return new System.Drawing.Point((int)Math.Round(X), (int)Math.Round(Y));
        }
    }
}

