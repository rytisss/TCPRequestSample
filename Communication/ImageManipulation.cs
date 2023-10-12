using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Communication
{
    public class ImageManipulation
    {
        public ImageManipulation()
        {
        }
        public static string ImageToBase64(Image image)
        {
            var imageStream = new MemoryStream();
            try
            {
                image.Save(imageStream, ImageFormat.Bmp);
                imageStream.Position = 0;
                var imageBytes = imageStream.ToArray();
                var ImageBase64 = Convert.ToBase64String(imageBytes);
                return ImageBase64;
            }
            catch (Exception ex)
            {
                return "Error converting image to base64! " + ex.Message;
            }
            finally
            {
                imageStream.Dispose();
            }
        }
        public static Image ReadImage(string filePath)
        {
            return File.Exists(filePath) ? Image.FromFile(filePath) : null;
        }

        public static void GetImageAsBase64(string imagePath, out string imageBase64, out int imageWidth, out int imageHeight, out int imageChannels)
        {
            //initial states of outputs
            imageBase64 = "";
            imageWidth = 0;
            imageHeight = 0;
            imageChannels = 0;

            //read image
            Image image = ReadImage(imagePath);

            //if image is 'null' there is nothing more to do
            if (image == null) return;

            imageWidth = image.Width;
            imageHeight = image.Height;
            switch (image.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    imageChannels = 1;
                    break;
                case PixelFormat.Format24bppRgb:
                    imageChannels = 3;
                    break;
                case PixelFormat.Format32bppArgb:
                    imageChannels = 4;
                    break;
                default:
                    throw new Exception("Unexpected image type!");
            }

            //convert to base64
            imageBase64 = ImageToBase64(image);
        }

        public static void GetImageAsBase64(Image image, out string imageBase64, out int imageWidth, out int imageHeight, out int imageChannels)
        {
            //initial states of outputs
            imageBase64 = "";
            imageWidth = 0;
            imageHeight = 0;
            imageChannels = 0;

            //if image is 'null' there is nothing more to do
            if (image == null) return;

            imageWidth = image.Width;
            imageHeight = image.Height;
            switch (image.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    imageChannels = 1;
                    break;
                case PixelFormat.Format24bppRgb:
                    imageChannels = 3;
                    break;
                case PixelFormat.Format32bppArgb:
                    imageChannels = 4;
                    break;
                default:
                    throw new Exception("Unexpected image type!");
            }

            //draw timestamp
            using (Graphics graphics = Graphics.FromImage(image))
            {
                string text = DateTime.Now.ToString();
                graphics.DrawString(text, new Font("Tahoma", 50, FontStyle.Bold), Brushes.Black, new RectangleF(50, 50, 500, 500));
            }

            //convert to base64
            imageBase64 = ImageToBase64(image);
        }
    }
}
