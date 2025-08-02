using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.MediaFeature.UploadMedia;

namespace PropVivo.Application.Dto.MediaFeature.DownloadMedia
{
    public class DownloadMediaDto : UploadMediaDto
    {
        public string? FilePath { get; set; }
    }

    public class DownloadMediaRequest : DownloadMediaDto, IRequest<BaseResponse<DownloadMediaResponse>>
    {
    }
}