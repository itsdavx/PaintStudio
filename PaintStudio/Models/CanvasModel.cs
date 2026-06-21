using System;
using System.Collections.Generic;
using PaintStudio.Utils;

namespace PaintStudio.Models
{
    public class CanvasModel
    {
        public List<Shape> Shapes { get; private set; }

        public CanvasModel()
        {
            Shapes = new List<Shape>();
        }

        public void AddShape(Shape shape)
        {
            Shapes.Add(shape);
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
            {
                Shapes.RemoveAt(index);
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

