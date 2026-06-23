using System;
using System.Collections.Generic;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    public class CanvasModel
    {
        // -------------------- CAMPOS --------------------
        public List<Shape> Shapes { get; private set; }

        // -------------------- CONSTRUCTOR --------------------
        public CanvasModel()
        {
            Shapes = new List<Shape>();
        }

        // -------------------- MÉTODOS PÚBLICOS --------------------
        public void AddShape(Shape shape)
        {
            Shapes.Add(shape);
        }

        public void SetShapes(List<Shape> shapes)
        {
            Shapes = shapes ?? new List<Shape>();
        }

        public void RemoveShapesAt(IEnumerable<int> indices)
        {
            var sorted = new List<int>(indices);
            sorted.Sort();
            for (int i = sorted.Count - 1; i >= 0; i--)
                RemoveShapeAt(sorted[i]);
        }

        public void RemoveShape(Shape shape)
        {
            Shapes.Remove(shape);
        }

        public void Clear()
        {
            Shapes.Clear();
        }

        public void RemoveShapeAt(int index)
        {
            if (index >= 0 && index < Shapes.Count)
                Shapes.RemoveAt(index);
        }

        public void MoveShapeUp(int index)
        {
            if (index >= 0 && index < Shapes.Count - 1)
            {
                var shape = Shapes[index];
                Shapes.RemoveAt(index);
                Shapes.Insert(index + 1, shape);
            }
        }

        public void MoveShapeDown(int index)
        {
            if (index > 0 && index < Shapes.Count)
            {
                var shape = Shapes[index];
                Shapes.RemoveAt(index);
                Shapes.Insert(index - 1, shape);
            }
        }

        public void SaveToFile(string path)
        {
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, Shapes);
            }
        }

        public void LoadFromFile(string path)
        {
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Shapes = (List<Shape>)formatter.Deserialize(stream);
            }
        }
    }
}