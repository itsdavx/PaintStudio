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
        public Color CurrentFillColor { get; set; } = Color.White;
        public bool FillEnabled { get; set; } = false;
        public int CurrentThickness { get; set; } = 2;
        public double ZoomFactor { get; set; } = 1.0;

        public Action<Color> OnColorPicked;
        public Action<Point> OnRequestTextInput;
        public Action OnLayersChanged;
        public Action<bool, bool> OnUndoRedoStateChanged; // (canUndo, canRedo)

        private Shape currentShape;
        private bool isDrawing;
        private Shape selectedShape;
        private PointD startPoint;

        private readonly Stack<System.Collections.Generic.List<Shape>> undoStack = new Stack<System.Collections.Generic.List<Shape>>();
        private readonly Stack<System.Collections.Generic.List<Shape>> redoStack = new Stack<System.Collections.Generic.List<Shape>>();
        private const int MaxUndoSteps = 40;

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

        // ---------------- UNDO / REDO ----------------
        // Se usa un Memento simple: se serializa una "foto" de la lista de figuras
        // antes de cada operación que modifica el lienzo (nueva figura, borrado,
        // transformación, limpieza, redimensionado). Se reutiliza el mismo mecanismo
        // de serialización binaria que ya usa CanvasModel para guardar proyectos.
        // Crear una copia profunda de la lista de figuras usando Shape.Clone()
        private System.Collections.Generic.List<Shape> CloneShapes()
        {
            var list = new System.Collections.Generic.List<Shape>(Model.Shapes.Count);
            foreach (var s in Model.Shapes)
            {
                list.Add(s?.Clone());
            }
            return list;
        }

        public void SaveUndoState()
        {
            // Guardar una copia profunda en la pila de undo. Más rápido que serializar a binario.
            undoStack.Push(CloneShapes());
            redoStack.Clear();
            while (undoStack.Count > MaxUndoSteps)
            {
                // Quita el snapshot más antiguo para no crecer indefinidamente en memoria
                var temp = new Stack<System.Collections.Generic.List<Shape>>(undoStack);
                undoStack.Clear();
                temp.Pop();
                while (temp.Count > 0) undoStack.Push(temp.Pop());
            }
            NotifyUndoRedoState();
        }

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        public void Undo()
        {
            if (undoStack.Count == 0) return;
            // Guardar estado actual en redo
            redoStack.Push(CloneShapes());
            var snapshot = undoStack.Pop();
            Model.SetShapes(snapshot);
            selectedShape = null;
            OnLayersChanged?.Invoke();
            NotifyUndoRedoState();
            Redraw();
        }

        public void Redo()
        {
            if (redoStack.Count == 0) return;
            // Guardar estado actual en undo
            undoStack.Push(CloneShapes());
            var snapshot = redoStack.Pop();
            Model.SetShapes(snapshot);
            selectedShape = null;
            OnLayersChanged?.Invoke();
            NotifyUndoRedoState();
            Redraw();
        }

        private void NotifyUndoRedoState()
        {
            OnUndoRedoStateChanged?.Invoke(CanUndo, CanRedo);
        }
        // ------------------------------------------------

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
            SaveUndoState();
            Model.RemoveShapeAt(index);
            selectedShape = null;
            OnLayersChanged?.Invoke();
            Redraw();
        }

        public void RemoveShapesAt(IEnumerable<int> indices)
        {
            SaveUndoState();
            Model.RemoveShapesAt(indices);
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
                SaveUndoState();
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
                    currentShape = new Circle(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness, IsFilled = FillEnabled, FillColor = CurrentFillColor };
                    break;
                case ToolMode.Rectangle:
                    currentShape = new RectShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness, IsFilled = FillEnabled, FillColor = CurrentFillColor };
                    break;
                case ToolMode.Triangle:
                    currentShape = new TriangleShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness, IsFilled = FillEnabled, FillColor = CurrentFillColor };
                    break;
                case ToolMode.RightTriangle:
                    currentShape = new RightTriangleShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness, IsFilled = FillEnabled, FillColor = CurrentFillColor };
                    break;
                case ToolMode.Diamond:
                    currentShape = new DiamondShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness, IsFilled = FillEnabled, FillColor = CurrentFillColor };
                    break;
                case ToolMode.Star:
                    currentShape = new StarShape(p, p) { LineColor = CurrentColor, Thickness = CurrentThickness, IsFilled = FillEnabled, FillColor = CurrentFillColor };
                    break;
                case ToolMode.Polygon:
                    currentShape = new PolygonShape() { LineColor = CurrentColor, Thickness = CurrentThickness, IsFilled = FillEnabled, FillColor = CurrentFillColor };
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
                SaveUndoState();
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
            SaveUndoState();
            Model.Clear();
            selectedShape = null;
            OnLayersChanged?.Invoke();
            Redraw();
        }

        public void AddTextShape(PointD p, string text, Font font)
        {
            SaveUndoState();
            Model.AddShape(new TextShape(p, text, font));
            OnLayersChanged?.Invoke();
            Redraw();
        }

        public void Redraw()
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
            undoStack.Clear();
            redoStack.Clear();
            NotifyUndoRedoState();
            selectedShape = null;
            Redraw();
            OnLayersChanged?.Invoke();
        }

        public void ResizeCanvas(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                SaveUndoState();
                Renderer?.Dispose();
                Renderer = new Rasterizer(width, height);
                // Antes solo se recreaba el Rasterizer (bitmap) pero el PictureBox
                // conservaba su tamaño anterior, quedando el lienzo visual desincronizado.
                CanvasView.Width = (int)(width * ZoomFactor);
                CanvasView.Height = (int)(height * ZoomFactor);
                CanvasView.SizeMode = PictureBoxSizeMode.StretchImage;
                Redraw();
            }
        }

        public Size GetCanvasSize() => new Size(Renderer.Canvas.Width, Renderer.Canvas.Height);

        public void SetZoom(double zoom)
        {
            if (zoom <= 0) return;
            ZoomFactor = zoom;
            CanvasView.Width = (int)(Renderer.Canvas.Width * zoom);
            CanvasView.Height = (int)(Renderer.Canvas.Height * zoom);
            CanvasView.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
