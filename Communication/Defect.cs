using System.Collections.Generic;
using System.Drawing;

namespace Communication
{
    public class Defect
    {
        public Defect()
        {
        }
        // Defect contour points
        public List<Point> Contour { get; set; } = new List<Point>();
        // Defect confidence
        public float Confidence { get; set; } = -1.0f;
        // Defect type [name]
        public string Type { get; set; } = "";
        // Clean resource
        public void Clear()
        {
            if (Contour != null)
            {
                Contour.Clear();
            }
            Confidence = -1.0f;
            Type = "";
        }
    }
}
