using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PaintStudio
{
    public partial class FrmHome
    {
        // -------------------- CENTRADO DE LIENZO --------------------
        private void AssignCanvasAreaEvents()
        {
            canvasPicBox.Paint += CanvasPicBox_Paint;
            pnlCenter.Resize += (s, e) => CenterCanvas();
            pnlCenter.Paint += PnlCenter_Paint;
            pnlCenter.MouseDown += PnlCenter_MouseDown;
            pnlCenter.MouseMove += PnlCenter_MouseMove;
            pnlCenter.MouseUp += PnlCenter_MouseUp;
            pnlCenter.MouseLeave += (s, e) => { if (!isResizingCanvas) pnlCenter.Cursor = Cursors.Default; };
        }

        private void CenterCanvas()
        {
            int x = Math.Max((pnlCenter.ClientSize.Width - canvasPicBox.Width) / 2, 0);
            int y = Math.Max((pnlCenter.ClientSize.Height - canvasPicBox.Height) / 2, 0);
            var newLoc = new Point(x, y);
            if (canvasPicBox.Location != newLoc)
            {
                canvasPicBox.Location = newLoc;
                pnlCenter.Invalidate();
            }
        }

        // -------------------- MANEJADORES DE REDIMENSIONADO --------------------
        private Dictionary<ResizeHandle, Rectangle> GetHandleRects()
        {
            var b = canvasPicBox.Bounds;
            var dict = new Dictionary<ResizeHandle, Rectangle>
            {
                [ResizeHandle.Right] = new Rectangle(b.Right, b.Top + (b.Height / 2) - HandleSize, HandleSize, HandleSize * 2),
                [ResizeHandle.Bottom] = new Rectangle(b.Left + (b.Width / 2) - HandleSize, b.Bottom, HandleSize * 2, HandleSize),
                [ResizeHandle.Corner] = new Rectangle(b.Right, b.Bottom, HandleSize, HandleSize)
            };
            return dict;
        }

        private void CanvasPicBox_Paint(object sender, PaintEventArgs e)
        {
            controller.Selection.Draw(e.Graphics, controller.SelectedShape, controller.ZoomFactor);
        }

        private void PnlCenter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var canvasRect = canvasPicBox.Bounds;
            using (var shadowPen = new Pen(Color.FromArgb(60, 0, 0, 0), 6))
                e.Graphics.DrawRectangle(shadowPen, Rectangle.Inflate(canvasRect, 3, 3));
            using (var borderPen = new Pen(ColorBorder, 1))
                e.Graphics.DrawRectangle(borderPen, Rectangle.Inflate(canvasRect, 1, 1));

            var handles = GetHandleRects();
            using (var fillBrush = new SolidBrush(Color.White))
            using (var pen = new Pen(Color.DimGray))
            {
                foreach (var r in handles.Values)
                {
                    e.Graphics.FillRectangle(fillBrush, r);
                    e.Graphics.DrawRectangle(pen, r);
                }
            }

            if (isResizingCanvas)
            {
                double zoom = controller.ZoomFactor <= 0 ? 1.0 : controller.ZoomFactor;
                int previewW = (int)(previewCanvasSize.Width * zoom);
                int previewH = (int)(previewCanvasSize.Height * zoom);
                Rectangle previewRect = new Rectangle(canvasPicBox.Left, canvasPicBox.Top, previewW, previewH);
                using (var dashPen = new Pen(Color.DodgerBlue, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    e.Graphics.DrawRectangle(dashPen, previewRect);
                }
            }
        }

        private void PnlCenter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            foreach (var kv in GetHandleRects())
            {
                if (kv.Value.Contains(e.Location))
                {
                    isResizingCanvas = true;
                    activeHandle = kv.Key;
                    resizeStartMouse = e.Location;
                    resizeStartCanvasSize = controller.GetCanvasSize();
                    previewCanvasSize = resizeStartCanvasSize;
                    return;
                }
            }
        }

        private void PnlCenter_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizingCanvas)
            {
                double zoom = controller.ZoomFactor <= 0 ? 1.0 : controller.ZoomFactor;
                int deltaXScreen = e.X - resizeStartMouse.X;
                int deltaYScreen = e.Y - resizeStartMouse.Y;
                int deltaXCanvas = (int)(deltaXScreen / zoom);
                int deltaYCanvas = (int)(deltaYScreen / zoom);

                int newWidth = resizeStartCanvasSize.Width;
                int newHeight = resizeStartCanvasSize.Height;

                if (activeHandle == ResizeHandle.Right || activeHandle == ResizeHandle.Corner)
                    newWidth = Math.Max(20, resizeStartCanvasSize.Width + deltaXCanvas);
                if (activeHandle == ResizeHandle.Bottom || activeHandle == ResizeHandle.Corner)
                    newHeight = Math.Max(20, resizeStartCanvasSize.Height + deltaYCanvas);

                previewCanvasSize = new Size(newWidth, newHeight);
                pnlCenter.Invalidate();
                return;
            }

            var handles = GetHandleRects();
            if (handles[ResizeHandle.Corner].Contains(e.Location)) pnlCenter.Cursor = Cursors.SizeNWSE;
            else if (handles[ResizeHandle.Right].Contains(e.Location)) pnlCenter.Cursor = Cursors.SizeWE;
            else if (handles[ResizeHandle.Bottom].Contains(e.Location)) pnlCenter.Cursor = Cursors.SizeNS;
            else pnlCenter.Cursor = Cursors.Default;
        }

        private void PnlCenter_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isResizingCanvas) return;
            isResizingCanvas = false;
            activeHandle = ResizeHandle.None;
            pnlCenter.Cursor = Cursors.Default;

            controller.ResizeCanvas(previewCanvasSize.Width, previewCanvasSize.Height);
            numCanvasWidth.Value = Clamp(previewCanvasSize.Width, numCanvasWidth.Minimum, numCanvasWidth.Maximum);
            numCanvasHeight.Value = Clamp(previewCanvasSize.Height, numCanvasHeight.Minimum, numCanvasHeight.Maximum);
            lblStatusCanvas.Text = $"{previewCanvasSize.Width} x {previewCanvasSize.Height} px";

            CenterCanvas();
        }

        private decimal Clamp(int value, decimal min, decimal max)
        {
            decimal d = value;
            if (d < min) return min;
            if (d > max) return max;
            return d;
        }
    }
}
