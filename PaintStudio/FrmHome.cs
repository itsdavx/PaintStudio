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
    }
}