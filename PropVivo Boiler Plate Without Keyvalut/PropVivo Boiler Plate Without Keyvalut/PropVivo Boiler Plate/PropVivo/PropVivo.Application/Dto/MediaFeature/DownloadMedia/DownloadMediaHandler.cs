using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Constants;
using PropVivo.Application.Repositories;
using System.Net;

namespace PropVivo.Application.Dto.MediaFeature.DownloadMedia
{
    public sealed class DownloadMediaHandler : IRequestHandler<DownloadMediaRequest, BaseResponse<DownloadMediaResponse>>
    {
        private readonly IAzureStorage _azureStorage;

        public DownloadMediaHandler(IAzureStorage azureStorage)
        {
            _azureStorage = azureStorage;
        }

        public async Task<BaseResponse<DownloadMediaResponse>> Handle(DownloadMediaRequest downloadMediaRequest, CancellationToken cancellationToken)
        {
            if (downloadMediaRequest == null || string.IsNullOrEmpty(downloadMediaRequest.ContainerName) || string.IsNullOrEmpty(downloadMediaRequest.FilePath))
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponse<DownloadMediaResponse>();
            var filePath = $"{downloadMediaRequest.FilePath}";
            if (!string.IsNullOrEmpty(downloadMediaRequest.SubFolderName))
                filePath = string.Format("{0}/{1}", downloadMediaRequest.SubFolderName, filePath);

            if (!string.IsNullOrEmpty(downloadMediaRequest.FolderName))
                filePath = string.Format("{0}/{1}", downloadMediaRequest.FolderName, filePath);

            response.Data = await _azureStorage.DownloadAsync(downloadMediaRequest.ContainerName, filePath);
            if (response.Data.Error && !string.IsNullOrEmpty(response.Data.Status))
                throw new BadRequestException(response.Data.Status);

            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = response.Data.Status;

            return response;
        }
    }
}