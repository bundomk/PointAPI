using CoreFtp;
using ImageSharp;
using ImageSharp.Processing;
using Point.Services.ReturnImageService.Model;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Point.Services.ReturnImageService
{
    public class FtpStorageImageService : IStorageImageService
    {
        private readonly string originalFileName = "org";

        public async Task<byte[]> GetImageAsync(string url, string name)
        {
            byte[] result;

            using (var ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = "kostoski.com",
                Username = "pointadvisor",
                Password = "Qpmn91@5",
                BaseDirectory = $"/Point/Images/{url}"
            }))
            {
                await ftpClient.LoginAsync();

                using (var ftpReadStream = await ftpClient.OpenFileReadStreamAsync($"{name}"))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ftpReadStream.CopyTo(ms);
                        result = ms.ToArray();
                    }
                }

                await ftpClient.LogOutAsync();
            }

            return result;
        }

        public async Task<string> PostImageAsync(byte[] bytes)
        {
            Guid key = Guid.NewGuid();

            Dictionary<string, Stream> images = GenerateImages(bytes);

            using (var ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = "kostoski.com",
                Username = "pointadvisor",
                Password = "Qpmn91@5",
                BaseDirectory = $"/Point/Images/{key}"
            }))
            {
                await ftpClient.LoginAsync();

                //await ftpClient.CreateDirectoryAsync($"{key}");

                //await ftpClient.ChangeWorkingDirectoryAsync($"{key}");

                foreach (var image in images)
                {
                    using (var writeStream = await ftpClient.OpenFileWriteStreamAsync($"{image.Key}.png"))
                    {
                        await image.Value.CopyToAsync(writeStream);
                    }
                }

                await ftpClient.LogOutAsync();
            }

            return key.ToString();
        }

        private Dictionary<string, Stream> GenerateImages(byte[] bytes)
        {
            Dictionary<string, Stream> outputStreams = new Dictionary<string, Stream>();

            //outputStreams.Add(originalFileName + "1", new MemoryStream(bytes));
            //outputStreams.Add(originalFileName, SaveAsPng(bytes));
            //outputStreams.Add("32", GenerateImage(bytes, 32));
            outputStreams.Add("48", GenerateImage(bytes, 48));
            outputStreams.Add("256", GenerateImage(bytes, 256));
            //outputStreams.Add("512", GenerateImage(bytes, 512));

            return outputStreams;
        }

        private Stream SaveAsPng(byte[] bytes)
        {
            Stream outputStream = new MemoryStream();

            using (var image = Image.Load(bytes))
            {
                image.SaveAsPng(outputStream);
            }

            outputStream.Position = 0;

            return outputStream;
        }

        private Stream GenerateImage(byte[] bytes, int size)
        {
            Stream outputStream = new MemoryStream();

            using (var image = Image.Load(bytes))
            {
                image.Resize(new ResizeOptions
                {
                    Size = new Size(size, size),
                    Mode = ResizeMode.Max
                })
                .SaveAsPng(outputStream);
            }

            outputStream.Position = 0;

            return outputStream;
        }

        public static byte[] ReadFully(Stream input)
        {
            input.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}