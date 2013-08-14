using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Drawing;

namespace LetsDonateStuff.Helpers.Attributes
{
    public class FileAttribute : ValidationAttribute
    {

        public int MaxContentLength { get; set; }
        public string[] AllowedFileExtensions { get; set; }
        public string[] AllowedContentTypes {get; set; }
        public bool IsImage { get; set; }

        public FileAttribute()
        {
            MaxContentLength = Int32.MaxValue;
        }

        public override bool IsValid(object value)
        {

            var file = value as HttpPostedFileBase;

            //this should be handled by [Required]
            if (file == null)
                return true;

            if (!ValidateSize(file.ContentLength))
                return false;

            if (!ValidateExtension(file.FileName))
                return false;

            if (!ValidateContentType(file.ContentType))
                return false;

            if (!ValidateImage(file.InputStream))
                return false;

            return true;
        }

        bool ValidateSize(int contentLength)
        {
            if (contentLength > MaxContentLength)
            {
                ErrorMessage = String.Format("File is too large, maximum allowed is: {0} KB", MaxContentLength / 1024);
                return false;
            }
            return true;
        }

        bool ValidateExtension(string fileName)
        {
            if (AllowedFileExtensions == null)
                return true;

            string extension = Path.GetExtension(fileName);
            bool isValid = AllowedFileExtensions.Any(allowedExtension => allowedExtension.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
            if (!isValid)
            {
                ErrorMessage = "Please upload file of type: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            return true;
        }

        bool ValidateImage(Stream stream)
        {
            if (IsImage && !IsValidImage(stream))
            {
                ErrorMessage = "Please upload a valid image";
                return false;
            }
            return true;
        }

        bool ValidateContentType(string contentType)
        {
            if (AllowedContentTypes == null)
                return false;

            bool isValid = AllowedContentTypes.Any(allowedContentType => allowedContentType.Equals(contentType, StringComparison.InvariantCultureIgnoreCase));
            if (!isValid)
            {
                ErrorMessage = "Please upload file of type: " + string.Join(", ", AllowedContentTypes);
                return false;
            }
            return true;
        }

        static byte[][] imageHeaders = new[]{
                                           new byte[]{255, 216},                          // JPEG
                                           new byte[]{(int)'B', (int)'M'},                // BMP
                                           new byte[]{(int)'G', (int)'I', (int)'F'},      // GIF
                                           new byte[]{137, 80, 78, 71}                    // PNG
                                       };

        static int headerLength = imageHeaders.Max(h => h.Length);

        static bool IsValidImage(Stream imgStream)
        {
            bool isValid = false;

            if (imgStream.Length > 0)
            {
                var header = new byte[headerLength];
                imgStream.Read(header, 0, header.Length);

                isValid = imageHeaders.Any(sample => header.Take(sample.Length).SequenceEqual(sample));
            }

            imgStream.Seek(0, SeekOrigin.Begin);

            return isValid;
        }
    }
}