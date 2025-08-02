using PropVivo.Application.Dto.Enum;

namespace PropVivo.Application.Dto.MediaFeature.UploadMedia
{
    public class MediaItem
    {
        public string? ContentType { get; set; }
        public string? FileExtension { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long? FileSize { get; set; }
        public string? MediaType { get; set; }

        public string? Uri { get; set; }

        public void setMediaType()
        {
            this.MediaType = MediaTypeMapper.GetMediaTypeFromExtension(this.FileExtension).ToString();
        }
    }

    public class UploadMediaResponse
    {
        public bool Error { get; set; }
        public MediaItem? Media { get; set; }
        public string? Status { get; set; }
    }
}