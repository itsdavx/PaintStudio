using System;
using System.Collections.Generic;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    /// <summary>
    /// Clase abstracta base para todas las figuras geométricas.
    /// Define las propiedades y métodos comunes.
    /// </summary>
    [System.Serializable]
    public abstract class Shape
    {
        public List<PointD> Vertices { get; set; }
        public Color LineColor { get; set; }
        public Color FillColor { get; set; }
        public int Thickness { get; set; }
        public bool IsFilled { get; set; }
        
        public bool Selected { get; set; }

        public Shape()
        {
            Vertices = new List<PointD>();
            LineColor = Color.Black;
            FillColor = Color.Transparent;
            Thickness = 1;
            IsFilled = false;
            Selected = false;
        }

        /// <summary>
        /// Método polimórfico para dibujar la figura utilizando el motor de rasterización.
        /// </summary>
        /// <param name="rasterizer">Motor de dibujo a nivel de píxel</param>
        public abstract void Draw(Rasterizer rasterizer);

        /// <summary>
        /// Aplica una matriz de transformación a todos los vértices de la figura.
        /// </summary>
        /// <param name="matrix">Matriz de transformación 3x3</param>
        public void ApplyTransformation(double[,] matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var v = Vertices[i];
                // Multiplicación de la matriz por el vector [x, y, 1]
                double newX = v.X * matrix[0, 0] + v.Y * matrix[0, 1] + matrix[0, 2];
                double newY = v.X * matrix[1, 0] + v.Y * matrix[1, 1] + matrix[1, 2];
                Vertices[i] = new PointD(newX, newY);
            }
        }

        public abstract bool ContainsPoint(PointD p);

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
    }
}


