using System;

namespace PaintStudio.Utils
{
    public static class Transformations
    {
        // -------------------- MATRIZ DE TRASLACIÓN --------------------
        public static double[,] GetTranslationMatrix(double dx, double dy)
        {
            return new double[,] {
                { 1, 0, dx },
                { 0, 1, dy },
                { 0, 0, 1  }
            };
        }

        // -------------------- MATRIZ DE ROTACIÓN --------------------
        public static double[,] GetRotationMatrix(double angleDegrees, double cx, double cy)
        {
            double angleRad = angleDegrees * Math.PI / 180.0;
            double cos = Math.Cos(angleRad);
            double sin = Math.Sin(angleRad);

            return new double[,] {
                { cos, -sin, -cx * cos + cy * sin + cx },
                { sin,  cos, -cx * sin - cy * cos + cy },
                { 0,    0,   1 }
            };
        }

        // -------------------- MATRIZ DE ESCALADO --------------------
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