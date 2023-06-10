using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MyLeasing.Web.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        /// <summary>
        /// Blob connection configuration
        /// </summary>
        private readonly CloudBlobClient _blobClient;

        public BlobHelper(IConfiguration configuration)
        {
            // Setting Blob connection configuration

            string keys = configuration["Blob:ConnectionString"];

            var storageAccount = CloudStorageAccount.Parse(keys);

            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            var stream = new MemoryStream(file);
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(string imageUrl, string containerName)
        {
            Stream stream = File.OpenRead(imageUrl);
            return await UploadStreamAsync(stream, containerName);
        }

        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {
            var guid = Guid.NewGuid();

            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob block = container.GetBlockBlobReference($"{guid}");

            await block.UploadFromStreamAsync(stream);

            return guid;
        }
    }
}
