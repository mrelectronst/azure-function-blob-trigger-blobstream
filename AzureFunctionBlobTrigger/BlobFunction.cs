using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctionBlobTrigger
{
    public class BlobFunction
    {
        [FunctionName("FunctionBlob")]
        public static async Task FunctionBlob([BlobTrigger("blobitems/{name}", Connection = "AzureLocalStorageConnectionString")] Stream myBlob, string name, ILogger log,
            [Blob("blobitems-resize/{name}", FileAccess.Write, Connection = "AzureLocalStorageConnectionString")] Stream blobStream)
        {
            try
            {
                var imageformat = await Image.DetectFormatAsync(myBlob);

                var resizeImage = Image.Load(myBlob);

                resizeImage.Mutate(x => x.Resize(100, 100));

                resizeImage.Save(blobStream, imageformat);

                log.LogInformation($"Saved Successfully blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            }
            catch (Exception ex) { log.LogInformation($"{ex.Message}"); }
        }
    }
}
