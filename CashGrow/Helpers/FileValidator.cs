using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace CashGrow.Helpers
{
    public class FileValidator
    {
        public static bool IsWebFriendlyImage(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                        ImageFormat.Png.Equals(img.RawFormat) ||
                        ImageFormat.Gif.Equals(img.RawFormat) ||
                        ImageFormat.Bmp.Equals(img.RawFormat) ||
                        ImageFormat.Tiff.Equals(img.RawFormat);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 2048)
                return false;

            try
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var AllowableExtensions = WebConfigurationManager.AppSettings["AllowableExtensions"].Split(',');
                return AllowableExtensions.Contains(fileExtension);
            }
            catch
            {
                return false;
            }
        }
    }
}