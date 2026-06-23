using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PaintStudio.Controllers;

namespace PaintStudio
{
    public partial class FrmHome
    {
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

        private void BtnApplyTransform_Click(object sender, EventArgs e)
        {
            int index = lstLayers.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Seleccione una capa primero.");
                return;
            }
            int realIndex = (controller.GetShapes().Count - 1) - index;
            double angle = (double)numRot.Value;
            double scale = (double)numScale.Value / 100.0;
            double tx = (double)numTransX.Value;
            double ty = -(double)numTransY.Value;

            if (angle == 0 && scale == 1.0 && tx == 0 && ty == 0) return;

            controller.ApplyTransformationToSelected(realIndex, angle, scale, tx, ty);

            numRot.Value = 0;
            numScale.Value = 100;
            numTransX.Value = 0;
            numTransY.Value = 0;
        }
    }
}
