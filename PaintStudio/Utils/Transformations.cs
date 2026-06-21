using System;

namespace PaintStudio.Utils
{
    /// <summary>
    /// Utilidades para transformaciones geométricas utilizando matrices 2D (coordenadas homogéneas 3x3).
    /// </summary>
    public static class Transformations
    {
        public static double[,] GetTranslationMatrix(double dx, double dy)
        {
            return new double[,] {
                { 1, 0, dx },
                { 0, 1, dy },
                { 0, 0, 1  }
            };
        }

        public static double[,] GetRotationMatrix(double angleDegrees, double cx, double cy)
        {
            double angleRad = angleDegrees * Math.PI / 180.0;
            double cos = Math.Cos(angleRad);
            double sin = Math.Sin(angleRad);

            // Matriz para trasladar al origen, rotar, y trasladar de vuelta
            // [ 1  0  cx ] [ cos -sin 0 ] [ 1  0 -cx ]
            // [ 0  1  cy ] [ sin  cos 0 ] [ 0  1 -cy ]
            // [ 0  0  1  ] [  0    0  1 ] [ 0  0  1  ]
            
            return new double[,] {
                { cos, -sin, -cx * cos + cy * sin + cx },
                { sin,  cos, -cx * sin - cy * cos + cy },
                { 0,    0,   1 }
            };
        }

        public static double[,] GetScaleMatrix(double sx, double sy, double cx, double cy)
        {
            return new double[,] {
                { sx, 0,  cx * (1 - sx) },
                { 0,  sy, cy * (1 - sy) },
                { 0,  0,  1 }
            };
        }
    }
}
