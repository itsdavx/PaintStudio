using System;
using System.Drawing;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    [System.Serializable]
    public class TextShape : Shape
    {
        public string Text { get; set; }
        public Font Font { get; set; }

        public TextShape(PointD p, string text, Font font)
        {
            Vertices.Add(p);
            Text = text;
            Font = font;
        }

        public override void Draw(Rasterizer rasterizer)
        {
            if (Vertices.Count == 0 || string.IsNullOrEmpty(Text)) return;

            // Dibujar el texto directamente sobre el Canvas usando GDI+
            // ya que un rasterizador puro de fuentes es extremadamente complejo.
            using (Graphics g = Graphics.FromImage(rasterizer.Canvas))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                Brush b = new SolidBrush(Selected ? Color.Red : LineColor);
                g.DrawString(Text, Font, b, (float)Vertices[0].X, (float)Vertices[0].Y);
                b.Dispose();
            }
        }

        public override bool ContainsPoint(PointD p)
        {
            if (Vertices.Count == 0 || string.IsNullOrEmpty(Text)) return false;
            // Simplificación: aproximar la caja de texto
            double w = Text.Length * Font.Size;
            double h = Font.Size * 1.5;
            double x = Vertices[0].X;
            double y = Vertices[0].Y;
            return p.X >= x && p.X <= x + w && p.Y >= y && p.Y <= y + h;
        }
    }
}
