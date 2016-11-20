using System;
using System.IO;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace Base64Image
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Base64ImageElement(this HtmlHelper htmlHelper, string base64Image)
        {
            var builder = new TagBuilder("img");

            var bytes = Convert.FromBase64String(base64Image);
            var stream = new MemoryStream(bytes);
            var imageCodec = GetImageEncoding(Image.FromStream(stream).RawFormat);
            builder.Attributes.Add("src", BuildValues(imageCodec, base64Image));
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Base64ImageFromFile(this HtmlHelper htmlHelper, string relativeLocation)
        {
            var builder = new TagBuilder("img");
            string base64Image = "";
            string imageCodec = "";
            var physicalPath = htmlHelper.ViewContext.RequestContext.HttpContext.Server.MapPath(relativeLocation);
            if (File.Exists(physicalPath))
            {
                using (Image image = Image.FromFile(physicalPath))
                {
                    imageCodec = GetImageEncoding(image.RawFormat);
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64Image = Convert.ToBase64String(imageBytes);
                    }
                }
            }
            builder.Attributes.Add("src", BuildValues(imageCodec, base64Image));
            return MvcHtmlString.Create(builder.ToString());
        }

        private static string GetImageEncoding(ImageFormat imageRawFormat)
        {
            if (ImageFormat.Gif.Equals(imageRawFormat))
            {
                return "image/gif";
            }
            if (ImageFormat.Png.Equals(imageRawFormat))
            {
                return "image/png";
            }
            if (ImageFormat.Jpeg.Equals(imageRawFormat))
            {
                return "image/jpeg";
            }
            return "image/jpeg";
        }

        private static string BuildValues(string format, string base64Image)
        {
            return $"data:{format};base64,{base64Image}";
        }
    }
}
