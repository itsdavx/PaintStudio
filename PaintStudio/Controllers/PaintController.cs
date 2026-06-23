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
        // -------------------- CAMPOS PRIVADOS --------------------
        private CanvasModel Model;
        private Rasterizer Renderer;
        private PictureBox CanvasView;
        private Shape currentShape;
        private bool isDrawing;
        private bool isResizingShape;
        private bool isMovingShape;
        private ShapeHandle activeHandle = ShapeHandle.None;
        private Shape selectedShape;
        private PointD startPoint;
        private PointD lastMousePoint;
        private bool isMouseInCanvas = false;
        private Point cursorLocation = Point.Empty;
        private Cursor transparentCursor;
        private readonly SelectionManager selectionManager = new SelectionManager();
        private readonly Stack<List<Shape>> undoStack = new Stack<List<Shape>>();
        private readonly Stack<List<Shape>> redoStack = new Stack<List<Shape>>();
        private const int MaxUndoSteps = 40;

        // -------------------- PROPIEDADES PÚBLICAS --------------------
        private ToolMode currentTool = ToolMode.Freehand;
        public ToolMode CurrentTool
        {
            get => currentTool;
            set
            {
                currentTool = value;
                if (CanvasView != null)
                {
                    if (currentTool != ToolMode.Erase) CanvasView.Cursor = Cursors.Default;
                    CanvasView.Invalidate();
                }
            }
        }
        public Color CurrentColor { get; set; } = Color.Black;
        public Color CurrentFillColor { get; set; } = Color.White;
        public bool FillEnabled { get; set; } = false;
        private int currentThickness = 2;
        public int CurrentThickness
        {
            get => currentThickness;
            set
            {
                currentThickness = value;
                if (CanvasView != null && CurrentTool == ToolMode.Erase) CanvasView.Invalidate();
            }
        }
        public double ZoomFactor { get; set; } = 1.0;
        public Shape SelectedShape => selectedShape;
        public SelectionManager Selection => selectionManager;

        // -------------------- EVENTOS --------------------
        public Action<Color> OnColorPicked;
        public Action<Point> OnRequestTextInput;
        public Action OnLayersChanged;
        public Action<bool, bool> OnUndoRedoStateChanged;

        // -------------------- CONSTRUCTOR --------------------
        public PaintController(PictureBox view)
        {
            Model = new CanvasModel();
            CanvasView = view;
            Renderer = new Rasterizer(view.Width, view.Height);
            
            using (Bitmap bmp = new Bitmap(1, 1))
            {
                transparentCursor = new Cursor(bmp.GetHicon());
            }

            CanvasView.MouseDown += CanvasView_MouseDown;
            CanvasView.MouseMove += CanvasView_MouseMove;
            CanvasView.MouseUp += CanvasView_MouseUp;
            CanvasView.Paint += CanvasView_Paint;
            CanvasView.MouseEnter += (s, e) => { isMouseInCanvas = true; if (CurrentTool == ToolMode.Erase) CanvasView.Invalidate(); };
            CanvasView.MouseLeave += (s, e) => { isMouseInCanvas = false; if (CurrentTool == ToolMode.Erase) CanvasView.Invalidate(); };
            Redraw();
        }

        // -------------------- MÉTODOS PÚBLICOS --------------------
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

        public void ApplyTransformation(double[,] matrix)
        {
            if (selectedShape != null)
            {
                selectedShape.ApplyTransformation(matrix);
                Redraw();
            }
        }

        public void MoveLayerUp(int index)
        {
            SaveUndoState();
            Model.MoveShapeUp(index);
            OnLayersChanged?.Invoke();
            Redraw();
        }

        public void MoveLayerDown(int index)
        {
            SaveUndoState();
            Model.MoveShapeDown(index);
            OnLayersChanged?.Invoke();
            Redraw();
        }

        public void ToggleLayerVisibility(int index)
        {
            if (index >= 0 && index < Model.Shapes.Count)
            {
                SaveUndoState();
                Model.Shapes[index].Visible = !Model.Shapes[index].Visible;
                OnLayersChanged?.Invoke();
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
                if (shape.Visible)
                    shape.Draw(Renderer);
            }
            CanvasView.Image = Renderer.Canvas;
            CanvasView.Refresh();
        }

        public void SaveImage(string path)
        {
            var ext = System.IO.Path.GetExtension(path).ToLower();
            var format = System.Drawing.Imaging.ImageFormat.Png;
            if (ext == ".jpg" || ext == ".jpeg") format = System.Drawing.Imaging.ImageFormat.Jpeg;
            else if (ext == ".bmp") format = System.Drawing.Imaging.ImageFormat.Bmp;

            Redraw(); // ensure up-to-date

            // Create a solid white background image to prevent transparent background issues
            using (var bmp = new Bitmap(Renderer.Canvas.Width, Renderer.Canvas.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(Renderer.Canvas, 0, 0);
                }
                bmp.Save(path, format);
            }
        }

        public void ImportImage(string path)
        {
            using (var bmp = new Bitmap(path))
            {
                SaveUndoState();
                if (bmp.Width > Renderer.Canvas.Width || bmp.Height > Renderer.Canvas.Height)
                {
                    ResizeCanvas(Math.Max(bmp.Width, Renderer.Canvas.Width), Math.Max(bmp.Height, Renderer.Canvas.Height));
                }
                Model.AddShape(new ImageShape(new PointD(0, 0), bmp));
                OnLayersChanged?.Invoke();
                Redraw();
            }
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

        // -------------------- DESHACER / REHACER --------------------
        public void SaveUndoState()
        {
            undoStack.Push(CloneShapes());
            redoStack.Clear();
            while (undoStack.Count > MaxUndoSteps)
            {
                var temp = new Stack<List<Shape>>(undoStack);
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
            undoStack.Push(CloneShapes());
            var snapshot = redoStack.Pop();
            Model.SetShapes(snapshot);
            selectedShape = null;
            OnLayersChanged?.Invoke();
            NotifyUndoRedoState();
            Redraw();
        }

        // -------------------- MÉTODOS PRIVADOS --------------------
        private List<Shape> CloneShapes()
        {
            var list = new List<Shape>(Model.Shapes.Count);
            foreach (var s in Model.Shapes)
                list.Add(s?.Clone());
            return list;
        }

        private void NotifyUndoRedoState()
        {
            OnUndoRedoStateChanged?.Invoke(CanUndo, CanRedo);
        }

        private void SelectShapeAt(PointD p)
        {
            selectedShape = null;
            foreach (var s in Model.Shapes) s.Selected = false;
            for (int i = Model.Shapes.Count - 1; i >= 0; i--)
            {
                if (Model.Shapes[i].ContainsPoint(p))
                {
                    selectedShape = Model.Shapes[i];
                    selectedShape.Selected = true;
                    break;
                }
            }
            Redraw();
        }

        private void ResizeSelectedShape(PointD mouse)
        {
            var b = selectedShape.GetBounds();
            double minSize = 4;
            double left = b.Left, top = b.Top, right = b.Right, bottom = b.Bottom;
            double anchorX = left, anchorY = top, targetW = b.Width, targetH = b.Height;

            switch (activeHandle)
            {
                case ShapeHandle.BottomRight: anchorX = left; anchorY = top; targetW = mouse.X - left; targetH = mouse.Y - top; break;
                case ShapeHandle.BottomLeft: anchorX = right; anchorY = top; targetW = right - mouse.X; targetH = mouse.Y - top; break;
                case ShapeHandle.TopRight: anchorX = left; anchorY = bottom; targetW = mouse.X - left; targetH = bottom - mouse.Y; break;
                case ShapeHandle.TopLeft: anchorX = right; anchorY = bottom; targetW = right - mouse.X; targetH = bottom - mouse.Y; break;
                case ShapeHandle.MiddleRight: anchorX = left; targetW = mouse.X - left; break;
                case ShapeHandle.MiddleLeft: anchorX = right; targetW = right - mouse.X; break;
                case ShapeHandle.BottomCenter: anchorY = top; targetH = mouse.Y - top; break;
                case ShapeHandle.TopCenter: anchorY = bottom; targetH = bottom - mouse.Y; break;
                default: return;
            }

            if (targetW < minSize) targetW = minSize;
            if (targetH < minSize) targetH = minSize;

            double sx = b.Width > 0 ? targetW / b.Width : 1;
            double sy = b.Height > 0 ? targetH / b.Height : 1;
            selectedShape.Scale(sx, sy, new PointD(anchorX, anchorY));
        }

        private Cursor CursorForHandle(ShapeHandle h)
        {
            switch (h)
            {
                case ShapeHandle.TopLeft: case ShapeHandle.BottomRight: return Cursors.SizeNWSE;
                case ShapeHandle.TopRight: case ShapeHandle.BottomLeft: return Cursors.SizeNESW;
                case ShapeHandle.MiddleLeft: case ShapeHandle.MiddleRight: return Cursors.SizeWE;
                case ShapeHandle.TopCenter: case ShapeHandle.BottomCenter: return Cursors.SizeNS;
                default: return Cursors.Default;
            }
        }

        // -------------------- EVENTOS DEL RATÓN --------------------
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
                if (selectedShape != null)
                {
                    var handle = selectionManager.HitTestHandles(selectedShape, ZoomFactor, e.Location);
                    if (handle != ShapeHandle.None)
                    {
                        isResizingShape = true;
                        activeHandle = handle;
                        SaveUndoState();
                        return;
                    }
                }
                SelectShapeAt(p);
                if (selectedShape != null)
                {
                    isMovingShape = true;
                    lastMousePoint = p;
                    SaveUndoState();
                }
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
                    poly.AddVertex(p);
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
                    bezier.AddControlPoint(p);
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
                    currentShape = new FreehandShape() { LineColor = Color.White, Thickness = CurrentThickness * 4, IsSquare = true };
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
            cursorLocation = e.Location;
            if (CurrentTool == ToolMode.Erase)
            {
                if (CanvasView.Cursor != transparentCursor) CanvasView.Cursor = transparentCursor;
                CanvasView.Invalidate();
            }

            PointD p = new PointD(e.X / ZoomFactor, e.Y / ZoomFactor);

            if (isResizingShape && selectedShape != null)
            {
                ResizeSelectedShape(p);
                Redraw();
                return;
            }

            if (isMovingShape && selectedShape != null)
            {
                double dx = p.X - lastMousePoint.X;
                double dy = p.Y - lastMousePoint.Y;
                selectedShape.Move(dx, dy);
                lastMousePoint = p;
                Redraw();
                return;
            }

            if (CurrentTool == ToolMode.Select && selectedShape != null && !isDrawing && !isMovingShape)
                CanvasView.Cursor = CursorForHandle(selectionManager.HitTestHandles(selectedShape, ZoomFactor, e.Location));

            if (!isDrawing || currentShape == null) return;

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
            if (isResizingShape)
            {
                isResizingShape = false;
                activeHandle = ShapeHandle.None;
                CanvasView.Cursor = Cursors.Default;
                return;
            }

            if (isMovingShape)
            {
                isMovingShape = false;
                CanvasView.Cursor = Cursors.Default;
                return;
            }

            if (CurrentTool != ToolMode.Polygon && CurrentTool != ToolMode.Bezier)
            {
                isDrawing = false;
                currentShape = null;
            }
        }

        private void CanvasView_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentTool == ToolMode.Erase && isMouseInCanvas)
            {
                int size = (int)(CurrentThickness * 4 * ZoomFactor);
                if (size < 4) size = 4;
                Rectangle rect = new Rectangle(cursorLocation.X - size / 2, cursorLocation.Y - size / 2, size, size);
                e.Graphics.FillRectangle(Brushes.White, rect);
                e.Graphics.DrawRectangle(Pens.Black, rect);
            }
        }
    }
}