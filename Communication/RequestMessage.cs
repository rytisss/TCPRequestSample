using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Communication
{
    /// <summary>
    /// Message stucture for image processing server analysis
    /// </summary>
    public class RequestMessage
    {
        public RequestMessage()
        {
        }
        /// <summary>
        /// Image name/title
        /// </summary>
        public string ImageName { get; set; } = "";
        /// <summary>
        /// Image timestamp
        /// </summary>
        public long Timestamp { get; set; } = 0;
        /// <summary>
        /// Image grabbing system ID
        /// </summary>
        public int SystemID { get; set; } = -1;
        /// <summary>
        /// Name of the camera that current image is grabbed
        /// </summary>
        public string CameraName { get; set; } = "";
        /// <summary>
        /// Seat part
        /// </summary>
        public string Part { get; set; } = "";
        /// <summary>
        /// Command name
        /// </summary>
        public string Command { get; set; } = "SendImageData";
        /// <summary>
        /// Model name
        /// </summary>
        public string Model { get; set; } = "C236";
        /// <summary>
        /// Image width
        /// </summary>
        public int ImageWidth { get; set; } = 0;
        /// <summary>
        /// Image height
        /// </summary>
        public int ImageHeight { get; set; } = 0;
        /// <summary>
        /// Number of image color channels
        /// </summary>
        public int ImageChannelsCount { get; set; } = 0;
        /// <summary>
        /// Image data (byte array) encoded as base64
        /// </summary>
        public string ImageAsBase64 { get; set; } = "";
        /// <summary>
        /// Main threshold for wrinkles
        /// </summary>
        public float Threshold { get; set; } = 0.9f;
        /// <summary>
        /// Borderline threshold. It is substracted from the main 'Threshold'. For example 'Threshold' = 0.5f and the 'ThresholdBorderline' = 0.1f,
        /// so the calculated borderline threshold will be 0.4f
        /// </summary>
        public float ThresholdBorderline { get; set; } = 0.1f;
        /// <summary>
        /// Reset all internal parameter
        /// </summary>
        public void Clear()
        {
            Timestamp = 0;
            SystemID = -1;
            CameraName = "";
            Part = "";
            Command = "";
            Model = "";
            ImageWidth = 0;
            ImageHeight = 0;
            ImageChannelsCount = 0;
            ImageAsBase64 = "";
            ImageName = "";
            Threshold = 0.9f;
            ThresholdBorderline = 0.1f;
        }
    }
}
