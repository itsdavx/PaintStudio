using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using PaintStudio.Controllers;
using PaintStudio.Models;

namespace PaintStudio
{
    public partial class FrmHome : Form
    {
        // -------------------- CAMPOS PRIVADOS --------------------
        private PaintController controller;
        private NumericUpDown numRot, numScale, numTransX, numTransY;
        private Button btnColor;
        private Button btnFillColorBtn;
        private CheckBox chkFillEnabled;
        private NumericUpDown numThickness;
        private Guna2Button btnMoveUpLayer, btnMoveDownLayer, btnToggleVisibleLayer;
        private ToolTip toolTip1;

        private static readonly Dictionary<string, string> ToolTipTexts = new Dictionary<string, string>
        {
            ["select"] = "Seleccionar objetos",
            ["pencil"] = "Dibujo libre",
            ["bezier"] = "Curva Bézier",
            ["eraser"] = "Borrar trazos",
            ["fill"] = "Rellenar con color",
            ["picker"] = "Tomar color del lienzo",
            ["text"] = "Insertar texto",
            ["line"] = "Dibujar línea",
            ["rect"] = "Dibujar rectángulo",
            ["circle"] = "Dibujar círculo",
            ["polygon"] = "Dibujar polígono",
            ["triangle"] = "Dibujar triángulo",
            ["star"] = "Dibujar estrella",
            ["undo"] = "Deshacer (Ctrl+Z)",
            ["redo"] = "Rehacer (Ctrl+Y)"
        };

        private static readonly Color ColorBackground = ColorTranslator.FromHtml("#121212");
        private static readonly Color ColorSurface = ColorTranslator.FromHtml("#1E1E1E");
        private static readonly Color ColorCard = ColorTranslator.FromHtml("#252526");
        private static readonly Color ColorBorder = ColorTranslator.FromHtml("#333333");
        private static readonly Color ColorAccent = ColorTranslator.FromHtml("#3B82F6");
        private static readonly Color ColorHover = ColorTranslator.FromHtml("#2D2D30");
        private static readonly Color ColorSelected = ColorTranslator.FromHtml("#2563EB");
        private static readonly Color ColorTextSecondary = ColorTranslator.FromHtml("#A0A0A0");

        private List<Guna2Button> sidebarToolButtons = new List<Guna2Button>();
        private const int CardRadius = 16;

        // -------------------- CAMPOS DE REDIMENSIONADO --------------------
        private enum ResizeHandle { None, Right, Bottom, Corner }
        private const int HandleSize = 7;
        private bool isResizingCanvas = false;
        private ResizeHandle activeHandle = ResizeHandle.None;
        private Point resizeStartMouse;
        private Size resizeStartCanvasSize;
        private Size previewCanvasSize;

        // -------------------- CONSTRUCTOR --------------------
        public FrmHome()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        // -------------------- INICIALIZACIÓN --------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            ApplyDarkTitleBar();

            controller = new PaintController(canvasPicBox);
            controller.OnLayersChanged = UpdateLayersList;
            controller.OnUndoRedoStateChanged = (canUndo, canRedo) => {
                btnUndo.Enabled = canUndo;
                btnRedo.Enabled = canRedo;
            };

            this.MinimumSize = new Size(900, 600);
            this.MaximizeBox = true;
            this.WindowState = FormWindowState.Maximized;

            toolTip1 = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 400,
                ReshowDelay = 200,
                ShowAlways = true,
                BackColor = ColorCard,
                ForeColor = Color.White
            };

            // -------------------- CONSTRUCCIÓN DE PANELES --------------------
            BuildSidebar();
            BuildCanvasSizeCard();
            BuildColorAndThickness();
            BuildTransformsPanel();
            BuildLayersHeader();

            var size0 = controller.GetCanvasSize();
            numCanvasWidth.Value = size0.Width;
            numCanvasHeight.Value = size0.Height;

            // -------------------- EVENTOS DE TEXTO --------------------
            controller.OnRequestTextInput = (p) => {
                string input = Prompt.ShowDialog("Ingrese el texto:", "Texto");
                if (!string.IsNullOrEmpty(input))
                {
                    controller.AddTextShape(new PaintStudio.Models.PointD(p.X, p.Y), input, new Font("Segoe UI", 12));
                }
            };

            // -------------------- TECLADO --------------------
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

            // -------------------- COLOR PICKER --------------------
            controller.OnColorPicked = (c) => {
                if (btnColor != null) btnColor.BackColor = c;
                controller.CurrentColor = c;
            };

            // -------------------- CAPAS --------------------
            lstLayers.SelectionMode = SelectionMode.MultiExtended;
            lstLayers.SelectedIndexChanged += (s, args) => {
                if (controller != null && lstLayers.SelectedIndex >= 0)
                {
                    int realIndex = (controller.GetShapes().Count - 1) - lstLayers.SelectedIndex;
                    controller.SelectShapeIndex(realIndex);
                }
            };

            // -------------------- ASIGNACIÓN DE EVENTOS --------------------
            AssignMenuEvents();
            AssignCanvasResizeEvents();
            AssignZoomEvents();
            AssignLayersButtons();
            AssignCanvasAreaEvents();

            btnUndo.Enabled = false;
            btnRedo.Enabled = false;

            CenterCanvas();
            ApplyRoundedPanels();
            SetupFloatingShells();
            SetupStatusBar();
        }

        // -------------------- SIDEBAR IZQUIERDO --------------------
        private void BuildSidebar()
        {
            pnlSidebarCard.Controls.Clear();
            sidebarToolButtons.Clear();

            int colA = 12, colB = 56;
            int y = 10;

            pnlSidebarCard.Controls.Add(SidebarLabel("Selección", y)); y += 18;
            btnSelect = CreateToolButton("select", colA, y, "Selección", () => controller.CurrentTool = ToolMode.Select);
            y += 44;

            y += 4;
            pnlSidebarCard.Controls.Add(SidebarLabel("Dibujo", y)); y += 18;
            btnFreehand = CreateToolButton("pencil", colA, y, "Lápiz", () => controller.CurrentTool = ToolMode.Freehand);
            btnBezier = CreateToolButton("bezier", colB, y, "Bézier", () => controller.CurrentTool = ToolMode.Bezier);
            y += 40;
            btnErase = CreateToolButton("eraser", colA, y, "Borrador", () => controller.CurrentTool = ToolMode.Erase);
            btnFill = CreateToolButton("fill", colB, y, "Relleno", () => controller.CurrentTool = ToolMode.Fill);
            y += 40;
            btnPicker = CreateToolButton("picker", colA, y, "Gotero", () => controller.CurrentTool = ToolMode.Picker);
            btnText = CreateToolButton("text", colB, y, "Texto", () => controller.CurrentTool = ToolMode.Text);
            y += 44;

            y += 4;
            pnlSidebarCard.Controls.Add(SidebarLabel("Formas", y)); y += 18;
            btnLine = CreateToolButton("line", colA, y, "Línea", () => controller.CurrentTool = ToolMode.Line);
            btnRect = CreateToolButton("rect", colB, y, "Rectángulo", () => controller.CurrentTool = ToolMode.Rectangle);
            y += 40;
            btnCircle = CreateToolButton("circle", colA, y, "Círculo", () => controller.CurrentTool = ToolMode.Circle);
            btnPoly = CreateToolButton("polygon", colB, y, "Polígono", () => controller.CurrentTool = ToolMode.Polygon);
            y += 40;
            btnTri = CreateToolButton("triangle", colA, y, "Triángulo", () => controller.CurrentTool = ToolMode.Triangle);
            btnStar = CreateToolButton("star", colB, y, "Estrella", () => controller.CurrentTool = ToolMode.Star);
            y += 44;

            y += 4;
            pnlSidebarCard.Controls.Add(SidebarLabel("Utilidades", y)); y += 18;
            btnUndo = CreateToolButton("undo", colA, y, "Deshacer", () => controller.Undo(), isToolSelector: false);
            btnRedo = CreateToolButton("redo", colB, y, "Rehacer", () => controller.Redo(), isToolSelector: false);
            y += 44;

            pnlSidebarCard.Height = y + 10;
            SetActiveSidebarButton(btnSelect);
        }

        private Label SidebarLabel(string text, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(10, y),
                AutoSize = false,
                Size = new Size(84, 16),
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = ColorTranslator.FromHtml("#6E6E6E"),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private Guna2Button CreateToolButton(string iconKey, int x, int y, string label, Action onClick, bool isToolSelector = true)
        {
            var btn = new Guna2Button
            {
                Location = new Point(x, y),
                Size = new Size(36, 36),
                Text = "",
                BorderRadius = 10,
                FillColor = ColorCard,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btn.HoverState.FillColor = ColorHover;
            btn.Paint += (s, e) => ToolIcons.Draw(e.Graphics, btn.ClientRectangle, iconKey, btn.ForeColor);
            btn.Click += (s, e) => {
                if (isToolSelector)
                {
                    SetActiveSidebarButton(btn);
                    lblStatusTool.Text = "Herramienta: " + label;
                }
                onClick?.Invoke();
            };
            pnlSidebarCard.Controls.Add(btn);

            if (ToolTipTexts.TryGetValue(iconKey, out var tip)) toolTip1.SetToolTip(btn, tip);

            if (isToolSelector) sidebarToolButtons.Add(btn);
            return btn;
        }

        private void SetActiveSidebarButton(Guna2Button btn)
        {
            foreach (var b in sidebarToolButtons)
                b.FillColor = ColorCard;
            btn.FillColor = ColorSelected;
            foreach (var b in sidebarToolButtons)
                b.Invalidate();
        }

        // -------------------- PANEL LIENZO --------------------
        private void BuildCanvasSizeCard()
        {
            pnlCanvasSizeCard.Controls.Clear();

            var title = new Label { Text = "Lienzo", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = ColorAccent, BackColor = Color.Transparent, Location = new Point(12, 10), AutoSize = true };
            pnlCanvasSizeCard.Controls.Add(title);

            pnlCanvasSizeCard.Controls.Add(FieldLabel("Ancho", 12, 38));
            numCanvasWidth = new NumericUpDown { Location = new Point(12, 56), Width = 90, Minimum = 20, Maximum = 10000, Value = 800, BackColor = ColorCard, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            pnlCanvasSizeCard.Controls.Add(numCanvasWidth);
            toolTip1.SetToolTip(numCanvasWidth, "Ancho del lienzo en píxeles");

            pnlCanvasSizeCard.Controls.Add(FieldLabel("Alto", 112, 38));
            numCanvasHeight = new NumericUpDown { Location = new Point(112, 56), Width = 90, Minimum = 20, Maximum = 10000, Value = 600, BackColor = ColorCard, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            pnlCanvasSizeCard.Controls.Add(numCanvasHeight);
            toolTip1.SetToolTip(numCanvasHeight, "Alto del lienzo en píxeles");

            pnlCanvasSizeCard.Controls.Add(FieldLabel("Zoom (%)", 12, 86));
            numZoom = new NumericUpDown { Location = new Point(12, 104), Width = 90, Minimum = 10, Maximum = 500, Value = 100, BackColor = ColorCard, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            pnlCanvasSizeCard.Controls.Add(numZoom);
            toolTip1.SetToolTip(numZoom, "Nivel de zoom del lienzo");

            btnResizeCanvas = new Guna2Button { Text = "Aplicar", Location = new Point(112, 104), Size = new Size(90, 28), BorderRadius = 8, FillColor = ColorAccent, ForeColor = Color.White, Font = new Font("Segoe UI", 8.5F) };
            btnResizeCanvas.HoverState.FillColor = ColorSelected;
            pnlCanvasSizeCard.Controls.Add(btnResizeCanvas);
            toolTip1.SetToolTip(btnResizeCanvas, "Aplicar nuevo tamaño de lienzo");
            pnlCanvasSizeCard.Height = 144;
        }

        private Label FieldLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true, ForeColor = ColorTextSecondary, BackColor = Color.Transparent, Font = new Font("Segoe UI", 8F) };
        }

        // -------------------- PANEL PROPIEDADES --------------------
        private void BuildColorAndThickness()
        {
            pnlPropertiesCard.Controls.Clear();

            var title = new Label { Text = "Propiedades", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = ColorAccent, BackColor = Color.Transparent, Location = new Point(12, 10), AutoSize = true };
            pnlPropertiesCard.Controls.Add(title);

            pnlPropertiesCard.Controls.Add(FieldLabel("Línea", 12, 40));

            btnColor = new Button { BackColor = Color.Black, Size = new Size(22, 22), Location = new Point(70, 36), FlatStyle = FlatStyle.Flat };
            btnColor.FlatAppearance.BorderColor = ColorAccent;
            btnColor.FlatAppearance.BorderSize = 1;
            btnColor.Click += (s, e) => {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    btnColor.BackColor = colorDialog1.Color;
                    controller.CurrentColor = colorDialog1.Color;
                }
            };
            pnlPropertiesCard.Controls.Add(btnColor);
            toolTip1.SetToolTip(btnColor, "Color de línea");

            pnlPropertiesCard.Controls.Add(FieldLabel("Relleno", 110, 40));
            btnFillColorBtn = new Button { BackColor = Color.White, Size = new Size(22, 22), Location = new Point(168, 36), FlatStyle = FlatStyle.Flat };
            btnFillColorBtn.FlatAppearance.BorderColor = ColorAccent;
            btnFillColorBtn.FlatAppearance.BorderSize = 1;
            btnFillColorBtn.Click += (s, e) => {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    btnFillColorBtn.BackColor = colorDialog1.Color;
                    controller.CurrentFillColor = colorDialog1.Color;
                }
            };
            pnlPropertiesCard.Controls.Add(btnFillColorBtn);
            toolTip1.SetToolTip(btnFillColorBtn, "Color de relleno");

            chkFillEnabled = new CheckBox { Text = "Activar relleno", Location = new Point(12, 68), AutoSize = true, ForeColor = Color.White, BackColor = Color.Transparent };
            chkFillEnabled.CheckedChanged += (s, e) => { controller.FillEnabled = chkFillEnabled.Checked; };
            pnlPropertiesCard.Controls.Add(chkFillEnabled);
            toolTip1.SetToolTip(chkFillEnabled, "Aplicar relleno al dibujar");

            pnlPropertiesCard.Controls.Add(FieldLabel("Grosor", 12, 96));
            numThickness = new NumericUpDown { Location = new Point(12, 114), Width = 70, Minimum = 1, Maximum = 30, Value = 2, BackColor = ColorCard, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            numThickness.ValueChanged += (s, e) => { controller.CurrentThickness = (int)numThickness.Value; };
            pnlPropertiesCard.Controls.Add(numThickness);
            toolTip1.SetToolTip(numThickness, "Grosor del trazo");
            pnlPropertiesCard.Height = 150;
        }

        // -------------------- PANEL TRANSFORMACIONES --------------------
        private void BuildTransformsPanel()
        {
            pnlTransforms.Controls.Clear();
            Color labelColor = Color.White;
            Color fieldBg = ColorCard;

            Label title = new Label() { Text = "Transformar", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter, ForeColor = ColorAccent, BackColor = Color.Transparent, Height = 26 };
            pnlTransforms.Controls.Add(title);

            NumericUpDown StyledNumeric(int px, int py) => new NumericUpDown() { Location = new Point(px, py), Width = 100, BackColor = fieldBg, ForeColor = labelColor, BorderStyle = BorderStyle.FixedSingle };
            Label StyledLabel(string text, int px, int py) => new Label() { Text = text, Location = new Point(px, py), AutoSize = true, ForeColor = labelColor, BackColor = Color.Transparent };

            int yPos = 34;
            pnlTransforms.Controls.Add(StyledLabel("Rotación (°):", 10, yPos));
            numRot = StyledNumeric(120, yPos); numRot.Minimum = -360; numRot.Maximum = 360; numRot.Value = 0;
            pnlTransforms.Controls.Add(numRot);
            toolTip1.SetToolTip(numRot, "Rotar figura seleccionada");
            yPos += 30;
            pnlTransforms.Controls.Add(StyledLabel("Escala (%):", 10, yPos));
            numScale = StyledNumeric(120, yPos); numScale.Minimum = 10; numScale.Maximum = 500; numScale.Value = 100;
            pnlTransforms.Controls.Add(numScale);
            toolTip1.SetToolTip(numScale, "Escalar figura seleccionada");
            yPos += 30;
            pnlTransforms.Controls.Add(StyledLabel("Traslación X:", 10, yPos));
            numTransX = StyledNumeric(120, yPos); numTransX.Minimum = -1000; numTransX.Maximum = 1000; numTransX.Value = 0;
            pnlTransforms.Controls.Add(numTransX);
            toolTip1.SetToolTip(numTransX, "Trasladar en X");
            yPos += 30;
            pnlTransforms.Controls.Add(StyledLabel("Traslación Y:", 10, yPos));
            numTransY = StyledNumeric(120, yPos); numTransY.Minimum = -1000; numTransY.Maximum = 1000; numTransY.Value = 0;
            pnlTransforms.Controls.Add(numTransY);
            toolTip1.SetToolTip(numTransY, "Trasladar en Y");
            yPos += 34;
            Button btnApply = new Button() { Text = "Aplicar", Location = new Point(10, yPos), Width = 210, Height = 32, BackColor = ColorAccent, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += BtnApplyTransform_Click;
            pnlTransforms.Controls.Add(btnApply);
            toolTip1.SetToolTip(btnApply, "Aplicar transformaciones a la capa seleccionada");
        }

        // -------------------- CAPAS --------------------
        private void BuildLayersHeader()
        {
            int btnSize = 24;
            int x = Math.Max(0, lblLayersTitle.Width - btnSize - 8);

            btnDeleteLayer = new Guna2Button
            {
                Size = new Size(btnSize, btnSize),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderRadius = 6,
                FillColor = ColorCard,
                ForeColor = Color.White,
                Text = "",
                Cursor = Cursors.Hand,
                Location = new Point(x, 2)
            };
            btnDeleteLayer.HoverState.FillColor = ColorTranslator.FromHtml("#A32D2D");
            btnDeleteLayer.Paint += (s, e) => ToolIcons.Draw(e.Graphics, btnDeleteLayer.ClientRectangle, "trash", Color.White);
            lblLayersTitle.Controls.Add(btnDeleteLayer);
            toolTip1.SetToolTip(btnDeleteLayer, "Eliminar capa(s) seleccionada(s)");

            x -= (btnSize + 4);
            btnMoveDownLayer = CreateLayerTextButton("▼", ColorHover, x);
            lblLayersTitle.Controls.Add(btnMoveDownLayer);
            toolTip1.SetToolTip(btnMoveDownLayer, "Bajar capa");

            x -= (btnSize + 4);
            btnMoveUpLayer = CreateLayerTextButton("▲", ColorHover, x);
            lblLayersTitle.Controls.Add(btnMoveUpLayer);
            toolTip1.SetToolTip(btnMoveUpLayer, "Subir capa");

            x -= (btnSize + 4);
            btnToggleVisibleLayer = CreateLayerTextButton("👁", ColorHover, x);
            lblLayersTitle.Controls.Add(btnToggleVisibleLayer);
            toolTip1.SetToolTip(btnToggleVisibleLayer, "Mostrar/Ocultar capa");

            toolTip1.SetToolTip(lblLayersTitle, "Administración de capas");
            toolTip1.SetToolTip(lstLayers, "Lista de capas del lienzo");
        }

        private Guna2Button CreateLayerTextButton(string text, Color hoverColor, int x)
        {
            var btn = new Guna2Button
            {
                Size = new Size(24, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderRadius = 6,
                FillColor = ColorCard,
                ForeColor = Color.White,
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Location = new Point(x, 2)
            };
            btn.HoverState.FillColor = hoverColor;
            return btn;
        }

        // -------------------- STATUS BAR --------------------
        private void SetupStatusBar()
        {
            var size0 = controller.GetCanvasSize();
            lblStatusCanvas.Text = $"{size0.Width} x {size0.Height} px";
            lblStatusZoom.Text = $"Zoom: {numZoom.Value}%";
            lblStatusTool.Text = "Herramienta: Selección";
            lblStatusCoords.Text = "X: 0  Y: 0";

            canvasPicBox.MouseMove += (s, e) => { lblStatusCoords.Text = $"X: {e.X}  Y: {e.Y}"; };
            numZoom.ValueChanged += (s, e) => { lblStatusZoom.Text = $"Zoom: {numZoom.Value}%"; };
        }

        // -------------------- EVENTOS DE MENÚ --------------------
        private void AssignMenuEvents()
        {
            nuevoToolStripMenuItem.Click += (s, e) => {
                if (MessageBox.Show("¿Limpiar todo el lienzo? Esta acción se puede deshacer.", "Nuevo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    controller.ClearCanvas();
            };

            var abrirImgItem = new ToolStripMenuItem("Abrir Imagen...");
            abrirImgItem.Click += (s, e) => {
                openFileDialog1.Filter = "Archivos de imagen|*.png;*.jpg;*.jpeg;*.bmp|Todos los archivos|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    controller.ImportImage(openFileDialog1.FileName);
                    numCanvasWidth.Value = Clamp(controller.GetCanvasSize().Width, numCanvasWidth.Minimum, numCanvasWidth.Maximum);
                    numCanvasHeight.Value = Clamp(controller.GetCanvasSize().Height, numCanvasHeight.Minimum, numCanvasHeight.Maximum);
                    lblStatusCanvas.Text = $"{controller.GetCanvasSize().Width} x {controller.GetCanvasSize().Height} px";
                    CenterCanvas();
                }
            };
            if (nuevoToolStripMenuItem.Owner != null)
                nuevoToolStripMenuItem.Owner.Items.Insert(1, abrirImgItem);

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
                    lblStatusCanvas.Text = $"{controller.GetCanvasSize().Width} x {controller.GetCanvasSize().Height} px";
                    CenterCanvas();
                }
            };
        }

        // -------------------- EVENTOS DE LIENZO --------------------
        private void AssignCanvasResizeEvents()
        {
            btnResizeCanvas.Click += (s, e) => {
                controller.ResizeCanvas((int)numCanvasWidth.Value, (int)numCanvasHeight.Value);
                CenterCanvas();
                pnlCenter.Invalidate();
                lblStatusCanvas.Text = $"{(int)numCanvasWidth.Value} x {(int)numCanvasHeight.Value} px";
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

        private void AssignLayersButtons()
        {
            btnDeleteLayer.Click += (s, e) => DeleteSelectedLayers();

            btnMoveUpLayer.Click += (s, e) => {
                if (lstLayers.SelectedIndices.Count == 1)
                {
                    int index = lstLayers.SelectedIndex;
                    int realIndex = (controller.GetShapes().Count - 1) - index;
                    if (realIndex < controller.GetShapes().Count - 1)
                    {
                        controller.MoveLayerUp(realIndex);
                        lstLayers.SelectedIndex = index - 1;
                    }
                }
            };

            btnMoveDownLayer.Click += (s, e) => {
                if (lstLayers.SelectedIndices.Count == 1)
                {
                    int index = lstLayers.SelectedIndex;
                    int realIndex = (controller.GetShapes().Count - 1) - index;
                    if (realIndex > 0)
                    {
                        controller.MoveLayerDown(realIndex);
                        lstLayers.SelectedIndex = index + 1;
                    }
                }
            };

            btnToggleVisibleLayer.Click += (s, e) => {
                foreach (int index in lstLayers.SelectedIndices)
                {
                    int realIndex = (controller.GetShapes().Count - 1) - index;
                    controller.ToggleLayerVisibility(realIndex);
                }
            };
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
                string vis = shapes[i].Visible ? "" : " [Oculta]";
                lstLayers.Items.Add($"({i}) {shapes[i].GetType().Name}{vis}");
            }
            if (prevIndex >= 0 && prevIndex < lstLayers.Items.Count)
                lstLayers.SelectedIndex = prevIndex;
        }

        // -------------------- APLICAR TRANSFORMACIÓN --------------------
        private void BtnApplyTransform_Click(object sender, EventArgs e)
        {
            int index = lstLayers.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Seleccione una capa primero.");
                return;
            }
            int realIndex = (controller.GetShapes().Count - 1) - index;
            var shapes = controller.GetShapes();
            var shape = shapes[realIndex];
            var c = shape.GetCentroid();
            double angle = (double)numRot.Value;
            double scale = (double)numScale.Value / 100.0;
            double tx = (double)numTransX.Value;
            double ty = -(double)numTransY.Value;

            if (angle == 0 && scale == 1.0 && tx == 0 && ty == 0) return;

            controller.SaveUndoState();

            if (angle != 0) shape.Rotate(angle, c);
            if (scale != 1.0) shape.Scale(scale, scale, c);
            if (tx != 0 || ty != 0) shape.Move(tx, ty);
            numRot.Value = 0;
            numScale.Value = 100;
            numTransX.Value = 0;
            numTransY.Value = 0;
            controller.Redraw();
        }

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

        // -------------------- ESTILOS VISUALES --------------------
        private void ApplyRoundedPanels()
        {
            void RoundCorners(Control p, int radius)
            {
                void UpdateRegion()
                {
                    using (var path = RoundedRectPath(p.ClientRectangle, radius))
                        p.Region = new Region(path);
                }
                UpdateRegion();
                p.Resize += (s, e) => UpdateRegion();
                p.Paint += (s, e) => {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var path = RoundedRectPath(Rectangle.Inflate(p.ClientRectangle, -1, -1), radius))
                    using (var pen = new Pen(ColorBorder, 1.5f))
                        e.Graphics.DrawPath(pen, path);
                };
            }
            RoundCorners(pnlRight, CardRadius);
            RoundCorners(pnlTransforms, CardRadius);
        }

        private System.Drawing.Drawing2D.GraphicsPath RoundedRectPath(Rectangle r, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(r.X, r.Y, radius, radius, 180, 90);
            path.AddArc(r.Right - radius, r.Y, radius, radius, 270, 90);
            path.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(r.X, r.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void SetupFloatingShells()
        {
            void AttachShadow(Panel shell, Control card)
            {
                shell.Paint += (s, e) => {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    var cardRect = card.Bounds;
                    var layers = new (int offset, int alpha)[] { (5, 12), (3, 22), (2, 35) };
                    foreach (var (offset, alpha) in layers)
                    {
                        var shadowRect = new Rectangle(cardRect.X + offset, cardRect.Y + offset, cardRect.Width, cardRect.Height);
                        using (var path = RoundedRectPath(shadowRect, CardRadius))
                        using (var brush = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0)))
                            e.Graphics.FillPath(brush, path);
                    }
                };
            }
            AttachShadow(pnlRightShell, pnlRight);
        }

        // -------------------- BARRA DE TÍTULO OSCURA --------------------
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private void ApplyDarkTitleBar()
        {
            try
            {
                int useDark = 1;
                DwmSetWindowAttribute(this.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, sizeof(int));
            }
            catch { }
        }

        // -------------------- CLASES INTERNAS --------------------
        internal class DarkToolStripColorTable : System.Windows.Forms.ProfessionalColorTable
        {
            private static readonly Color Bg = ColorTranslator.FromHtml("#1E1E1E");
            private static readonly Color Border = ColorTranslator.FromHtml("#333333");
            private static readonly Color Accent = ColorTranslator.FromHtml("#3B82F6");
            public override Color ToolStripGradientBegin => Bg;
            public override Color ToolStripGradientMiddle => Bg;
            public override Color ToolStripGradientEnd => Bg;
            public override Color MenuStripGradientBegin => Bg;
            public override Color MenuStripGradientEnd => Bg;
            public override Color ImageMarginGradientBegin => Bg;
            public override Color ImageMarginGradientMiddle => Bg;
            public override Color ImageMarginGradientEnd => Bg;
            public override Color ButtonSelectedHighlight => Accent;
            public override Color ButtonSelectedBorder => Accent;
            public override Color MenuItemSelected => Accent;
            public override Color MenuBorder => Border;
            public override Color SeparatorDark => Border;
            public override Color SeparatorLight => Border;
        }

        internal class DarkToolStripRenderer : System.Windows.Forms.ToolStripProfessionalRenderer
        {
            public DarkToolStripRenderer() : base(new DarkToolStripColorTable()) { }
            protected override void OnRenderItemText(System.Windows.Forms.ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = Color.White;
                base.OnRenderItemText(e);
            }
        }

        // -------------------- PROMPT PARA TEXTO --------------------
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

    // -------------------- PANEL CON DOBLE BUFFER --------------------
    internal class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.ResizeRedraw, true);
            UpdateStyles();
        }
    }

    // -------------------- ICONOS VECTORIALES --------------------
    internal static class ToolIcons
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