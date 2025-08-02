using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Constants;
using PropVivo.Application.Repositories;
using System.Net;

namespace PropVivo.Application.Dto.MediaFeature.UploadMedia
{
    public sealed class UploadMediaHandler : IRequestHandler<UploadMediaRequest, BaseResponse<UploadMediaResponse>>
    {
        private readonly IAzureStorage _azureStorage;
        private readonly IConfiguration _configuration;

        public UploadMediaHandler(IAzureStorage azureStorage, IConfiguration configuration)
        {
            _azureStorage = azureStorage;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<BaseResponse<UploadMediaResponse>> Handle(UploadMediaRequest uploadMediaRequest, CancellationToken cancellationToken)
        {
            if (uploadMediaRequest == null || uploadMediaRequest.FormFile == null || string.IsNullOrEmpty(uploadMediaRequest.ContainerName))
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));
            var response = new BaseResponse<UploadMediaResponse>();

            var folderName = $"{uploadMediaRequest.FolderName}";
            if (!string.IsNullOrEmpty(uploadMediaRequest.SubFolderName))
                folderName = string.Format("{0}/{1}", folderName, uploadMediaRequest.SubFolderName);

            var file = uploadMediaRequest.FormFile;
            var containerName = uploadMediaRequest.ContainerName;
            var filePath = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}{Path.GetExtension(file.FileName)}";
            var media = new MediaItem
            {
                FileExtension = Path.GetExtension(file.FileName),
                FileName = file.FileName,
                FilePath = filePath,
                FileSize = file.Length,
                ContentType = file.ContentType,
                Uri = string.IsNullOrEmpty(folderName) ? string.Format("{0}{1}/{2}", _configuration["BlobURL"], containerName, filePath) : string.Format("{0}{1}/{2}/{3}", _configuration["BlobURL"], containerName, folderName, filePath)
            };
            media.setMediaType();

            await UploadFileInBlob(containerName, folderName, filePath, file);
            // _ = Task.Run(() => UploadFileInBlob(containerName, folderName, filePath, file));

            response.Data = new UploadMediaResponse { Media = media };
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = $"File Uploaded Successfully";

            return response;
        }

        public async ValueTask UploadFileInBlob(string containerName, string folderName, string fileName, IFormFile blob)
        {
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            try
            {
                // Create a MemoryStream to hold the content of the IFormFile
                using (var stream = new MemoryStream())
                {
                    await blob.CopyToAsync(stream); // Copy the content of IFormFile to MemoryStream
                    stream.Position = 0; // Reset the position to beginning

                    await SaveStreamAsFile(Path.GetTempPath(), stream, fileName);

                    await _azureStorage.UploadAsync(containerName, folderName, fileName, filePath);
                }
            }
            //catch (Exception ex)
            //{
            //    // Handle exceptions or log them appropriately
            //    Console.WriteLine($"Error uploading file: {ex.Message}");
            //    throw; // Rethrow the exception to propagate it further if needed
            //}
            finally
            {
                // Ensure the temporary file is deleted
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private async ValueTask SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            if (inputStream != null)
            {
                string path = Path.Combine(filePath, fileName);
                using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
                {
                    inputStream.Position = 0;
                    await inputStream.CopyToAsync(outputFileStream).ConfigureAwait(false);
                }
            }
        }

        //if (uploadMediaRequest.FormFile.Length == 0)
        //    throw new BadRequestException("Uploaded file is empty.");

        //if (!Path.GetExtension(uploadMediaRequest.FormFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        //    throw new BadRequestException("Invalid file format. Only .xlsx files are allowed.");
    }
}