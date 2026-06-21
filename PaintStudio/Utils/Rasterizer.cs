using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PaintStudio.Utils
{
    /// <summary>
    /// Motor de rasterización propio. Se encarga de dibujar primitivas directamente
    /// sobre un Bitmap utilizando punteros (unsafe) o LockBits para alto rendimiento.
    /// Implementa algoritmos clásicos de computación gráfica.
    /// </summary>
    public class Rasterizer : IDisposable
    {
        public Bitmap Canvas { get; private set; }
        private int Width;
        private int Height;
        
        public Rasterizer(int width, int height)
        {
            Width = width;
            Height = height;
            Canvas = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Clear(Color.White);
        }

        public void Clear(Color color)
        {
            using (Graphics g = Graphics.FromImage(Canvas))
            {
                g.Clear(color);
            }
        }

        /// <summary>
        /// Dibuja un píxel seguro comprobando los límites.
        /// (Nota: Para un motor real se usaría LockBits y punteros, pero SetPixel 
        ///  está bien encapsulado aquí, se puede optimizar después si hay lentitud).
        /// </summary>
        public void SetPixel(int x, int y, Color c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                Canvas.SetPixel(x, y, c);
            }
        }

        private void SetBrushPixel(int x, int y, Color c, int thickness)
        {
            if (thickness <= 1)
            {
                SetPixel(x, y, c);
                return;
            }

            int radius = thickness / 2;
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    if (dx * dx + dy * dy <= radius * radius)
                    {
                        SetPixel(x + dx, y + dy, c);
                    }
                }
            }
        }

        /// <summary>
        /// Algoritmo de Bresenham para dibujar líneas
        /// </summary>
        public void DrawLine(int x0, int y0, int x1, int y1, Color c, int thickness = 1)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                SetBrushPixel(x0, y0, c, thickness);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        /// <summary>
        /// Algoritmo de punto medio / Bresenham para circunferencias
        /// </summary>
        public void DrawCircle(int xc, int yc, int r, Color c, int thickness = 1)
        {
            int x = 0;
            int y = r;
            int d = 3 - 2 * r;

            DrawCirclePoints(xc, yc, x, y, c, thickness);
            while (y >= x)
            {
                x++;
                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                {
                    d = d + 4 * x + 6;
                }
                DrawCirclePoints(xc, yc, x, y, c, thickness);
            }
        }

        private void DrawCirclePoints(int xc, int yc, int x, int y, Color c, int thickness)
        {
            SetBrushPixel(xc + x, yc + y, c, thickness);
            SetBrushPixel(xc - x, yc + y, c, thickness);
            SetBrushPixel(xc + x, yc - y, c, thickness);
            SetBrushPixel(xc - x, yc - y, c, thickness);
            SetBrushPixel(xc + y, yc + x, c, thickness);
            SetBrushPixel(xc - y, yc + x, c, thickness);
            SetBrushPixel(xc + y, yc - x, c, thickness);
            SetBrushPixel(xc - y, yc - x, c, thickness);
        }

        /// <summary>
        /// Algoritmo de relleno FloodFill usando una pila (iterativo) para evitar StackOverflow
        /// </summary>
        public void FloodFill(int x, int y, Color replacementColor)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;

            Color targetColor = Canvas.GetPixel(x, y);
            if (targetColor.ToArgb() == replacementColor.ToArgb()) return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(x, y));

            // Optimización con LockBits para FloodFill
            BitmapData data = Canvas.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = data.Stride;
            
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int bytesPerPixel = 4;
                
                int targetArgb = targetColor.ToArgb();
                int repArgb = replacementColor.ToArgb();

                while (pixels.Count > 0)
                {
                    Point pt = pixels.Pop();
                    int px = pt.X;
                    int py = pt.Y;

                    if (px < 0 || px >= Width || py < 0 || py >= Height) continue;

                    int offset = (py * stride) + (px * bytesPerPixel);
                    int currentColor = *(int*)(ptr + offset);

                    if (currentColor == targetArgb)
                    {
                        *(int*)(ptr + offset) = repArgb;

                        pixels.Push(new Point(px + 1, py));
                        pixels.Push(new Point(px - 1, py));
                        pixels.Push(new Point(px, py + 1));
                        pixels.Push(new Point(px, py - 1));
                    }
                }
            }
            Canvas.UnlockBits(data);
        }

        public void Dispose()
        {
            Canvas?.Dispose();
        }
    }
}
