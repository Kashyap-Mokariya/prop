namespace PropVivo.Application.Dto.Enum
{
    public enum MediaType
    {
        pdf,
        image,
        video,
        audio,
        doc,
        excel,
        ppt,
        txt,
        zip,
        svg,
        other
    }

    public static class MediaTypeMapper
    {
        private static readonly Dictionary<string, MediaType> _mediaTypeMappings = new Dictionary<string, MediaType>(StringComparer.OrdinalIgnoreCase)
    {
        { "pdf", MediaType.pdf },
        { "jpg", MediaType.image },
        { "jpeg", MediaType.image },
        { "png", MediaType.image },
        { "gif", MediaType.image },
        { "svg", MediaType.image },
        { "mp4", MediaType.video },
        { "avi", MediaType.video },
        { "mov", MediaType.video },
        { "mp3", MediaType.audio },
        { "wav", MediaType.audio },
        { "doc", MediaType.doc },
        { "docx", MediaType.doc },
        { "xls", MediaType.excel },
        { "xlsx", MediaType.excel },
        { "ppt", MediaType.ppt },
        { "pptx", MediaType.ppt },
        { "txt", MediaType.txt },
        { "zip", MediaType.zip },
        { "rar", MediaType.zip }
    };

        public static MediaType GetMediaTypeFromExtension(string fileExtension)
        {
            if (_mediaTypeMappings.TryGetValue(fileExtension.ToLower(), out MediaType mediaType))
            {
                return mediaType;
            }

            return MediaType.other;
        }
    }
}