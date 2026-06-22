using System;
using System.Collections.Generic;
using System.Drawing;
using PaintStudio.Models;

namespace PaintStudio.Controllers
{
    public enum ShapeHandle
    {
        None, TopLeft, TopCenter, TopRight, MiddleLeft, MiddleRight, BottomLeft, BottomCenter, BottomRight
    }

    public class SelectionManager
    {
        public const int HandleSize = 8;
        private static readonly Color BoxColor = Color.DodgerBlue;
        private static readonly Color HandleFill = Color.White;
        private static readonly Color HandleBorder = Color.DodgerBlue;

        public Dictionary<ShapeHandle, RectangleF> GetHandleRects(RectangleF b)
        {
            float h = HandleSize / 2f;
            float midX = b.X + b.Width / 2f;
            float midY = b.Y + b.Height / 2f;
            return new Dictionary<ShapeHandle, RectangleF>
            {
                [ShapeHandle.TopLeft] = new RectangleF(b.Left - h, b.Top - h, HandleSize, HandleSize),
                [ShapeHandle.TopCenter] = new RectangleF(midX - h, b.Top - h, HandleSize, HandleSize),
                [ShapeHandle.TopRight] = new RectangleF(b.Right - h, b.Top - h, HandleSize, HandleSize),
                [ShapeHandle.MiddleLeft] = new RectangleF(b.Left - h, midY - h, HandleSize, HandleSize),
                [ShapeHandle.MiddleRight] = new RectangleF(b.Right - h, midY - h, HandleSize, HandleSize),
                [ShapeHandle.BottomLeft] = new RectangleF(b.Left - h, b.Bottom - h, HandleSize, HandleSize),
                [ShapeHandle.BottomCenter] = new RectangleF(midX - h, b.Bottom - h, HandleSize, HandleSize),
                [ShapeHandle.BottomRight] = new RectangleF(b.Right - h, b.Bottom - h, HandleSize, HandleSize),
            };
        }

        public void Draw(Graphics g, Shape selectedShape, double zoom)
        {
            if (selectedShape == null) return;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var b = ScaleRect(selectedShape.GetBounds(), zoom);

            using (var boxPen = new Pen(BoxColor, 1f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                g.DrawRectangle(boxPen, b.X, b.Y, b.Width, b.Height);

            using (var fillBrush = new SolidBrush(HandleFill))
            using (var borderPen = new Pen(HandleBorder, 1.2f))
            {
                foreach (var r in GetHandleRects(b).Values)
                {
                    g.FillRectangle(fillBrush, r);
                    g.DrawRectangle(borderPen, r.X, r.Y, r.Width, r.Height);
                }
            }
        }

        private RectangleF ScaleRect(RectangleF r, double zoom)
        {
            return new RectangleF((float)(r.X * zoom), (float)(r.Y * zoom), (float)(r.Width * zoom), (float)(r.Height * zoom));
        }

        public ShapeHandle HitTestHandles(Shape shape, double zoom, Point screenPoint)
        {
            if (shape == null) return ShapeHandle.None;
            var b = ScaleRect(shape.GetBounds(), zoom);
            foreach (var kv in GetHandleRects(b))
            {
                if (RectangleF.Inflate(kv.Value, 3, 3).Contains(screenPoint)) return kv.Key;
            }
            return ShapeHandle.None;
        }
    }
}