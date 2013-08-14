using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace LetsDonateStuff.Helpers
{
    public class Imgur
    {
        string apiKey;

        public Imgur(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public void Upload(string filePath)
        {
            using(var fileStream = File.OpenRead(filePath))
                Upload(fileStream);
        }

        public ImageUploadResult Upload(Stream fileStream)
        {
            var imageData = new byte[fileStream.Length];
            fileStream.Read(imageData, 0, imageData.Length);

            string uploadRequestString = "image=" + HttpUtility.UrlEncode(System.Convert.ToBase64String(imageData)) + "&key=" + apiKey;

            var webRequest = (HttpWebRequest)WebRequest.Create("http://api.imgur.com/2/upload");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;

            var streamWriter = new StreamWriter(webRequest.GetRequestStream());
            streamWriter.Write(uploadRequestString);
            streamWriter.Close();

            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            var responseReader = new StreamReader(responseStream);

            string responseString = responseReader.ReadToEnd();

            var responseXml = XElement.Parse(responseString);
            var result = ImageUploadResult.FromXml(responseXml);

            return result;
        }

        public void Delete(string hash)
        {
            var client = new WebClient();
            client.DownloadString("http://api.imgur.com/2/delete/" + hash);
        }
    }

    public class ImageUploadResult
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Hash { get; set; }
        public string DeleteHash { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public int Views { get; set; }
        public int Bandwidth { get; set; }

        public Uri Original { get; set; }
        public Uri ImgurPage { get; set; }
        public Uri DeletePage { get; set; }
        public Uri SmallSquare { get; set; }
        public Uri LargeThumbnail { get; set; }

        public static ImageUploadResult FromXml(XElement xml)
        {
            var result = new ImageUploadResult();

            var image = xml.Element("image");
            result.Name = GetField(image, "name");
            result.Title = GetField(image, "title");
            result.Hash = GetField(image, "hash");
            result.DeleteHash = GetField(image, "deletehash");
            result.DateTime = GetField(image, "datetime", x=>DateTime.Parse(x));
            result.Type = GetField(image, "type");
            result.Animated = GetField(image, "animated", x => Boolean.Parse(x));
            result.Width = GetField(image, "width", x => Int32.Parse(x));
            result.Height = GetField(image, "height", x => Int32.Parse(x));
            result.Size = GetField(image, "size", x => Int32.Parse(x));
            result.Views = GetField(image, "views", x => Int32.Parse(x));
            result.Bandwidth = GetField(image, "bandwidth", x => Int32.Parse(x));

            var links = xml.Element("links");
            result.Original = GetField(links, "original", x => new Uri(x, UriKind.Absolute));
            result.ImgurPage = GetField(links, "imgur_page", x => new Uri(x, UriKind.Absolute));
            result.DeletePage = GetField(links, "delete_page", x => new Uri(x, UriKind.Absolute));
            result.SmallSquare = GetField(links, "small_square", x => new Uri(x, UriKind.Absolute));
            result.LargeThumbnail = GetField(links, "large_thumbnail", x => new Uri(x, UriKind.Absolute));

            return result;
        }

        static string GetField(XElement xml, string fieldName)
        {
            return GetField<string>(xml, fieldName, x => x);
        }

        static T GetField<T>(XElement xml, string fieldName, Func<string, T> parseAction)
        {
            return xml.Element(fieldName).Coalesce(e => parseAction(e.Value), default(T));
        }
    }
}