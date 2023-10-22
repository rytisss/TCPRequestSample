using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Communication
{
    public class ResultMessage
    {
        public ResultMessage()
        {
        }
        /// <summary>
        /// Command
        /// </summary>
        public string Command { get; set; } = "OnResultsReady";
        /// <summary>
        /// Timestamp
        /// </summary>
        public long Timestamp { get; set; } = 0;
        /// <summary>
        /// System ID
        /// </summary>
        public int SystemID { get; set; } = -1;
        /// <summary>
        /// Name of camera the image is taken
        /// </summary>
        public string CameraName { get; set; } = "";
        /// <summary>
        /// Seat part name: 'Cushion' or 'Backrest'
        /// </summary>
        public string Part { get; set; } = "";
        /// <summary>
        /// List of wrinkles or other defects  contours
        /// </summary>
        public List<Defect> Defects { get; set; } = new List<Defect>();
        /// <summary>
        /// Segmented part contour
        /// </summary>
        public List<Point> PartContour { get; set; } = new List<Point>();
        /// <summary>
        /// Clear internal resources
        /// </summary>
        public void Clear()
        {
            Command = "";
            Timestamp = 0;
            SystemID = -1;
            CameraName = "";
            if (Defects == null)
            {
                Defects = new List<Defect>();
            }
            else
            {
                Defects.Clear();
            }
            if (PartContour == null) 
            {
                PartContour = new List<Point>();
            }
            else
            {
                PartContour.Clear();
            }
        }
    }
}
