namespace PaintStudio
{
    partial class FrmHome
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarImagenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUndo = new System.Windows.Forms.ToolStripButton();
            this.btnRedo = new System.Windows.Forms.ToolStripButton();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFreehand = new System.Windows.Forms.ToolStripButton();
            this.btnBezier = new System.Windows.Forms.ToolStripButton();
            this.btnErase = new System.Windows.Forms.ToolStripButton();
            this.btnFill = new System.Windows.Forms.ToolStripButton();
            this.btnPicker = new System.Windows.Forms.ToolStripButton();
            this.btnText = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLine = new System.Windows.Forms.ToolStripButton();
            this.btnRect = new System.Windows.Forms.ToolStripButton();
            this.btnCircle = new System.Windows.Forms.ToolStripButton();
            this.btnPoly = new System.Windows.Forms.ToolStripButton();
            this.btnTri = new System.Windows.Forms.ToolStripButton();
            this.btnStar = new System.Windows.Forms.ToolStripButton();
            this.numCanvasWidth = new System.Windows.Forms.NumericUpDown();
            this.numCanvasHeight = new System.Windows.Forms.NumericUpDown();
            this.numZoom = new System.Windows.Forms.NumericUpDown();
            this.btnResizeCanvas = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteLayer = new System.Windows.Forms.ToolStripButton();
            this.pnlLeftShell = new System.Windows.Forms.Panel();
            this.pnlRightShell = new System.Windows.Forms.Panel();
            this.pnlTransformsSpacer = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.lblLayersTitle = new System.Windows.Forms.Label();
            this.pnlTransforms = new System.Windows.Forms.Panel();
            this.lblTransform = new System.Windows.Forms.Label();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.canvasPicBox = new System.Windows.Forms.PictureBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCanvasWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCanvasHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZoom)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
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
            this.archivoToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
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
            this.nuevoToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.nuevoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            // 
            // guardarProyectoToolStripMenuItem
            // 
            this.guardarProyectoToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.guardarProyectoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.guardarProyectoToolStripMenuItem.Name = "guardarProyectoToolStripMenuItem";
            this.guardarProyectoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.guardarProyectoToolStripMenuItem.Text = "Guardar Proyecto (.paintproj)";
            // 
            // cargarProyectoToolStripMenuItem
            // 
            this.cargarProyectoToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cargarProyectoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.cargarProyectoToolStripMenuItem.Name = "cargarProyectoToolStripMenuItem";
            this.cargarProyectoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.cargarProyectoToolStripMenuItem.Text = "Cargar Proyecto (.paintproj)";
            // 
            // exportarImagenToolStripMenuItem
            // 
            this.exportarImagenToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.exportarImagenToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exportarImagenToolStripMenuItem.Name = "exportarImagenToolStripMenuItem";
            this.exportarImagenToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.exportarImagenToolStripMenuItem.Text = "Exportar Imagen (.png)";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.salirToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ForeColor = System.Drawing.Color.White;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUndo,
            this.btnRedo,
            this.btnSelect,
            this.toolStripSeparator1,
            this.btnFreehand,
            this.btnBezier,
            this.btnErase,
            this.btnFill,
            this.btnPicker,
            this.btnText,
            this.toolStripSeparator2,
            this.btnLine,
            this.btnRect,
            this.btnCircle,
            this.btnPoly,
            this.btnTri,
            this.btnStar});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(93, 718);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // numeric controls used by runtime (canvas size / zoom)
            this.numCanvasWidth.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            this.numCanvasWidth.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numCanvasWidth.Value = new decimal(new int[] { 800, 0, 0, 0 });
            this.numCanvasWidth.Width = 60;
            this.numCanvasHeight.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            this.numCanvasHeight.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numCanvasHeight.Value = new decimal(new int[] { 600, 0, 0, 0 });
            this.numCanvasHeight.Width = 60;
            this.numZoom.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.numZoom.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            this.numZoom.Value = new decimal(new int[] { 100, 0, 0, 0 });
            this.numZoom.Width = 60;

            // Add size/zoom controls to the toolstrip as hosts (labels + inputs)
            var hostWLabel = new System.Windows.Forms.ToolStripLabel("W:");
            var hostHLabel = new System.Windows.Forms.ToolStripLabel("H:");
            var hostZoomLabel = new System.Windows.Forms.ToolStripLabel("Zoom:");
            var hostW = new System.Windows.Forms.ToolStripControlHost(this.numCanvasWidth);
            var hostH = new System.Windows.Forms.ToolStripControlHost(this.numCanvasHeight);
            var hostZ = new System.Windows.Forms.ToolStripControlHost(this.numZoom);
            this.btnResizeCanvas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResizeCanvas.Name = "btnResizeCanvas";
            this.btnResizeCanvas.Size = new System.Drawing.Size(90, 19);
            this.btnResizeCanvas.Text = "Aplicar tamaño";
            this.btnDeleteLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDeleteLayer.Name = "btnDeleteLayer";
            this.btnDeleteLayer.Size = new System.Drawing.Size(90, 19);
            this.btnDeleteLayer.Text = "Borrar Capa";

            this.toolStrip1.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            this.toolStrip1.Items.Add(hostWLabel);
            this.toolStrip1.Items.Add(hostW);
            this.toolStrip1.Items.Add(hostHLabel);
            this.toolStrip1.Items.Add(hostH);
            this.toolStrip1.Items.Add(this.btnResizeCanvas);
            this.toolStrip1.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            this.toolStrip1.Items.Add(hostZoomLabel);
            this.toolStrip1.Items.Add(hostZ);
            this.toolStrip1.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            this.toolStrip1.Items.Add(this.btnDeleteLayer);
            // 
            // btnUndo
            // 
            this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(90, 19);
            this.btnUndo.Text = "↺ Deshacer";
            this.btnUndo.ToolTipText = "Deshacer (Ctrl+Z)";
            // 
            // btnRedo
            // 
            this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(90, 19);
            this.btnRedo.Text = "↻ Rehacer";
            this.btnRedo.ToolTipText = "Rehacer (Ctrl+Y)";
            // 
            // btnSelect
            // 
            this.btnSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(90, 19);
            this.btnSelect.Text = "🖱 Selección";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(90, 6);
            // 
            // btnFreehand
            // 
            this.btnFreehand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFreehand.Name = "btnFreehand";
            this.btnFreehand.Size = new System.Drawing.Size(90, 19);
            this.btnFreehand.Text = "✎ Lápiz";
            // 
            // btnBezier
            // 
            this.btnBezier.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBezier.Name = "btnBezier";
            this.btnBezier.Size = new System.Drawing.Size(90, 19);
            this.btnBezier.Text = "〰 Bézier";
            // 
            // btnErase
            // 
            this.btnErase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(90, 19);
            this.btnErase.Text = "▱ Borrador";
            // 
            // btnFill
            // 
            this.btnFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(90, 19);
            this.btnFill.Text = "▧ Relleno";
            // 
            // btnPicker
            // 
            this.btnPicker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPicker.Name = "btnPicker";
            this.btnPicker.Size = new System.Drawing.Size(90, 19);
            this.btnPicker.Text = "🎨 Color";
            // 
            // btnText
            // 
            this.btnText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(90, 19);
            this.btnText.Text = "A Texto";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(90, 6);
            // 
            // btnLine
            // 
            this.btnLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(90, 19);
            this.btnLine.Text = "➖ Línea";
            // 
            // btnRect
            // 
            this.btnRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(90, 19);
            this.btnRect.Text = "▭ Rect";
            // 
            // btnCircle
            // 
            this.btnCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(90, 19);
            this.btnCircle.Text = "⭕ Círculo";
            // 
            // btnPoly
            // 
            this.btnPoly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPoly.Name = "btnPoly";
            this.btnPoly.Size = new System.Drawing.Size(90, 19);
            this.btnPoly.Text = "⬡ Polígono";
            // 
            // btnTri
            // 
            this.btnTri.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTri.Name = "btnTri";
            this.btnTri.Size = new System.Drawing.Size(90, 19);
            this.btnTri.Text = "△ Triángulo";
            // 
            // btnStar
            // 
            this.btnStar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStar.Name = "btnStar";
            this.btnStar.Size = new System.Drawing.Size(90, 19);
            this.btnStar.Text = "★ Estrella";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);
            this.pnlRight.Controls.Add(this.lstLayers);
            this.pnlRight.Controls.Add(this.lblLayersTitle);
            this.pnlRight.Controls.Add(this.pnlTransformsSpacer);
            this.pnlRight.Controls.Add(this.pnlTransforms);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.TabIndex = 2;
            // 
            // lstLayers
            // 
            this.lstLayers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.lstLayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLayers.ForeColor = System.Drawing.Color.White;
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.ItemHeight = 17;
            this.lstLayers.Location = new System.Drawing.Point(0, 25);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.Size = new System.Drawing.Size(250, 501);
            this.lstLayers.TabIndex = 1;
            // 
            // lblLayersTitle
            // 
            this.lblLayersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLayersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLayersTitle.ForeColor = System.Drawing.Color.White;
            this.lblLayersTitle.Location = new System.Drawing.Point(0, 0);
            this.lblLayersTitle.Name = "lblLayersTitle";
            this.lblLayersTitle.Size = new System.Drawing.Size(250, 25);
            this.lblLayersTitle.TabIndex = 0;
            this.lblLayersTitle.Text = "Gestor de Capas";
            this.lblLayersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTransformsSpacer
            // 
            this.pnlTransformsSpacer.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.pnlTransformsSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTransformsSpacer.Height = 14;
            this.pnlTransformsSpacer.Name = "pnlTransformsSpacer";
            this.pnlTransformsSpacer.TabIndex = 6;
            // 
            // pnlTransforms
            // 
            this.pnlTransforms.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlTransforms.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTransforms.Location = new System.Drawing.Point(0, 526);
            this.pnlTransforms.Name = "pnlTransforms";
            this.pnlTransforms.Size = new System.Drawing.Size(250, 192);
            this.pnlTransforms.TabIndex = 2;
            // 
            // lblTransform
            // 
            this.lblTransform.Location = new System.Drawing.Point(0, 0);
            this.lblTransform.Name = "lblTransform";
            this.lblTransform.Size = new System.Drawing.Size(100, 23);
            this.lblTransform.TabIndex = 0;
            // 
            // pnlCenter
            // 
            this.pnlCenter.AutoScroll = true;
            this.pnlCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.pnlCenter.Controls.Add(this.canvasPicBox);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(93, 24);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(856, 718);
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
            // 
            // pnlLeftShell
            // 
            this.pnlLeftShell.BackColor = System.Drawing.ColorTranslator.FromHtml("#121212");
            this.pnlLeftShell.Controls.Add(this.toolStrip1);
            this.pnlLeftShell.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftShell.Name = "pnlLeftShell";
            this.pnlLeftShell.Padding = new System.Windows.Forms.Padding(18);
            this.pnlLeftShell.Size = new System.Drawing.Size(188, 693);
            this.pnlLeftShell.TabIndex = 4;
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

            // ---- Dark Theme Palette ----
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#121212");
            this.menuStrip1.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.menuStrip1.ForeColor = System.Drawing.Color.White;
            this.menuStrip1.Renderer = new DarkToolStripRenderer();
            this.toolStrip1.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.toolStrip1.ForeColor = System.Drawing.Color.White;
            this.toolStrip1.Renderer = new DarkToolStripRenderer();
            this.pnlRight.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.pnlTransforms.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.lstLayers.BackColor = System.Drawing.ColorTranslator.FromHtml("#1E1E1E");
            this.lstLayers.ForeColor = System.Drawing.Color.White;
            this.lstLayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblLayersTitle.ForeColor = System.Drawing.Color.White;
            this.pnlCenter.BackColor = System.Drawing.ColorTranslator.FromHtml("#121212");
            // 
            // FrmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(1199, 742);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlRightShell);
            this.Controls.Add(this.pnlLeftShell);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paint 2026 - PaintStudio";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCanvasWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCanvasHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZoom)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvasPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarProyectoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarProyectoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarImagenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnUndo;
        private System.Windows.Forms.ToolStripButton btnRedo;
        private System.Windows.Forms.ToolStripButton btnDeleteLayer;
        private System.Windows.Forms.ToolStripButton btnResizeCanvas;
        private System.Windows.Forms.NumericUpDown numCanvasWidth;
        private System.Windows.Forms.NumericUpDown numCanvasHeight;
        private System.Windows.Forms.NumericUpDown numZoom;
        private System.Windows.Forms.ToolStripButton btnSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnFreehand;
        private System.Windows.Forms.ToolStripButton btnBezier;
        private System.Windows.Forms.ToolStripButton btnErase;
        private System.Windows.Forms.ToolStripButton btnFill;
        private System.Windows.Forms.ToolStripButton btnPicker;
        private System.Windows.Forms.ToolStripButton btnText;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnLine;
        private System.Windows.Forms.ToolStripButton btnRect;
        private System.Windows.Forms.ToolStripButton btnCircle;
        private System.Windows.Forms.ToolStripButton btnPoly;
        private System.Windows.Forms.ToolStripButton btnTri;
        private System.Windows.Forms.ToolStripButton btnStar;

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlLeftShell;
        private System.Windows.Forms.Panel pnlRightShell;
        private System.Windows.Forms.Panel pnlTransformsSpacer;
        private System.Windows.Forms.Label lblLayersTitle;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.Panel pnlTransforms;
        private System.Windows.Forms.Label lblTransform;

        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.PictureBox canvasPicBox;

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}