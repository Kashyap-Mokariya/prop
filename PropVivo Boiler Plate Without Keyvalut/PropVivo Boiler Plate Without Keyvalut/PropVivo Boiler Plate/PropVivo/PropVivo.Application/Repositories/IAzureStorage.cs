using Microsoft.AspNetCore.Http;
using PropVivo.Application.Dto.AzureStorage;
using PropVivo.Application.Dto.MediaFeature.DownloadMedia;

namespace PropVivo.Application.Repositories
{
    public interface IAzureStorage
    {
        /// <summary>
        /// This method downloads a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob</returns>
        Task<DownloadMediaResponse> DownloadAsync(string storageContainerName, string blobFilename);

        /// <summary>
        /// This method uploads a file submitted with the request
        /// </summary>
        /// <param name="file">File for upload</param>
        /// <returns>Blob with status</returns>
        Task<BlobResponseDto> UploadAsync(string storageContainerName, string folderName, string fileName, string filePath);

        Task<BlobResponseDto> UploadAsync(string storageContainerName, string folderName, IFormFile blob);
    }
}