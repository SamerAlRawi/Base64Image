using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace Base64Image
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// adds image dom element from a base64 image string
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="base64Image"></param>
        /// <returns></returns>
        public static MvcHtmlString Base64ImageElement(this HtmlHelper htmlHelper, string base64Image, Dictionary<string, string> htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            var imageCodec = TryGetFormatFromBase64String(base64Image);
            builder.Attributes.Add("src", BuildValues(imageCodec, base64Image));
            AddHTMLAttributes(htmlAttributes, builder);
            return MvcHtmlString.Create(builder.ToString());
        }
        /// <summary>
        /// adds image as base64 string from a physical image file on web server
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="relativeLocation"></param>
        /// <returns></returns>
        public static MvcHtmlString Base64ImageFromFile(this HtmlHelper htmlHelper, string relativeLocation, Dictionary<string, string> htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            var base64Image = "";
            var imageCodec = "";
            var physicalPath = htmlHelper.ViewContext.RequestContext.HttpContext.Server.MapPath(relativeLocation);
            if (File.Exists(physicalPath))
            {
                using (var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (Image image = Image.FromStream(fileStream))
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
            }
            builder.Attributes.Add("src", BuildValues(imageCodec, base64Image));
            AddHTMLAttributes(htmlAttributes, builder);
            return MvcHtmlString.Create(builder.ToString());
        }

        private static void AddHTMLAttributes(Dictionary<string, string> htmlAttributes, TagBuilder builder)
        {
            if (htmlAttributes == null)
                return;
            foreach (var htmlAttribute in htmlAttributes)
            {
                builder.Attributes.Add(htmlAttribute.Key, htmlAttribute.Value);
            }
        }

        private static string TryGetFormatFromBase64String(string base64Image)
        {
            var bytes = Convert.FromBase64String(base64Image);
            var stream = new MemoryStream(bytes);
            var imageCodec = GetImageEncoding(Image.FromStream(stream).RawFormat);
            return imageCodec;
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
