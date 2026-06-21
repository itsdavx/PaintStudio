using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PaintStudio.Controllers;
using PaintStudio.Models;
namespace PaintStudio
{
    public partial class FrmHome : Form
    {
        private PaintController controller;
        private NumericUpDown numRot, numScale, numTransX, numTransY;
        private Button btnColor;
        private Button btnFillColorBtn;
        private CheckBox chkFillEnabled;
        private NumericUpDown numThickness;

        // --- Redimensionado del lienzo con mouse (estilo Paint) ---
        private enum ResizeHandle { None, Right, Bottom, Corner }
        private const int HandleSize = 7;
        private bool isResizingCanvas = false;
        private ResizeHandle activeHandle = ResizeHandle.None;
        private Point resizeStartMouse;
        private Size resizeStartCanvasSize;
        private Size previewCanvasSize;

        public FrmHome()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            controller = new PaintController(canvasPicBox);
            controller.OnLayersChanged = UpdateLayersList;
            controller.OnUndoRedoStateChanged = (canUndo, canRedo) => {
                btnUndo.Enabled = canUndo;
                btnRedo.Enabled = canRedo;
            };

            // Ventana responsiva / maximizable
            this.MinimumSize = new Size(900, 600);
            this.MaximizeBox = true;
            this.WindowState = FormWindowState.Maximized;

            // Sincroniza el numeric de tamaño de lienzo con el tamaño real inicial
            var size0 = controller.GetCanvasSize();
            numCanvasWidth.Value = size0.Width;
            numCanvasHeight.Value = size0.Height;

            // Text tool
            controller.OnRequestTextInput = (p) => {
                string input = Prompt.ShowDialog("Ingrese el texto:", "Texto");
                if (!string.IsNullOrEmpty(input))
                {
                    controller.AddTextShape(new PaintStudio.Models.PointD(p.X, p.Y), input, new Font("Segoe UI", 12));
                }
            };
            // Keyboard delete / undo / redo shortcuts
            this.KeyPreview = true;
            this.KeyDown += (s, args) => {
                if (args.KeyCode == Keys.Delete && lstLayers.SelectedIndices.Count > 0)
                {
                    DeleteSelectedLayers();
                }
                if (args.KeyCode == Keys.Escape)
                {
                    lstLayers.ClearSelected();
                    controller.SelectShapeIndex(-1);
                }
                if (args.Control && args.KeyCode == Keys.Z)
                {
                    controller.Undo();
                }
                if (args.Control && args.KeyCode == Keys.Y)
                {
                    controller.Redo();
                }
            };
            // Color picker
            controller.OnColorPicked = (c) => {
                if (btnColor != null) btnColor.BackColor = c;
                controller.CurrentColor = c;
            };
            // Multi-selección de capas
            lstLayers.SelectionMode = SelectionMode.MultiExtended;
            lstLayers.SelectedIndexChanged += (s, args) => {
                if (controller != null && lstLayers.SelectedIndex >= 0)
                {
                    int realIndex = (controller.GetShapes().Count - 1) - lstLayers.SelectedIndex;
                    controller.SelectShapeIndex(realIndex);
                }
            };
            BuildTransformsPanel();
            BuildColorAndThickness();
            AssignToolEvents();
            AssignMenuEvents();
            AssignUndoRedoEvents();
            AssignCanvasResizeEvents();
            AssignZoomEvents();
            AssignDeleteLayersButton();
            AssignCanvasAreaEvents();

            btnUndo.Enabled = false;
            btnRedo.Enabled = false;

            // Posiciona el lienzo centrado por primera vez
            CenterCanvas();
        }
        private void BuildTransformsPanel()
        {
            pnlTransforms.Controls.Clear();
            Label title = new Label() { Text = "Transformaciones", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
            pnlTransforms.Controls.Add(title);
            int y = 30;
            pnlTransforms.Controls.Add(new Label() { Text = "Rotación (°):", Location = new Point(10, y), AutoSize = true });
            numRot = new NumericUpDown() { Location = new Point(120, y), Width = 100, Minimum = -360, Maximum = 360, Value = 0 };
            pnlTransforms.Controls.Add(numRot);
            y += 30;
            pnlTransforms.Controls.Add(new Label() { Text = "Escala (%):", Location = new Point(10, y), AutoSize = true });
            numScale = new NumericUpDown() { Location = new Point(120, y), Width = 100, Minimum = 10, Maximum = 500, Value = 100 };
            pnlTransforms.Controls.Add(numScale);
            y += 30;
            pnlTransforms.Controls.Add(new Label() { Text = "Traslación X:", Location = new Point(10, y), AutoSize = true });
            numTransX = new NumericUpDown() { Location = new Point(120, y), Width = 100, Minimum = -1000, Maximum = 1000, Value = 0 };
            pnlTransforms.Controls.Add(numTransX);
            y += 30;
            pnlTransforms.Controls.Add(new Label() { Text = "Traslación Y:", Location = new Point(10, y), AutoSize = true });
            numTransY = new NumericUpDown() { Location = new Point(120, y), Width = 100, Minimum = -1000, Maximum = 1000, Value = 0 };
            pnlTransforms.Controls.Add(numTransY);
            y += 30;
            Button btnApply = new Button() { Text = "Aplicar", Location = new Point(10, y), Width = 210, Height = 30 };
            btnApply.Click += BtnApplyTransform_Click;
            pnlTransforms.Controls.Add(btnApply);
        }
        private void BuildColorAndThickness()
        {
            ToolStripControlHost hostColorLabel = new ToolStripControlHost(new Label() { Text = " Línea:", AutoSize = true });
            btnColor = new Button() { BackColor = Color.Black, Size = new Size(20, 20), FlatStyle = FlatStyle.Flat };
            btnColor.Click += (s, e) => {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    btnColor.BackColor = colorDialog1.Color;
                    controller.CurrentColor = colorDialog1.Color;
                }
            };
            ToolStripControlHost hostColorBtn = new ToolStripControlHost(btnColor);

            ToolStripControlHost hostFillLabel = new ToolStripControlHost(new Label() { Text = " Relleno:", AutoSize = true });
            btnFillColorBtn = new Button() { BackColor = Color.White, Size = new Size(20, 20), FlatStyle = FlatStyle.Flat };
            btnFillColorBtn.Click += (s, e) => {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    btnFillColorBtn.BackColor = colorDialog1.Color;
                    controller.CurrentFillColor = colorDialog1.Color;
                }
            };
            ToolStripControlHost hostFillBtn = new ToolStripControlHost(btnFillColorBtn);

            chkFillEnabled = new CheckBox() { Text = "Activar relleno", AutoSize = true };
            chkFillEnabled.CheckedChanged += (s, e) => { controller.FillEnabled = chkFillEnabled.Checked; };
            ToolStripControlHost hostFillChk = new ToolStripControlHost(chkFillEnabled);

            ToolStripControlHost hostThickLabel = new ToolStripControlHost(new Label() { Text = " Grosor:", AutoSize = true });
            numThickness = new NumericUpDown() { Minimum = 1, Maximum = 30, Value = 2, Width = 50 };
            numThickness.ValueChanged += (s, e) => { controller.CurrentThickness = (int)numThickness.Value; };
            ToolStripControlHost hostThickNum = new ToolStripControlHost(numThickness);

            toolStrip1.Items.Add(new ToolStripSeparator());
            toolStrip1.Items.Add(hostColorLabel);
            toolStrip1.Items.Add(hostColorBtn);
            toolStrip1.Items.Add(hostFillLabel);
            toolStrip1.Items.Add(hostFillBtn);
            toolStrip1.Items.Add(hostFillChk);
            toolStrip1.Items.Add(hostThickLabel);
            toolStrip1.Items.Add(hostThickNum);
        }
        private void AssignToolEvents()
        {
            btnSelect.Click += (s, e) => controller.CurrentTool = ToolMode.Select;
            btnFreehand.Click += (s, e) => controller.CurrentTool = ToolMode.Freehand;
            btnBezier.Click += (s, e) => controller.CurrentTool = ToolMode.Bezier;
            btnLine.Click += (s, e) => controller.CurrentTool = ToolMode.Line;
            btnCircle.Click += (s, e) => controller.CurrentTool = ToolMode.Circle;
            btnRect.Click += (s, e) => controller.CurrentTool = ToolMode.Rectangle;
            btnPoly.Click += (s, e) => controller.CurrentTool = ToolMode.Polygon;
            btnTri.Click += (s, e) => controller.CurrentTool = ToolMode.Triangle;
            btnStar.Click += (s, e) => controller.CurrentTool = ToolMode.Star;
            btnErase.Click += (s, e) => controller.CurrentTool = ToolMode.Erase;
            btnFill.Click += (s, e) => controller.CurrentTool = ToolMode.Fill;
            btnPicker.Click += (s, e) => controller.CurrentTool = ToolMode.Picker;
            btnText.Click += (s, e) => controller.CurrentTool = ToolMode.Text;
        }
        private void AssignMenuEvents()
        {
            nuevoToolStripMenuItem.Click += (s, e) => {
                if (MessageBox.Show("¿Limpiar todo el lienzo? Esta acción se puede deshacer.", "Nuevo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    controller.ClearCanvas();
            };
            salirToolStripMenuItem.Click += (s, e) => Application.Exit();
            guardarProyectoToolStripMenuItem.Click += (s, e) => {
                saveFileDialog1.Filter = "Paint Project|*.paintproj";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    controller.SaveProject(saveFileDialog1.FileName);
                }
            };
            exportarImagenToolStripMenuItem.Click += (s, e) => {
                saveFileDialog1.Filter = "PNG Image|*.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    controller.SaveImage(saveFileDialog1.FileName);
                }
            };
            cargarProyectoToolStripMenuItem.Click += (s, e) => {
                openFileDialog1.Filter = "Paint Project|*.paintproj";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    controller.LoadProject(openFileDialog1.FileName);
                    numCanvasWidth.Value = Clamp(controller.GetCanvasSize().Width, numCanvasWidth.Minimum, numCanvasWidth.Maximum);
                    numCanvasHeight.Value = Clamp(controller.GetCanvasSize().Height, numCanvasHeight.Minimum, numCanvasHeight.Maximum);
                    CenterCanvas();
                }
            };
        }
        private void AssignUndoRedoEvents()
        {
            btnUndo.Click += (s, e) => controller.Undo();
            btnRedo.Click += (s, e) => controller.Redo();
        }
        private void AssignCanvasResizeEvents()
        {
            btnResizeCanvas.Click += (s, e) => {
                controller.ResizeCanvas((int)numCanvasWidth.Value, (int)numCanvasHeight.Value);
                CenterCanvas();
                pnlCenter.Invalidate();
            };
        }
        private void AssignZoomEvents()
        {
            numZoom.ValueChanged += (s, e) => {
                controller.SetZoom((double)numZoom.Value / 100.0);
                CenterCanvas();
                pnlCenter.Invalidate();
            };
        }
        private void AssignDeleteLayersButton()
        {
            btnDeleteLayer.Click += (s, e) => DeleteSelectedLayers();
        }
        private void DeleteSelectedLayers()
        {
            if (lstLayers.SelectedIndices.Count == 0) return;
            int shapesCount = controller.GetShapes().Count;
            var realIndices = new List<int>();
            foreach (int visibleIndex in lstLayers.SelectedIndices)
            {
                realIndices.Add((shapesCount - 1) - visibleIndex);
            }
            controller.RemoveShapesAt(realIndices);
        }
        private void UpdateLayersList()
        {
            int prevIndex = lstLayers.SelectedIndex;
            lstLayers.Items.Clear();
            var shapes = controller.GetShapes();
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                lstLayers.Items.Add($"({i}) {shapes[i].GetType().Name}");
            }
            if (prevIndex >= 0 && prevIndex < lstLayers.Items.Count)
                lstLayers.SelectedIndex = prevIndex;
        }
        private void BtnApplyTransform_Click(object sender, EventArgs e)
        {
            int index = lstLayers.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Seleccione una capa primero.");
                return;
            }
            // Debido a que invertimos el orden visual, el índice real es:
            int realIndex = (controller.GetShapes().Count - 1) - index;
            var shapes = controller.GetShapes();
            var shape = shapes[realIndex];
            var c = shape.GetCentroid();
            double angle = (double)numRot.Value;
            double scale = (double)numScale.Value / 100.0;
            double tx = (double)numTransX.Value;
            double ty = (double)numTransY.Value;

            if (angle == 0 && scale == 1.0 && tx == 0 && ty == 0) return;

            // Se guarda un único snapshot de Undo para todo el conjunto de transformaciones aplicadas
            controller.SaveUndoState();

            if (angle != 0) shape.ApplyTransformation(PaintStudio.Utils.Transformations.GetRotationMatrix(angle, c.X, c.Y));
            if (scale != 1.0) shape.ApplyTransformation(PaintStudio.Utils.Transformations.GetScaleMatrix(scale, scale, c.X, c.Y));
            if (tx != 0 || ty != 0) shape.ApplyTransformation(PaintStudio.Utils.Transformations.GetTranslationMatrix(tx, ty));
            numRot.Value = 0;
            numScale.Value = 100;
            numTransX.Value = 0;
            numTransY.Value = 0;
            // Forzar el re-dibujo inmediato usando el controlador (más fiable que solo Invalidate)
            controller.Redraw();
        }

        // ==================================================================
        // Centrado del lienzo en el área de trabajo + redimensionado con mouse
        // (estilo Paint: manijas en el borde derecho, inferior y esquina)
        // ==================================================================
        private void AssignCanvasAreaEvents()
        {
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
            canvasPicBox.Location = new Point(x, y);
            pnlCenter.Invalidate();
        }

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

        private void PnlCenter_Paint(object sender, PaintEventArgs e)
        {
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

            // Retroalimentación visual: cambia el cursor al pasar sobre una manija
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

            CenterCanvas();
        }

        private decimal Clamp(int value, decimal min, decimal max)
        {
            decimal d = value;
            if (d < min) return min;
            if (d > max) return max;
            return d;
        }
        // ==================================================================

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 400,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 20, Top = 20, Text = text, AutoSize = true };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
                Button confirmation = new Button() { Text = "Ok", Left = 260, Width = 100, Top = 80, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;
                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }
    }
}
