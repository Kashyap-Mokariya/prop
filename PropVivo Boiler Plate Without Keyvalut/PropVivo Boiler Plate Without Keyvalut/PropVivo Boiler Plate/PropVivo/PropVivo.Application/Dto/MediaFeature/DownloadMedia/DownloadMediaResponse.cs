using PropVivo.Application.Dto.Enum;

namespace PropVivo.Application.Dto.MediaFeature.DownloadMedia
{
    public class DownloadMediaResponse
    {
        public Stream? Content { get; set; }
        public string? ContentType { get; set; }
        public bool Error { get; set; }
        public string? FileExtension { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long? FileSize { get; set; }
        public string? MediaType { get; set; }
        public string? Status { get; set; }
        public string? Uri { get; set; }

        public void setMediaType()
        {
            this.MediaType = MediaTypeMapper.GetMediaTypeFromExtension(this.FileExtension).ToString();
        }
    }
}