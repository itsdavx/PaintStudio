using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using PaintStudio.Controllers;
using PaintStudio.Models;
using PaintStudio.Utils;

namespace PaintStudio
{
    public partial class FrmHome
    {
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
    }
}
