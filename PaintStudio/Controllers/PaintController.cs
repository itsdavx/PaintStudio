using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using PaintStudio.Models;
using PaintStudio.Utils;

namespace PaintStudio.Controllers
{
    public enum ToolMode
    {
        Line, Circle, Rectangle, Polygon, Freehand, Erase, Fill, Select, Picker, Text, Airbrush,
        Triangle, RightTriangle, Diamond, Star, Bezier
    }

    public class PaintController
    {
        private CanvasModel Model;
        private Rasterizer Renderer;
        private PictureBox CanvasView;

        public ToolMode CurrentTool { get; set; } = ToolMode.Freehand;
        public Color CurrentColor { get; set; } = Color.Black;
        public int CurrentThickness { get; set; } = 2;
        public double ZoomFactor { get; set; } = 1.0;

        public Action<Color> OnColorPicked;
        public Action<Point> OnRequestTextInput;
        public Action OnLayersChanged;

        private Shape currentShape;
        private bool isDrawing;
        private Shape selectedShape;
        private PointD startPoint;

        public PaintController(PictureBox view)
        {
            Model = new CanvasModel();
            CanvasView = view;
            Renderer = new Rasterizer(view.Width, view.Height);
            
            CanvasView.MouseDown += CanvasView_MouseDown;
            CanvasView.MouseMove += CanvasView_MouseMove;
            CanvasView.MouseUp += CanvasView_MouseUp;
            
            Redraw();
        }

        public IReadOnlyList<Shape> GetShapes() => Model.Shapes;
        
        public void SelectShapeIndex(int index)
        {
            foreach (var s in Model.Shapes) s.Selected = false;
            selectedShape = null;
            if (index >= 0 && index < Model.Shapes.Count)
            {
                selectedShape = Model.Shapes[index];
                selectedShape.Selected = true;
            }
            Redraw();
        }

        public void RemoveShapeAt(int index)
        {
            Model.RemoveShapeAt(index);
            selectedShape = null;
            OnLayersChanged?.Invoke();
            Redraw();
        }
        
        private void SelectShapeAt(PointD p)
        {
            selectedShape = null;
            foreach (var s in Model.Shapes)
            {
                s.Selected = false;
            }

            // Recorrer de atrás hacia adelante (capas superiores primero)
            for (int i = Model.Shapes.Count - 1; i >= 0; i--)
            {
                if (Model.Shapes[i].ContainsPoint(p))
                {
                    selectedShape = Model.Shapes[i];
                    selectedShape.Selected = true;
                    // También podemos disparar un evento para que Form1 seleccione en el ListBox, pero es opcional.
                    break;
                }
            }
            Redraw();
        }

        private void CanvasView_MouseDown(object sender, MouseEventArgs e)
        {
            PointD p = new PointD(e.X / ZoomFactor, e.Y / ZoomFactor);

            if (CurrentTool == ToolMode.Picker)
            {
                if (p.X >= 0 && p.X < Renderer.Canvas.Width && p.Y >= 0 && p.Y < Renderer.Canvas.Height)
                {
                    Color picked = Renderer.Canvas.GetPixel((int)p.X, (int)p.Y);
                    OnColorPicked?.Invoke(picked);
                }
                return;
            }

            if (CurrentTool == ToolMode.Text)
            {
                OnRequestTextInput?.Invoke(new Point((int)p.X, (int)p.Y));
                return;
            }

            if (CurrentTool == ToolMode.Select)
            {
                SelectShapeAt(p);
                return;
            }

            if (CurrentTool == ToolMode.Fill)
            {
                Model.AddShape(new FillShape(p, CurrentColor));
                OnLayersChanged?.Invoke();
                Redraw();
                return;
            }

            if (CurrentTool == ToolMode.Polygon && isDrawing && currentShape is PolygonShape poly)
            {
                if (e.Button == MouseButtons.Right)
                {
                    isDrawing = false;
                    currentShape = null;
                }
                else
                {
                    poly.AddVertex(p);
                }
                Redraw();
                return;
            }

            if (CurrentTool == ToolMode.Bezier && isDrawing && currentShape is BezierShape bezier)
            {
                if (e.Button == MouseButtons.Right || bezier.ControlPointsCount >= 4)
                {
                    isDrawing = false;
                    currentShape = null;
                }
                else
                {
                    bezier.AddControlPoint(p);
                }
                Redraw();
                return;
            }

            isDrawing = true;
            startPoint = p;

            switch (CurrentTool)
            {
                case ToolMode.Line:
                    currentShape = new Line(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness };
                    break;
                case ToolMode.Circle:
                    currentShape = new Circle(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness };
                    break;
                case ToolMode.Rectangle:
                    currentShape = new RectShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness };
                    break;
                case ToolMode.Triangle:
                    currentShape = new TriangleShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness };
                    break;
                case ToolMode.RightTriangle:
                    currentShape = new RightTriangleShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness };
                    break;
                case ToolMode.Diamond:
                    currentShape = new DiamondShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness };
                    break;
                case ToolMode.Star:
                    currentShape = new StarShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness };
                    break;
                case ToolMode.Polygon:
                    currentShape = new PolygonShape() { LineColor = CurrentColor, Thickness = CurrentThickness };
                    ((PolygonShape)currentShape).AddVertex(p);
                    ((PolygonShape)currentShape).AddVertex(p);
                    break;
                case ToolMode.Bezier:
                    currentShape = new BezierShape() { LineColor = CurrentColor, Thickness = CurrentThickness };
                    ((BezierShape)currentShape).AddControlPoint(p);
                    ((BezierShape)currentShape).AddControlPoint(p);
                    break;
                case ToolMode.Freehand:
                    currentShape = new FreehandShape() { LineColor = CurrentColor, Thickness = CurrentThickness };
                    ((FreehandShape)currentShape).AddPoint(p);
                    break;
                case ToolMode.Airbrush:
                    currentShape = new AirbrushShape() { LineColor = CurrentColor, Thickness = CurrentThickness };
                    ((AirbrushShape)currentShape).AddPoint(p);
                    break;
                case ToolMode.Erase:
                    currentShape = new FreehandShape() { LineColor = Color.White, Thickness = CurrentThickness * 4 };
                    ((FreehandShape)currentShape).AddPoint(p);
                    break;
            }

            if (currentShape != null)
            {
                Model.AddShape(currentShape);
                OnLayersChanged?.Invoke();
            }
            Redraw();
        }

        private void CanvasView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || currentShape == null) return;

            PointD p = new PointD(e.X / ZoomFactor, e.Y / ZoomFactor);

            switch (CurrentTool)
            {
                case ToolMode.Line:
                case ToolMode.Circle:
                    currentShape.Vertices[1] = p;
                    break;
                case ToolMode.Rectangle:
                    var startP = currentShape.Vertices[0];
                    currentShape.Vertices[1] = new PointD(p.X, startP.Y);
                    currentShape.Vertices[2] = new PointD(p.X, p.Y);
                    currentShape.Vertices[3] = new PointD(startP.X, p.Y);
                    break;
                case ToolMode.Triangle:
                    ((TriangleShape)currentShape).UpdateVertices(startPoint, p);
                    break;
                case ToolMode.RightTriangle:
                    ((RightTriangleShape)currentShape).UpdateVertices(startPoint, p);
                    break;
                case ToolMode.Diamond:
                    ((DiamondShape)currentShape).UpdateVertices(startPoint, p);
                    break;
                case ToolMode.Star:
                    ((StarShape)currentShape).UpdateVertices(startPoint, p);
                    break;
                case ToolMode.Polygon:
                case ToolMode.Bezier:
                    currentShape.Vertices[currentShape.Vertices.Count - 1] = p;
                    break;
                case ToolMode.Freehand:
                case ToolMode.Erase:
                    ((FreehandShape)currentShape).AddPoint(p);
                    break;
                case ToolMode.Airbrush:
                    ((AirbrushShape)currentShape).AddPoint(p);
                    break;
            }

            Redraw();
        }

        private void CanvasView_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentTool != ToolMode.Polygon && CurrentTool != ToolMode.Bezier)
            {
                isDrawing = false;
                currentShape = null;
            }
        }

        public void ApplyTransformation(double[,] matrix)
        {
            if (selectedShape != null)
            {
                selectedShape.ApplyTransformation(matrix);
                Redraw();
            }
        }

        public void ClearCanvas()
        {
            Model.Clear();
            Redraw();
        }

        public void AddTextShape(PointD p, string text, Font font)
        {
            Model.AddShape(new TextShape(p, text, font));
            Redraw();
        }

        private void Redraw()
        {
            Renderer.Clear(Color.White);
            foreach (var shape in Model.Shapes)
            {
                shape.Draw(Renderer);
            }
            CanvasView.Image = Renderer.Canvas;
            CanvasView.Refresh();
        }

        public void SaveImage(string path)
        {
            Renderer.Canvas.Save(path);
        }
        
        public void SaveProject(string path)
        {
            Model.SaveToFile(path);
        }

        public void LoadProject(string path)
        {
            Model.LoadFromFile(path);
            Redraw();
            OnLayersChanged?.Invoke();
        }
        
        public void ResizeCanvas(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                Renderer?.Dispose();
                Renderer = new Rasterizer(width, height);
                Redraw();
            }
        }
    }
}
