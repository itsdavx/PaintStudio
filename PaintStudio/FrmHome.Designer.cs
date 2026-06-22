using Guna.UI2.WinForms;

namespace PaintStudio
{
    partial class FrmHome
    {
        // -------------------- COMPONENTES --------------------
        private System.ComponentModel.IContainer components = null;

        // -------------------- DESTRUCTOR --------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // -------------------- INICIALIZAR COMPONENTES --------------------
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarImagenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLeftShell = new System.Windows.Forms.Panel();
            this.pnlSidebarCard = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlRightShell = new BufferedPanel();
            this.pnlTransformsSpacer = new System.Windows.Forms.Panel();
            this.pnlCanvasSpacer = new System.Windows.Forms.Panel();
            this.pnlPropSpacer = new System.Windows.Forms.Panel();
            this.pnlRight = new BufferedPanel();
            this.pnlCanvasSizeCard = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlPropertiesCard = new Guna.UI2.WinForms.Guna2Panel();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.lblLayersTitle = new System.Windows.Forms.Label();
            this.pnlTransforms = new BufferedPanel();
            this.lblTransform = new System.Windows.Forms.Label();
            this.pnlCenter = new BufferedPanel();
            this.canvasPicBox = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusCoords = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusCanvas = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusToolSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusTool = new System.Windows.Forms.ToolStripStatusLabel();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.pnlLeftShell.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPicBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            // -------------------- MENU STRIP --------------------
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.menuStrip1.ForeColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1199, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";

            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.guardarProyectoToolStripMenuItem,
            this.cargarProyectoToolStripMenuItem,
            this.exportarImagenToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";

            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.nuevoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";

            // 
            // guardarProyectoToolStripMenuItem
            // 
            this.guardarProyectoToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.guardarProyectoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.guardarProyectoToolStripMenuItem.Name = "guardarProyectoToolStripMenuItem";
            this.guardarProyectoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.guardarProyectoToolStripMenuItem.Text = "Guardar Proyecto (.paintproj)";

            // 
            // cargarProyectoToolStripMenuItem
            // 
            this.cargarProyectoToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.cargarProyectoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.cargarProyectoToolStripMenuItem.Name = "cargarProyectoToolStripMenuItem";
            this.cargarProyectoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.cargarProyectoToolStripMenuItem.Text = "Cargar Proyecto (.paintproj)";

            // 
            // exportarImagenToolStripMenuItem
            // 
            this.exportarImagenToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.exportarImagenToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exportarImagenToolStripMenuItem.Name = "exportarImagenToolStripMenuItem";
            this.exportarImagenToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.exportarImagenToolStripMenuItem.Text = "Exportar Imagen (.png)";

            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.salirToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.salirToolStripMenuItem.Text = "Salir";

            // -------------------- PANEL IZQUIERDO --------------------
            // 
            // pnlLeftShell
            // 
            this.pnlLeftShell.BackColor = System.Drawing.ColorTranslator.FromHtml("#121212");
            this.pnlLeftShell.Controls.Add(this.pnlSidebarCard);
            this.pnlLeftShell.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftShell.Name = "pnlLeftShell";
            this.pnlLeftShell.Padding = new System.Windows.Forms.Padding(16);
            this.pnlLeftShell.Size = new System.Drawing.Size(136, 693);
            this.pnlLeftShell.TabIndex = 4;

            // 
            // pnlSidebarCard
            // 
            this.pnlSidebarCard.BorderRadius = 16;
            this.pnlSidebarCard.FillColor = System.Drawing.ColorTranslator.FromHtml("#252526");
            this.pnlSidebarCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSidebarCard.Name = "pnlSidebarCard";
            this.pnlSidebarCard.Size = new System.Drawing.Size(104, 440);
            this.pnlSidebarCard.ShadowDecoration.Color = System.Drawing.Color.Black;
            this.pnlSidebarCard.ShadowDecoration.Depth = 18;
            this.pnlSidebarCard.ShadowDecoration.Enabled = true;
            this.pnlSidebarCard.TabIndex = 0;

            // -------------------- PANEL DERECHO --------------------
            // 
            // pnlRightShell
            // 
            this.pnlRightShell.BackColor = System.Drawing.ColorTranslator.FromHtml("#121212");
            this.pnlRightShell.Controls.Add(this.pnlRight);
            this.pnlRightShell.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightShell.Name = "pnlRightShell";
            this.pnlRightShell.Padding = new System.Windows.Forms.Padding(18);
            this.pnlRightShell.Size = new System.Drawing.Size(310, 693);
            this.pnlRightShell.TabIndex = 5;

            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.ColorTranslator.FromHtml("#252526");
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);
            this.pnlRight.Controls.Add(this.lstLayers);
            this.pnlRight.Controls.Add(this.lblLayersTitle);
            this.pnlRight.Controls.Add(this.pnlPropSpacer);
            this.pnlRight.Controls.Add(this.pnlPropertiesCard);
            this.pnlRight.Controls.Add(this.pnlCanvasSpacer);
            this.pnlRight.Controls.Add(this.pnlCanvasSizeCard);
            this.pnlRight.Controls.Add(this.pnlTransformsSpacer);
            this.pnlRight.Controls.Add(this.pnlTransforms);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.TabIndex = 2;

            // 
            // pnlCanvasSizeCard
            // 
            this.pnlCanvasSizeCard.BorderRadius = 16;
            this.pnlCanvasSizeCard.FillColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.pnlCanvasSizeCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCanvasSizeCard.Name = "pnlCanvasSizeCard";
            this.pnlCanvasSizeCard.Size = new System.Drawing.Size(254, 144);
            this.pnlCanvasSizeCard.ShadowDecoration.Color = System.Drawing.Color.Black;
            this.pnlCanvasSizeCard.ShadowDecoration.Depth = 14;
            this.pnlCanvasSizeCard.ShadowDecoration.Enabled = true;
            this.pnlCanvasSizeCard.TabIndex = 7;

            // 
            // pnlCanvasSpacer
            // 
            this.pnlCanvasSpacer.BackColor = System.Drawing.ColorTranslator.FromHtml("#252526");
            this.pnlCanvasSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCanvasSpacer.Height = 12;
            this.pnlCanvasSpacer.Name = "pnlCanvasSpacer";
            this.pnlCanvasSpacer.TabIndex = 8;

            // 
            // pnlPropertiesCard
            // 
            this.pnlPropertiesCard.BorderRadius = 16;
            this.pnlPropertiesCard.FillColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.pnlPropertiesCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPropertiesCard.Name = "pnlPropertiesCard";
            this.pnlPropertiesCard.Size = new System.Drawing.Size(254, 150);
            this.pnlPropertiesCard.ShadowDecoration.Color = System.Drawing.Color.Black;
            this.pnlPropertiesCard.ShadowDecoration.Depth = 14;
            this.pnlPropertiesCard.ShadowDecoration.Enabled = true;
            this.pnlPropertiesCard.TabIndex = 9;

            // 
            // pnlPropSpacer
            // 
            this.pnlPropSpacer.BackColor = System.Drawing.ColorTranslator.FromHtml("#252526");
            this.pnlPropSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPropSpacer.Height = 12;
            this.pnlPropSpacer.Name = "pnlPropSpacer";
            this.pnlPropSpacer.TabIndex = 10;

            // 
            // lstLayers
            // 
            this.lstLayers.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.lstLayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLayers.ForeColor = System.Drawing.Color.White;
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.ItemHeight = 17;
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.TabIndex = 1;

            // 
            // lblLayersTitle
            // 
            this.lblLayersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLayersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLayersTitle.ForeColor = System.Drawing.Color.White;
            this.lblLayersTitle.Height = 28;
            this.lblLayersTitle.Name = "lblLayersTitle";
            this.lblLayersTitle.Text = "Capas";
            this.lblLayersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLayersTitle.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);

            // 
            // pnlTransforms
            // 
            this.pnlTransforms.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.pnlTransforms.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTransforms.Name = "pnlTransforms";
            this.pnlTransforms.Size = new System.Drawing.Size(254, 192);
            this.pnlTransforms.TabIndex = 2;

            // 
            // pnlTransformsSpacer
            // 
            this.pnlTransformsSpacer.BackColor = System.Drawing.ColorTranslator.FromHtml("#252526");
            this.pnlTransformsSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTransformsSpacer.Height = 12;
            this.pnlTransformsSpacer.Name = "pnlTransformsSpacer";
            this.pnlTransformsSpacer.TabIndex = 6;

            // 
            // lblTransform
            // 
            this.lblTransform.Location = new System.Drawing.Point(0, 0);
            this.lblTransform.Name = "lblTransform";
            this.lblTransform.Size = new System.Drawing.Size(100, 23);
            this.lblTransform.TabIndex = 0;

            // -------------------- PANEL CENTRAL --------------------
            // 
            // pnlCenter
            // 
            this.pnlCenter.AutoScroll = true;
            this.pnlCenter.BackColor = System.Drawing.ColorTranslator.FromHtml("#121212");
            this.pnlCenter.Controls.Add(this.canvasPicBox);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(136, 24);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(753, 694);
            this.pnlCenter.TabIndex = 3;

            // 
            // canvasPicBox
            // 
            this.canvasPicBox.BackColor = System.Drawing.Color.White;
            this.canvasPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvasPicBox.Location = new System.Drawing.Point(20, 20);
            this.canvasPicBox.Name = "canvasPicBox";
            this.canvasPicBox.Size = new System.Drawing.Size(800, 600);
            this.canvasPicBox.TabIndex = 0;
            this.canvasPicBox.TabStop = false;

            // -------------------- BARRA DE ESTADO --------------------
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.ColorTranslator.FromHtml("#1A1A1A");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusCoords,
            this.lblStatusZoom,
            this.lblStatusCanvas,
            this.lblStatusToolSpring,
            this.lblStatusTool});
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.TabIndex = 6;

            // 
            // lblStatusCoords
            // 
            this.lblStatusCoords.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblStatusCoords.Name = "lblStatusCoords";
            this.lblStatusCoords.Text = "X: 0  Y: 0";
            this.lblStatusCoords.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatusCoords.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;

            // 
            // lblStatusZoom
            // 
            this.lblStatusZoom.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblStatusZoom.Name = "lblStatusZoom";
            this.lblStatusZoom.Text = "Zoom: 100%";
            this.lblStatusZoom.Margin = new System.Windows.Forms.Padding(8, 3, 4, 2);

            // 
            // lblStatusCanvas
            // 
            this.lblStatusCanvas.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblStatusCanvas.Name = "lblStatusCanvas";
            this.lblStatusCanvas.Text = "800 x 600 px";
            this.lblStatusCanvas.Margin = new System.Windows.Forms.Padding(8, 3, 4, 2);

            // 
            // lblStatusToolSpring
            // 
            this.lblStatusToolSpring.Name = "lblStatusToolSpring";
            this.lblStatusToolSpring.Spring = true;
            this.lblStatusToolSpring.Text = "";

            // 
            // lblStatusTool
            // 
            this.lblStatusTool.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblStatusTool.Name = "lblStatusTool";
            this.lblStatusTool.Text = "Herramienta: Selección";

            // -------------------- FORMULARIO PRINCIPAL --------------------
            // 
            // FrmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#121212");
            this.ClientSize = new System.Drawing.Size(1199, 742);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlRightShell);
            this.Controls.Add(this.pnlLeftShell);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paint 2026 - PaintStudio";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlLeftShell.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvasPicBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // -------------------- CONTROLES DECLARADOS --------------------
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarProyectoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarProyectoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarImagenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;

        private System.Windows.Forms.Panel pnlLeftShell;
        private Guna.UI2.WinForms.Guna2Panel pnlSidebarCard;
        private Guna.UI2.WinForms.Guna2Button btnUndo;
        private Guna.UI2.WinForms.Guna2Button btnRedo;
        private Guna.UI2.WinForms.Guna2Button btnDeleteLayer;
        private Guna.UI2.WinForms.Guna2Button btnResizeCanvas;
        private System.Windows.Forms.NumericUpDown numCanvasWidth;
        private System.Windows.Forms.NumericUpDown numCanvasHeight;
        private System.Windows.Forms.NumericUpDown numZoom;
        private Guna.UI2.WinForms.Guna2Button btnSelect;
        private Guna.UI2.WinForms.Guna2Button btnFreehand;
        private Guna.UI2.WinForms.Guna2Button btnBezier;
        private Guna.UI2.WinForms.Guna2Button btnErase;
        private Guna.UI2.WinForms.Guna2Button btnFill;
        private Guna.UI2.WinForms.Guna2Button btnPicker;
        private Guna.UI2.WinForms.Guna2Button btnText;
        private Guna.UI2.WinForms.Guna2Button btnLine;
        private Guna.UI2.WinForms.Guna2Button btnRect;
        private Guna.UI2.WinForms.Guna2Button btnCircle;
        private Guna.UI2.WinForms.Guna2Button btnPoly;
        private Guna.UI2.WinForms.Guna2Button btnTri;
        private Guna.UI2.WinForms.Guna2Button btnStar;

        private BufferedPanel pnlRight;
        private BufferedPanel pnlRightShell;
        private System.Windows.Forms.Panel pnlTransformsSpacer;
        private System.Windows.Forms.Panel pnlCanvasSpacer;
        private System.Windows.Forms.Panel pnlPropSpacer;
        private Guna.UI2.WinForms.Guna2Panel pnlCanvasSizeCard;
        private Guna.UI2.WinForms.Guna2Panel pnlPropertiesCard;
        private System.Windows.Forms.Label lblLayersTitle;
        private System.Windows.Forms.ListBox lstLayers;
        private BufferedPanel pnlTransforms;
        private System.Windows.Forms.Label lblTransform;

        private BufferedPanel pnlCenter;
        private System.Windows.Forms.PictureBox canvasPicBox;

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCoords;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusZoom;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCanvas;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusToolSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusTool;

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}