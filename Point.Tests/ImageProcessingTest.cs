using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageSharp;
using System.IO;
using ImageSharp.Formats;
using Point.Contracts.Models;
using Newtonsoft.Json;

namespace Point.Tests
{
    [TestClass]
    public class ImageProcessingTest
    {
        string _imageFolderPath = "../../../Images/";

        [TestMethod]
        public void TestMethod1()
        {

            //Image image = null;
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response = await httpClient.GetAsync(url);
            //Stream inputStream = await response.Content.ReadAsStreamAsync();

            //using (var image = Image.Load(_imageFolderPath + "test.jpg"))
            //{
            //    image.Resize(image.Width / 2, image.Height / 2)
            //         .Grayscale()
            //         .Save(_imageFolderPath + "resized.jpg"); // automatic encoder selected based on extension.
            //}

            //using (var image = Image.Load(_imageFolderPath + "test.jpg"))
            //{
            //    image.Resize(20, 20)
            //         .Grayscale()
            //         .Save(_imageFolderPath + "resized20x20.jpg"); // automatic encoder selected based on extension.
            //}

            using (var image = Image.Load(_imageFolderPath + "resized20x20.jpg"))
            {
                Stream outputStream = new MemoryStream();
                image.Save(outputStream, new JpegEncoder());
                var bytes = ReadFully(outputStream);

                //image.Resize(image.Width / 2, image.Height / 2)
                //     .Grayscale()
                //     .SaveAsPng(outputStream); // automatic encoder selected based on extension.
            }
        }

        [TestMethod]
        public void SerealizeInfoPostTest()
        {
            byte[] bytes;
            using (var image = Image.Load(_imageFolderPath + "resized20x20.jpg"))
            {
                Stream outputStream = new MemoryStream();
                image.Save(outputStream, new JpegEncoder());
                bytes = ReadFully(outputStream);
            }

            InfoPost info = new InfoPost()
            {
                Description = "des",
                Latitude = 123.134235,
                Longitude = 223.134235,
                UserId = 100,
                Images = new System.Collections.Generic.List<byte[]>() { bytes }
            };

            var ser = JsonConvert.SerializeObject(info);
        }

        public static byte[] ReadFully(Stream input)
        {
            input.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }

            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    return ms.ToArray();
            //}
        }

    }
}
