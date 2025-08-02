using MediatR;
using Microsoft.AspNetCore.Http;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Dto.MediaFeature.UploadMedia
{
    public class UploadMediaDto : ExecutionRequest
    {
        public string? ContainerName { get; set; }
        public string? FolderName { get; set; }
        public string? SubFolderName { get; set; }
    }

    public class UploadMediaRequest : UploadMediaDto, IRequest<BaseResponse<UploadMediaResponse>>
    {
        public IFormFile? FormFile { get; set; }
    }
}