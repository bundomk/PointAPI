using MetadataExtractor;
using Point.Data.Models;
using Point.Services.ImageService.Model;
using Point.Services.ReturnImageService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Point.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IStorageImageService _storageImageService;

        public ImageService(IStorageImageService storageImageService)
        {
            _storageImageService = storageImageService;
        }

        public async Task<Image> CreateImageAsync(byte[] byteArray, long userId)
        {
            var metadata = GetMetadata(byteArray);

            var path = await _storageImageService.PostImageAsync(byteArray);

            var image = new Image()
            {
                Type = "png",
                Longitude = GetMetadataItem<decimal>(metadata, "GPS - GPS Longitude"),
                Latitude = GetMetadataItem<decimal>(metadata, "GPS - GPS Latitude"),
                Path = path,
                TakenTime = GetMetadataItemDateTime(metadata, "Exif IFD0 - Date/Time"),
                UserId = userId
            };

            return image;
        }

        private DateTime? GetMetadataItemDateTime(IDictionary<string, string> metadata, string key)
        {
            DateTime? dateTime = GetMetadataItem<DateTime>(metadata, key);

            if (dateTime <= DateTime.MinValue)
            {
                dateTime = null;
            }

            return dateTime;
        }

        private T GetMetadataItem<T>(IDictionary<string, string> metadata, string key)
        {
            string value = "";
            object result = default(T);

            if (metadata.ContainsKey(key))
            {
                value = metadata[key];
            }

            if (!string.IsNullOrEmpty(value))
            {
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.DateTime:
                        DateTime.TryParseExact(value, "yyyy:MM:dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dateTime);
                        result = dateTime;
                        break;
                    case TypeCode.String:
                        result = value;
                        break;
                    case TypeCode.Decimal:
                        result = ConvertDegreeAngleToDecimal(value);
                        break;
                }
            }

            return (T)result;
        }

        private decimal ConvertDegreeAngleToDecimal(string value)
        {
            decimal result = 0;
            try
            {
                var degrees = value.Split('°');
                var minutes = degrees[1].Split('\'');
                var seconds = minutes[1].Split('"');
                result = ConvertDegreeAngleToDecimal(degrees[0], minutes[0], seconds[0]);
            }
            catch { }

            return result;
        }

        private decimal ConvertDegreeAngleToDecimal(string degrees, string minutes, string seconds)
        {
            return ConvertDegreeAngleToDecimal(Convert.ToDecimal(degrees), Convert.ToDecimal(minutes), Convert.ToDecimal(seconds));
        }

        private decimal ConvertDegreeAngleToDecimal(decimal degrees, decimal minutes, decimal seconds)
        {
            return degrees + (minutes / 60) + (seconds / 3600);
        }

        public Dictionary<string, string> GetMetadata(byte[] byteArray)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Stream stream = new MemoryStream(byteArray);

            //var a = ImageSharp.Image.Load(byteArray);

            //var format = ImageSharp.Image.DetectFormat(byteArray);

            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(stream);
            
            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    //Debug.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
                    result.Add($"{directory.Name} - {tag.Name}", tag.Description);
                }
            }
            //var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            //var dateTime = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTime);

            //var a = directories.Select(x => x.Tags.ToDictionary(y => x.Name + "-" + y.Name, y => y.Description));
            //directories.SelectMany(x => x.Tags).ToDictionary(x => x.Name, x => x.Description)
            return result;
        }
    }
}