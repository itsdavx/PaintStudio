using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using PaintStudio.Utils;
using System.Collections.Generic;

namespace PaintStudio.Models
{
    [Serializable]
    public class ImageShape : Shape
    {
        public byte[] ImageData { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        [NonSerialized]
        private Bitmap cachedBitmap;

        public ImageShape() { }

        public ImageShape(PointD position, Bitmap bmp)
        {
            Vertices = new List<PointD> { position, new PointD(position.X + bmp.Width, position.Y + bmp.Height) };
            ImageWidth = bmp.Width;
            ImageHeight = bmp.Height;
            
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ImageData = ms.ToArray();
            }
        }

        private Bitmap GetBitmap()
        {
            if (cachedBitmap == null && ImageData != null)
            {
                using (var ms = new System.IO.MemoryStream(ImageData))
                {
                    cachedBitmap = new Bitmap(ms);
                }
            }
            return cachedBitmap;
        }

        public override void Draw(Rasterizer rasterizer)
        {
            var bmp = GetBitmap();
            if (bmp == null || Vertices.Count < 1) return;

            int x = (int)Math.Round(Vertices[0].X);
            int y = (int)Math.Round(Vertices[0].Y);

            // Simple drawing of the image onto the rasterizer's canvas
            using (Graphics g = Graphics.FromImage(rasterizer.Canvas))
            {
                g.DrawImage(bmp, x, y);
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count < 2) return false;
            double minX = Math.Min(Vertices[0].X, Vertices[1].X);
            double maxX = Math.Max(Vertices[0].X, Vertices[1].X);
            double minY = Math.Min(Vertices[0].Y, Vertices[1].Y);
            double maxY = Math.Max(Vertices[0].Y, Vertices[1].Y);
            return p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY;
        }

        public override Shape Clone()
        {
            var copy = (ImageShape)base.Clone();
            if (this.ImageData != null)
            {
                copy.ImageData = (byte[])this.ImageData.Clone();
            }
            return copy;
        }
    }
}
