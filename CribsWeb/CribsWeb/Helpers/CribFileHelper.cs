using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using Cribs.Web.Models;

namespace Cribs.Web.Helpers
{
    public class CribFileHelper
    {
        public static Byte[] GetByteArray(HttpPostedFileBase file)
        {
            Byte[] byteArray = new Byte[file.ContentLength];
            file.InputStream.Position = 0;
            file.InputStream.Read(byteArray, 0, file.ContentLength);

            return byteArray;
        }

        public static Image GetImage(byte[] b)
        {
            MemoryStream stream = new MemoryStream(b);
            return Image.FromStream(stream);
        }
        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        //resizing
        public static byte[] resizeImage(byte[] imageBytes, Size size)
        {
            Image imgToResize = (Bitmap)(new ImageConverter()).ConvertFrom(imageBytes);
            //source image dimension
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            //proportionality variables
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            //determining the actual size for resizing
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            Image imageIn = (Image)b;
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

            return ms.ToArray();
        }

        public static List<string> GetImageSources(List<CribImages> modelList)
        {
            return modelList.Select(image => GetImageSource(image.Image)).ToList();
        }

        public static string GetImageSource(byte[] image)
        {
            var imageConvert = Convert.ToBase64String(image);
            var source = string.Format("data:image/gif;base64,{0}", imageConvert);
            return source;
        }
    }
}