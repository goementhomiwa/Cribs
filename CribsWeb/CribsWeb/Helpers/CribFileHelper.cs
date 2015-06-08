using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
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
    }
}