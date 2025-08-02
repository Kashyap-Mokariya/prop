using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PropVivo.Application.Dto.AzureStorage;
using PropVivo.Application.Dto.MediaFeature.DownloadMedia;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Enums;

namespace PropVivo.AzureStorage
{
    public class AzureStorage : IAzureStorage
    {
        private readonly ILogger<AzureStorage> _logger;
        private readonly string _storageConnectionString;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
        {
            _storageConnectionString = configuration[$"{Key.BlobConnectionString}"] ?? string.Empty;
            _logger = logger;
        }

        public async Task<DownloadMediaResponse> DownloadAsync(string storageContainerName, string blobFilename)
        {
            DownloadMediaResponse response = new();
            // Get a reference to a container named in appsettings.json
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, storageContainerName);

            try
            {
                // Get a reference to the blob uploaded earlier from the API in the container from
                // configuration settings
                BlobClient blobClient = client.GetBlobClient(blobFilename);

                // Check if the file exists in the container
                if (await blobClient.ExistsAsync())
                {
                    // Download the blob content
                    var downloadResult = await blobClient.DownloadAsync();

                    // Check if download was successful
                    if (downloadResult != null && downloadResult.Value != null)
                    {
                        // Add data to variables in order to return a BlobDto

                        var blobContent = downloadResult.Value.Content;
                        var contentType = downloadResult.Value.Details.ContentType;
                        var contentLength = downloadResult.Value.Details.ContentLength;

                        // Set response properties
                        response.Content = blobContent;
                        response.FileName = blobFilename;
                        response.ContentType = contentType;
                        response.Uri = blobClient.Uri.AbsoluteUri;
                        response.FileSize = contentLength;
                        response.Error = false;
                        response.Status = $"File {blobFilename} downloaded successfully.";
                    }
                    else
                    {
                        // Handle case where download result is null or details are null
                        response.Error = true;
                        response.Status = $"Failed to download file {blobFilename}. Download result or details are null.";
                    }
                }
                else
                {
                    // Handle case where the blob does not exist
                    response.Error = true;
                    response.Status = $"File {blobFilename} does not exist in container {storageContainerName}.";
                }
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // Handle specific case where blob is not found
                _logger.LogError($"File {blobFilename} was not found. Error: {ex.Message}");
                response.Error = true;
                response.Status = $"File {blobFilename} was not found.";
            }
            catch (RequestFailedException ex)
            {
                // Handle other request failures
                _logger.LogError($"Failed to download file {blobFilename}. Error: {ex.Message}");
                response.Error = true;
                response.Status = $"Failed to download file {blobFilename}.";
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                _logger.LogError($"Unexpected error downloading file {blobFilename}. Error: {ex.Message}");
                response.Error = true;
                response.Status = $"Unexpected error downloading file {blobFilename}.";
            }

            // File does not exist, return null and handle that in requesting method
            return response;
        }

        public async Task<BlobResponseDto> UploadAsync(string storageContainerName, string folderName, string fileName, string filePath)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, storageContainerName);

            // Create the container if it does not exist
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            try
            {
                // Get a reference to the blob just uploaded from the API in a container from
                // configuration settings
                BlobClient client = container.GetBlobClient($"{folderName}/{fileName}");

                // Open a stream for the file we want to upload

                await using (FileStream data = File.OpenRead(filePath))
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {filePath} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;
                response.Blob.Path = client.Name;
            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {filePath} already exists in container. Set another name to store the file in the container: '{storageContainerName}.'");
                response.Status = $"File with name {filePath} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        public async Task<BlobResponseDto> UploadAsync(string storageContainerName, string folderName, IFormFile blob)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, storageContainerName);

            // Create the container if it does not exist
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            try
            {
                var filePath = $"{Path.GetFileNameWithoutExtension(blob.FileName)}_{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}{Path.GetExtension(blob.FileName)}";
                // Get a reference to the blob just uploaded from the API in a container from
                // configuration settings
                BlobClient client = container.GetBlobClient($"{folderName}/{filePath}");

                // Open a stream for the file we want to upload
                if (blob != null)
                {
                    await using (Stream? data = blob.OpenReadStream())
                    {
                        // Upload the file async
                        await client.UploadAsync(data);
                    }

                    // Everything is OK and file got uploaded
                    response.Status = $"File {blob.FileName} Uploaded Successfully";
                    response.Error = false;
                    response.Blob.Uri = client.Uri.AbsoluteUri;
                    response.Blob.Name = blob.FileName;
                    response.Blob.Path = client.Name;
                }
            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }
    }
}