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
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
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
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            // 
            // guardarProyectoToolStripMenuItem
            // 
            this.guardarProyectoToolStripMenuItem.Name = "guardarProyectoToolStripMenuItem";
            this.guardarProyectoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.guardarProyectoToolStripMenuItem.Text = "Guardar Proyecto (.paintproj)";
            // 
            // cargarProyectoToolStripMenuItem
            // 
            this.cargarProyectoToolStripMenuItem.Name = "cargarProyectoToolStripMenuItem";
            this.cargarProyectoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.cargarProyectoToolStripMenuItem.Text = "Cargar Proyecto (.paintproj)";
            // 
            // exportarImagenToolStripMenuItem
            // 
            this.exportarImagenToolStripMenuItem.Name = "exportarImagenToolStripMenuItem";
            this.exportarImagenToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.exportarImagenToolStripMenuItem.Text = "Exportar Imagen (.png)";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1199, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSelect
            // 
            this.btnSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(76, 22);
            this.btnSelect.Text = "🖱 Selección";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFreehand
            // 
            this.btnFreehand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFreehand.Name = "btnFreehand";
            this.btnFreehand.Size = new System.Drawing.Size(53, 22);
            this.btnFreehand.Text = "✎ Lápiz";
            // 
            // btnBezier
            // 
            this.btnBezier.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBezier.Name = "btnBezier";
            this.btnBezier.Size = new System.Drawing.Size(57, 22);
            this.btnBezier.Text = "〰 Bézier";
            // 
            // btnErase
            // 
            this.btnErase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnErase.Name = "btnErase";
            this.btnErase.Size = new System.Drawing.Size(72, 22);
            this.btnErase.Text = "▱ Borrador";
            // 
            // btnFill
            // 
            this.btnFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(63, 22);
            this.btnFill.Text = "▧ Relleno";
            // 
            // btnPicker
            // 
            this.btnPicker.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPicker.Name = "btnPicker";
            this.btnPicker.Size = new System.Drawing.Size(55, 22);
            this.btnPicker.Text = "🎨 Color";
            // 
            // btnText
            // 
            this.btnText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(50, 22);
            this.btnText.Text = "A Texto";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLine
            // 
            this.btnLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(54, 22);
            this.btnLine.Text = "➖ Línea";
            // 
            // btnRect
            // 
            this.btnRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(49, 22);
            this.btnRect.Text = "▭ Rect";
            // 
            // btnCircle
            // 
            this.btnCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(62, 22);
            this.btnCircle.Text = "⭕ Círculo";
            // 
            // btnPoly
            // 
            this.btnPoly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPoly.Name = "btnPoly";
            this.btnPoly.Size = new System.Drawing.Size(72, 22);
            this.btnPoly.Text = "⬡ Polígono";
            // 
            // btnTri
            // 
            this.btnTri.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTri.Name = "btnTri";
            this.btnTri.Size = new System.Drawing.Size(74, 22);
            this.btnTri.Text = "△ Triángulo";
            // 
            // btnStar
            // 
            this.btnStar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStar.Name = "btnStar";
            this.btnStar.Size = new System.Drawing.Size(61, 22);
            this.btnStar.Text = "★ Estrella";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlRight.Controls.Add(this.lstLayers);
            this.pnlRight.Controls.Add(this.lblLayersTitle);
            this.pnlRight.Controls.Add(this.pnlTransforms);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(949, 49);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(250, 704);
            this.pnlRight.TabIndex = 2;
            // 
            // lstLayers
            // 
            this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.ItemHeight = 15;
            this.lstLayers.Location = new System.Drawing.Point(0, 25);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.Size = new System.Drawing.Size(250, 479);
            this.lstLayers.TabIndex = 1;
            // 
            // lblLayersTitle
            // 
            this.lblLayersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLayersTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLayersTitle.Location = new System.Drawing.Point(0, 0);
            this.lblLayersTitle.Name = "lblLayersTitle";
            this.lblLayersTitle.Size = new System.Drawing.Size(250, 25);
            this.lblLayersTitle.TabIndex = 0;
            this.lblLayersTitle.Text = "Gestor de Capas";
            this.lblLayersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTransforms
            // 
            this.pnlTransforms.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTransforms.Location = new System.Drawing.Point(0, 504);
            this.pnlTransforms.Name = "pnlTransforms";
            this.pnlTransforms.Size = new System.Drawing.Size(250, 200);
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
            this.pnlCenter.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnlCenter.Controls.Add(this.canvasPicBox);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(0, 49);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(949, 704);
            this.pnlCenter.TabIndex = 3;
            // 
            // canvasPicBox
            // 
            this.canvasPicBox.BackColor = System.Drawing.Color.White;
            this.canvasPicBox.Location = new System.Drawing.Point(20, 20);
            this.canvasPicBox.Name = "canvasPicBox";
            this.canvasPicBox.Size = new System.Drawing.Size(800, 600);
            this.canvasPicBox.TabIndex = 0;
            this.canvasPicBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 753);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Paint Pro - Estilo Clásico";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private System.Windows.Forms.Label lblLayersTitle;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.Panel pnlTransforms;
        private System.Windows.Forms.Label lblTransform;

        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.PictureBox canvasPicBox;

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
