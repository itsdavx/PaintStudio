using System;
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
        private NumericUpDown numThickness;

        public FrmHome()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controller = new PaintController(canvasPicBox);
            controller.OnLayersChanged = UpdateLayersList;
            
            // Text tool
            controller.OnRequestTextInput = (p) => {
                string input = Prompt.ShowDialog("Ingrese el texto:", "Texto");
                if (!string.IsNullOrEmpty(input)) {
                    controller.AddTextShape(new PaintStudio.Models.PointD(p.X, p.Y), input, new Font("Segoe UI", 12));
                }
            };

            // Keyboard delete
            this.KeyPreview = true;
            this.KeyDown += (s, args) => {
                if (args.KeyCode == Keys.Delete && lstLayers.SelectedIndex >= 0) {
                    int realIndex = (controller.GetShapes().Count - 1) - lstLayers.SelectedIndex;
                    controller.RemoveShapeAt(realIndex);
                }
                if (args.KeyCode == Keys.Escape) {
                    lstLayers.ClearSelected();
                    controller.SelectShapeIndex(-1);
                }
            };

            // Color picker
            controller.OnColorPicked = (c) => { 
                if (btnColor != null) btnColor.BackColor = c; 
                controller.CurrentColor = c; 
            };

            lstLayers.SelectedIndexChanged += (s, args) => {
                if (controller != null && lstLayers.SelectedIndex >= 0) {
                    int realIndex = (controller.GetShapes().Count - 1) - lstLayers.SelectedIndex;
                    controller.SelectShapeIndex(realIndex);
                }
            };

            BuildTransformsPanel();
            BuildColorAndThickness();
            AssignToolEvents();
            AssignMenuEvents();
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
            ToolStripControlHost hostColorLabel = new ToolStripControlHost(new Label() { Text = " Color:", AutoSize = true });
            btnColor = new Button() { BackColor = Color.Black, Size = new Size(20, 20), FlatStyle = FlatStyle.Flat };
            btnColor.Click += (s, e) => {
                if (colorDialog1.ShowDialog() == DialogResult.OK) {
                    btnColor.BackColor = colorDialog1.Color;
                    controller.CurrentColor = colorDialog1.Color;
                }
            };
            ToolStripControlHost hostColorBtn = new ToolStripControlHost(btnColor);
            
            ToolStripControlHost hostThickLabel = new ToolStripControlHost(new Label() { Text = " Grosor:", AutoSize = true });
            numThickness = new NumericUpDown() { Minimum = 1, Maximum = 30, Value = 2, Width = 50 };
            numThickness.ValueChanged += (s, e) => { controller.CurrentThickness = (int)numThickness.Value; };
            ToolStripControlHost hostThickNum = new ToolStripControlHost(numThickness);

            toolStrip1.Items.Add(new ToolStripSeparator());
            toolStrip1.Items.Add(hostColorLabel);
            toolStrip1.Items.Add(hostColorBtn);
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
            nuevoToolStripMenuItem.Click += (s, e) => controller.ClearCanvas();
            salirToolStripMenuItem.Click += (s, e) => Application.Exit();

            guardarProyectoToolStripMenuItem.Click += (s, e) => {
                saveFileDialog1.Filter = "Paint Project|*.paintproj";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    controller.SaveProject(saveFileDialog1.FileName);
                }
            };

            exportarImagenToolStripMenuItem.Click += (s, e) => {
                saveFileDialog1.Filter = "PNG Image|*.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    controller.SaveImage(saveFileDialog1.FileName);
                }
            };

            cargarProyectoToolStripMenuItem.Click += (s, e) => {
                openFileDialog1.Filter = "Paint Project|*.paintproj";
                if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                    controller.LoadProject(openFileDialog1.FileName);
                }
            };
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
            if (index < 0) {
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

            if (angle != 0) shape.ApplyTransformation(PaintStudio.Utils.Transformations.GetRotationMatrix(angle, c.X, c.Y));
            if (scale != 1.0) shape.ApplyTransformation(PaintStudio.Utils.Transformations.GetScaleMatrix(scale, scale, c.X, c.Y));
            if (tx != 0 || ty != 0) shape.ApplyTransformation(PaintStudio.Utils.Transformations.GetTranslationMatrix(tx, ty));

            numRot.Value = 0;
            numScale.Value = 100;
            numTransX.Value = 0;
            numTransY.Value = 0;

            canvasPicBox.Invalidate();
        }

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
