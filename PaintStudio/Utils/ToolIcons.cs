using System;
using System.Drawing;

namespace PaintStudio.Utils
{
    // -------------------- ICONOS VECTORIALES --------------------
    public static class ToolIcons
    {
        public static void Draw(Graphics g, Rectangle bounds, string key, Color fallbackColor)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var r = Rectangle.Inflate(bounds, -(int)(bounds.Width * 0.2f), -(int)(bounds.Height * 0.2f));
            float cx = bounds.Left + bounds.Width / 2f;
            float cy = bounds.Top + bounds.Height / 2f;
            float w = r.Width;
            float h = r.Height;

            using (var pen = new Pen(Color.Black, 1.5f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round })
            using (var brush = new SolidBrush(Color.Black))
            {
                switch (key)
                {
                    case "select":
                        PointF[] cursorPts = {
                            new PointF(r.Left + w*0.2f, r.Top),
                            new PointF(r.Left + w*0.2f, r.Bottom - h*0.1f),
                            new PointF(r.Left + w*0.4f, r.Bottom - h*0.3f),
                            new PointF(r.Left + w*0.6f, r.Bottom + h*0.1f),
                            new PointF(r.Left + w*0.75f, r.Bottom),
                            new PointF(r.Left + w*0.55f, r.Bottom - h*0.4f),
                            new PointF(r.Right, r.Bottom - h*0.4f)
                        };
                        g.FillPolygon(Brushes.White, cursorPts);
                        g.DrawPolygon(pen, cursorPts);
                        break;

                    case "pencil":
                        var st = g.Save();
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(45);
                        g.TranslateTransform(-cx, -cy);
                        // Eraser
                        g.FillRectangle(Brushes.LightPink, cx - w*0.15f, cy - h*0.4f, w*0.3f, h*0.2f);
                        g.DrawRectangle(pen, cx - w*0.15f, cy - h*0.4f, w*0.3f, h*0.2f);
                        // Metal
                        g.FillRectangle(Brushes.Silver, cx - w*0.15f, cy - h*0.2f, w*0.3f, h*0.1f);
                        g.DrawRectangle(pen, cx - w*0.15f, cy - h*0.2f, w*0.3f, h*0.1f);
                        // Wood body
                        g.FillRectangle(Brushes.Gold, cx - w*0.15f, cy - h*0.1f, w*0.3f, h*0.4f);
                        g.DrawRectangle(pen, cx - w*0.15f, cy - h*0.1f, w*0.3f, h*0.4f);
                        // Cone
                        PointF[] cone = { new PointF(cx - w*0.15f, cy + h*0.3f), new PointF(cx + w*0.15f, cy + h*0.3f), new PointF(cx, cy + h*0.5f) };
                        g.FillPolygon(Brushes.BurlyWood, cone);
                        g.DrawPolygon(pen, cone);
                        // Tip
                        PointF[] tip = { new PointF(cx - w*0.05f, cy + h*0.43f), new PointF(cx + w*0.05f, cy + h*0.43f), new PointF(cx, cy + h*0.5f) };
                        g.FillPolygon(Brushes.Black, tip);
                        g.Restore(st);
                        break;

                    case "fill":
                        var stF = g.Save();
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(-25);
                        g.TranslateTransform(-cx, -cy);
                        // Paint spilling
                        g.FillEllipse(Brushes.DodgerBlue, cx - w*0.4f, cy + h*0.1f, w*0.6f, h*0.4f);
                        PointF[] spill = { new PointF(cx - w*0.3f, cy), new PointF(cx + w*0.1f, cy), new PointF(cx - w*0.1f, cy + h*0.5f) };
                        g.FillPolygon(Brushes.DodgerBlue, spill);
                        // Bucket
                        g.FillRectangle(Brushes.Silver, cx - w*0.2f, cy - h*0.3f, w*0.5f, h*0.5f);
                        g.DrawRectangle(pen, cx - w*0.2f, cy - h*0.3f, w*0.5f, h*0.5f);
                        // Handle
                        g.DrawArc(pen, cx - w*0.3f, cy - h*0.4f, w*0.7f, h*0.3f, 180, 180);
                        // Bucket top
                        g.FillEllipse(Brushes.Gray, cx - w*0.2f, cy - h*0.35f, w*0.5f, h*0.1f);
                        g.DrawEllipse(pen, cx - w*0.2f, cy - h*0.35f, w*0.5f, h*0.1f);
                        g.Restore(stF);
                        break;

                    case "eraser":
                        // 3D Eraser
                        PointF[] topFace = { new PointF(cx - w*0.3f, cy), new PointF(cx, cy - h*0.3f), new PointF(cx + w*0.4f, cy - h*0.1f), new PointF(cx + w*0.1f, cy + h*0.2f) };
                        PointF[] leftFace = { new PointF(cx - w*0.3f, cy), new PointF(cx + w*0.1f, cy + h*0.2f), new PointF(cx + w*0.1f, cy + h*0.4f), new PointF(cx - w*0.3f, cy + h*0.2f) };
                        PointF[] rightFace = { new PointF(cx + w*0.1f, cy + h*0.2f), new PointF(cx + w*0.4f, cy - h*0.1f), new PointF(cx + w*0.4f, cy + h*0.1f), new PointF(cx + w*0.1f, cy + h*0.4f) };
                        g.FillPolygon(Brushes.White, topFace);
                        g.DrawPolygon(pen, topFace);
                        g.FillPolygon(Brushes.LightPink, leftFace);
                        g.DrawPolygon(pen, leftFace);
                        g.FillPolygon(Brushes.HotPink, rightFace);
                        g.DrawPolygon(pen, rightFace);
                        break;

                    case "picker":
                        var stP = g.Save();
                        g.TranslateTransform(cx, cy);
                        g.RotateTransform(45);
                        g.TranslateTransform(-cx, -cy);
                        // Bulb
                        g.FillRectangle(Brushes.DimGray, cx - w*0.15f, cy - h*0.4f, w*0.3f, h*0.2f);
                        g.DrawRectangle(pen, cx - w*0.15f, cy - h*0.4f, w*0.3f, h*0.2f);
                        g.FillEllipse(Brushes.DimGray, cx - w*0.15f, cy - h*0.5f, w*0.3f, h*0.2f);
                        g.DrawEllipse(pen, cx - w*0.15f, cy - h*0.5f, w*0.3f, h*0.2f);
                        // Glass
                        g.FillRectangle(Brushes.LightCyan, cx - w*0.1f, cy - h*0.2f, w*0.2f, h*0.4f);
                        g.DrawRectangle(pen, cx - w*0.1f, cy - h*0.2f, w*0.2f, h*0.4f);
                        // Tip
                        PointF[] tipP = { new PointF(cx - w*0.1f, cy + h*0.2f), new PointF(cx + w*0.1f, cy + h*0.2f), new PointF(cx, cy + h*0.4f) };
                        g.FillPolygon(Brushes.LightCyan, tipP);
                        g.DrawPolygon(pen, tipP);
                        g.Restore(stP);
                        break;

                    case "text":
                        using (var f = new Font("Arial Black", h, FontStyle.Bold))
                        {
                            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                            g.DrawString("A", f, Brushes.RoyalBlue, cx, cy + 2, sf);
                        }
                        break;

                    case "bezier":
                        pen.Color = fallbackColor;
                        g.DrawBezier(pen, r.Left, r.Bottom, r.Left + w * 0.3f, r.Top - h * 0.2f, r.Right - w * 0.3f, r.Bottom + h * 0.2f, r.Right, r.Top);
                        break;

                    case "line":
                        pen.Color = fallbackColor;
                        g.DrawLine(pen, r.Left, r.Bottom, r.Right, r.Top);
                        break;

                    case "rect":
                        pen.Color = fallbackColor;
                        g.DrawRectangle(pen, r.Left, r.Top, w, h);
                        break;

                    case "circle":
                        pen.Color = fallbackColor;
                        g.DrawEllipse(pen, r.Left, r.Top, w, h);
                        break;

                    case "polygon":
                        pen.Color = fallbackColor;
                        g.DrawPolygon(pen, HexPoints(r));
                        break;

                    case "triangle":
                        pen.Color = fallbackColor;
                        g.DrawPolygon(pen, new[] { new PointF(cx, r.Top), new PointF(r.Left, r.Bottom), new PointF(r.Right, r.Bottom) });
                        break;

                    case "star":
                        pen.Color = fallbackColor;
                        g.DrawPolygon(pen, StarPoints(r));
                        break;

                    case "undo":
                        using (var arrowPen = new Pen(Color.RoyalBlue, 2.5f))
                        {
                            arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 4, true);
                            g.DrawArc(arrowPen, r.Left + w * 0.1f, r.Top + h * 0.2f, w * 0.8f, h * 0.6f, 20, -200);
                        }
                        break;

                    case "redo":
                        using (var arrowPen = new Pen(Color.RoyalBlue, 2.5f))
                        {
                            arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 4, true);
                            g.DrawArc(arrowPen, r.Left + w * 0.1f, r.Top + h * 0.2f, w * 0.8f, h * 0.6f, 160, 200);
                        }
                        break;

                    case "trash":
                        pen.Color = fallbackColor;
                        g.DrawLine(pen, r.Left, r.Top + h * 0.2f, r.Right, r.Top + h * 0.2f);
                        g.DrawRectangle(pen, r.Left + w * 0.2f, r.Top + h * 0.2f, w * 0.6f, h * 0.8f);
                        g.DrawLine(pen, r.Left + w * 0.35f, r.Top, r.Left + w * 0.65f, r.Top);
                        g.DrawLine(pen, r.Left + w * 0.35f, r.Top, r.Left + w * 0.35f, r.Top + h * 0.2f);
                        g.DrawLine(pen, r.Left + w * 0.65f, r.Top, r.Left + w * 0.65f, r.Top + h * 0.2f);
                        break;
                }
            }
        }

        private static PointF[] HexPoints(Rectangle r)
        {
            var pts = new PointF[6];
            double cx = r.Left + r.Width / 2.0, cy = r.Top + r.Height / 2.0;
            for (int i = 0; i < 6; i++)
            {
                double angle = Math.PI / 3 * i - Math.PI / 2;
                pts[i] = new PointF((float)(cx + r.Width / 2.0 * Math.Cos(angle)), (float)(cy + r.Height / 2.0 * Math.Sin(angle)));
            }
            return pts;
        }

        private static PointF[] StarPoints(Rectangle r)
        {
            var pts = new PointF[10];
            double cx = r.Left + r.Width / 2.0, cy = r.Top + r.Height / 2.0;
            double outerR = r.Width / 2.0, innerR = outerR * 0.45;
            for (int i = 0; i < 10; i++)
            {
                double angle = Math.PI / 5 * i - Math.PI / 2;
                double radius = (i % 2 == 0) ? outerR : innerR;
                pts[i] = new PointF((float)(cx + radius * Math.Cos(angle)), (float)(cy + radius * Math.Sin(angle)));
            }
            return pts;
        }
    }
}
