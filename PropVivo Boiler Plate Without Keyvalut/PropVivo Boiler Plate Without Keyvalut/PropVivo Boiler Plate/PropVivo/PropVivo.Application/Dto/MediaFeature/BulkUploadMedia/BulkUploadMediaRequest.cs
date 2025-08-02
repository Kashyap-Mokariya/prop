using MediatR;
using Microsoft.AspNetCore.Http;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.MediaFeature.UploadMedia;

namespace PropVivo.Application.Dto.MediaFeature.BulkUploadMedia
{
    public class BulkUploadMediaRequest : UploadMediaDto, IRequest<BaseResponse<BulkUploadMediaResponse>>
    {
        public List<IFormFile>? FormFiles { get; set; }
    }
}