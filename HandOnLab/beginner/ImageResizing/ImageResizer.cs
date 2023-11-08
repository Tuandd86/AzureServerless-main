using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace Company.Function
{
    public class ImageResizer
    {
        [FunctionName("ResizeImage")]
        public static void Run(
        [BlobTrigger("image/{name}", Connection = "AzureWebJobsStorage")] Stream imageStream,
        [Blob("thumbnails/{name}", FileAccess.Write)] Stream outputBlob,
        string name,
        ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {imageStream.Length} Bytes");

            using (var inputStream = new SKManagedStream(imageStream))
            using (var original = SKBitmap.Decode(inputStream))
            {
                // Ensure the bitmap has been decoded successfully
                if (original == null)
                {
                    log.LogError("Failed to decode the image.");
                    return;
                }

                // Resize the bitmap
                var resized = original.Resize(new SKImageInfo(100, 100), SKFilterQuality.High);
                if (resized == null)
                {
                    log.LogError("Failed to resize the image.");
                    return;
                }

                // Convert the resized bitmap to an image
                using (var image = SKImage.FromBitmap(resized))
                {
                    // Encode the image to data using JPEG format with a quality of 75
                    using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 75))
                    {
                        // Ensure the data has been encoded successfully
                        if (data == null)
                        {
                            log.LogError("Failed to encode the image.");
                            return;
                        }

                        // Write the data to the output blob stream
                        data.SaveTo(outputBlob);
                    }
                }
            }
        }
    }
}
