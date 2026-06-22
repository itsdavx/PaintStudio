using System;
using System.Collections.Generic;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public abstract class Shape
    {
        // -------------------- PROPIEDADES --------------------
        public List<PointD> Vertices { get; set; }
        public Color LineColor { get; set; }
        public Color FillColor { get; set; }
        public int Thickness { get; set; }
        public bool IsFilled { get; set; }
        public bool Selected { get; set; }

        // -------------------- CONSTRUCTOR --------------------
        public Shape()
        {
            Vertices = new List<PointD>();
            LineColor = Color.Black;
            FillColor = Color.Transparent;
            Thickness = 1;
            IsFilled = false;
            Selected = false;
        }

        // -------------------- CLONAR FIGURA --------------------
        public virtual Shape Clone()
        {
            var copy = (Shape)this.MemberwiseClone();
            var newVerts = new List<PointD>(this.Vertices.Count);
            foreach (var v in this.Vertices) newVerts.Add(new PointD(v.X, v.Y));
            copy.Vertices = newVerts;
            return copy;
        }

        // -------------------- DIBUJAR FIGURA --------------------
        public abstract void Draw(Rasterizer rasterizer);

        // -------------------- APLICAR TRANSFORMACIÓN --------------------
        public void ApplyTransformation(double[,] matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var v = Vertices[i];
                double newX = v.X * matrix[0, 0] + v.Y * matrix[0, 1] + matrix[0, 2];
                double newY = v.X * matrix[1, 0] + v.Y * matrix[1, 1] + matrix[1, 2];
                Vertices[i] = new PointD(newX, newY);
            }
        }

        // -------------------- VERIFICAR COLISIÓN --------------------
        public abstract bool ContainsPoint(PointD p);

        // -------------------- OBTENER CENTROIDE --------------------
        public PointD GetCentroid()
        {
            if (Vertices.Count == 0) return new PointD(0, 0);
            double sumX = 0, sumY = 0;
            foreach (var v in Vertices)
            {
                sumX += v.X;
                sumY += v.Y;
            }
            return new PointD(sumX / Vertices.Count, sumY / Vertices.Count);
        }

        // -------------------- OBTENER LÍMITES --------------------
        public RectangleF GetBounds()
        {
            if (Vertices.Count == 0) return RectangleF.Empty;
            double minX = double.MaxValue, minY = double.MaxValue, maxX = double.MinValue, maxY = double.MinValue;
            foreach (var v in Vertices)
            {
                if (v.X < minX) minX = v.X;
                if (v.Y < minY) minY = v.Y;
                if (v.X > maxX) maxX = v.X;
                if (v.Y > maxY) maxY = v.Y;
            }
            return new RectangleF((float)minX, (float)minY, (float)(maxX - minX), (float)(maxY - minY));
        }
    }
}